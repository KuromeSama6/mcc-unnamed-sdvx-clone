using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Arcade.Gameplay
{
    public class ArcArcSegmentComponent : MonoBehaviour
    {
        public Color ShadowColor;
        public Material ArcMaterial, ShadowMaterial;
        public MeshRenderer SegmentRenderer, ShadowRenderer;
        public MeshFilter SegmentFilter, ShadowFilter;
        public Texture2D DefaultTexture, HighlightTexture;
        [HideInInspector] public int FromTiming, ToTiming;
        [HideInInspector] public Vector3 FromPos, ToPos;

        public bool Enable
        {
            get
            {
                return enable;
            }
            set
            {
                if (enable != value)
                {
                    enable = value;
                    SegmentRenderer.enabled = value;
                    ShadowRenderer.enabled = value;
                }
            }
        }
        public float From
        {
            get
            {
                return currentFrom;
            }
            set
            {
                if (currentFrom != value)
                {
                    currentFrom = value;
                    bodyMaterialInstance.SetFloat(fromShaderId, value);
                    shadowMaterialInstance.SetFloat(fromShaderId, value);
                }
            }
        }
        public Color Color
        {
            get
            {
                return currentColor;
            }
            set
            {
                if (currentColor != value)
                {
                    currentColor = value;
                    bodyMaterialInstance.SetColor(colorShaderId, value);
                    Color c = ShadowColor;
                    c.a = value.a * 0.3f;
                    shadowMaterialInstance.SetColor(colorShaderId, c);
                }
            }
        }
        public float Alpha
        {
            get
            {
                return currentColor.a;
            }
            set
            {
                if (currentColor.a != value)
                {
                    currentColor.a = value;
                    bodyMaterialInstance.SetColor(colorShaderId, currentColor);
                    Color c = ShadowColor;
                    c.a = value * 0.3f;
                    shadowMaterialInstance.SetColor(colorShaderId, c);
                }
            }
        }
        public bool Highlight
        {
            get
            {
                return highlighted;
            }
            set
            {
                if (highlighted != value)
                {
                    highlighted = value;
                    bodyMaterialInstance.mainTexture = highlighted ? HighlightTexture : DefaultTexture;
                }
            }
        }
        public int RenderQueue
        {
            get
            {
                return renderQueue;
            }
            set
            {
                if (renderQueue != value)
                {
                    renderQueue = value;
                    bodyMaterialInstance.renderQueue = value;
                    shadowMaterialInstance.renderQueue = value;
                }
            }
        }
        public bool Selected
        {
            get
            {
                return selected;
            }
            set
            {
                if (selected != value)
                {
                    bodyMaterialInstance.SetInt(highlightShaderId, value ? 1 : 0);
                    selected = value;
                }
            }
        }

        public Material CurrentArcMaterial
        {
            set
            {
                if (value == null)
                {
                    if (usingArcInstanceMaterial == false)
                    {
                        SegmentRenderer.material = bodyMaterialInstance;
                        usingArcInstanceMaterial = true;
                    }
                }
                else
                {
                    if (usingArcInstanceMaterial == true)
                    {
                        SegmentRenderer.material = value;
                        usingArcInstanceMaterial = false;
                    }
                }
            }
        }
        public Material CurrentShadowMaterial
        {
            set
            {
                if (value == null)
                {
                    if (usingShadowInstanceMaterial == false)
                    {
                        ShadowRenderer.material = shadowMaterialInstance;
                        usingShadowInstanceMaterial = true;
                    }
                }
                else
                {
                    if (usingShadowInstanceMaterial == true)
                    {
                        ShadowRenderer.material = value;
                        usingShadowInstanceMaterial = false;
                    }
                }
            }
        }

        private bool enable = false;
        private bool selected = false;
        private bool usingArcInstanceMaterial = true;
        private bool usingShadowInstanceMaterial = true;
        private bool highlighted = false;
        private int fromShaderId;
        private int colorShaderId;
        private int highlightShaderId;
        private int renderQueue = 3000;
        private float currentFrom = 0;
        private Color currentColor;
        private Material bodyMaterialInstance, shadowMaterialInstance;

        private void Awake()
        {
            bodyMaterialInstance = Instantiate(ArcMaterial);
            shadowMaterialInstance = Instantiate(ShadowMaterial);
            SegmentRenderer.material = bodyMaterialInstance;
            ShadowRenderer.material = shadowMaterialInstance;
            SegmentRenderer.sortingLayerName = "Arc";
            SegmentRenderer.sortingOrder = 1;
            ShadowRenderer.sortingLayerName = "Arc";
            ShadowRenderer.sortingOrder = 0;
            fromShaderId = Shader.PropertyToID("_From");
            colorShaderId = Shader.PropertyToID("_Color");
            highlightShaderId = Shader.PropertyToID("_Highlight");
        }
        private void OnDestroy()
        {
            Destroy(SegmentFilter.sharedMesh);
            Destroy(ShadowFilter.sharedMesh);
            Destroy(bodyMaterialInstance);
            Destroy(shadowMaterialInstance);
        }

        public void BuildSegment(Vector3 fromPos, Vector3 toPos, float offset, int from, int to)
        {
            FromTiming = from;
            ToTiming = to;
            FromPos = fromPos;
            ToPos = toPos;

            if (fromPos == toPos) return;

            Vector3[] vertices = new Vector3[6];
            Vector2[] uv = new Vector2[6];
            int[] triangles = new int[] { 0, 3, 2, 0, 2, 1, 0, 5, 4, 0, 4, 1 };

            vertices[0] = fromPos + new Vector3(0, offset / 2, 0);
            uv[0] = new Vector2();
            vertices[1] = toPos + new Vector3(0, offset / 2, 0);
            uv[1] = new Vector2(0, 1);
            vertices[2] = toPos + new Vector3(offset, -offset / 2, 0);
            uv[2] = new Vector2(1, 1);
            vertices[3] = fromPos + new Vector3(offset, -offset / 2, 0);
            uv[3] = new Vector2(1, 0);
            vertices[4] = toPos + new Vector3(-offset, -offset / 2, 0);
            uv[4] = new Vector2(1, 1);
            vertices[5] = fromPos + new Vector3(-offset, -offset / 2, 0);
            uv[5] = new Vector2(1, 0);

            Destroy(SegmentFilter.sharedMesh);
            SegmentFilter.sharedMesh = new Mesh()
            {
                vertices = vertices,
                uv = uv,
                triangles = triangles
            };

            Vector3[] shadowvertices = new Vector3[4];
            Vector2[] shadowuv = new Vector2[4];
            int[] shadowtriangles = new int[6];

            shadowvertices[0] = fromPos + new Vector3(-offset, -fromPos.y, 0);
            shadowuv[0] = new Vector2();
            shadowvertices[1] = toPos + new Vector3(-offset, -toPos.y, 0); ;
            shadowuv[1] = new Vector2(0, 1);
            shadowvertices[2] = toPos + new Vector3(offset, -toPos.y, 0);
            shadowuv[2] = new Vector2(1, 1);
            shadowvertices[3] = fromPos + new Vector3(offset, -fromPos.y, 0);
            shadowuv[3] = new Vector2(1, 0);

            shadowtriangles[0] = 0;
            shadowtriangles[1] = 1;
            shadowtriangles[2] = 2;
            shadowtriangles[3] = 0;
            shadowtriangles[4] = 2;
            shadowtriangles[5] = 3;

            Destroy(ShadowFilter.sharedMesh);
            ShadowFilter.sharedMesh = new Mesh()
            {
                vertices = shadowvertices,
                uv = shadowuv,
                triangles = shadowtriangles
            };
        }
    }
}