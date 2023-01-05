using System.Collections.Generic;
using System.Linq;
using Arcade.Gameplay;
using Arcade.Gameplay.Chart;
using UnityEngine;

namespace Arcade.Gameplay
{
    public class ArcArcRenderer : MonoBehaviour
    {
        public const float OffsetNormal = 0.9f;
        public const float OffsetVoid = 0.15f;

        public Material arcMaterial;
        public Material shadowMaterial;
        public GameObject SegmentPrefab;
        public Color ArcRed;
        public Color ArcBlue;
        public Color ShadowColor;
        public readonly Color ArcVoid = new Color(0.5686275f, 0.4705882f, 0.6666667f, 0.4166f);

        public MeshCollider ArcCollider, HeadCollider;
        public MeshFilter HeadFilter;
        public MeshRenderer HeadRenderer;
        public Transform Head;
        public SpriteRenderer HeightIndicatorRenderer;
        public Transform ArcCap;
        public SpriteRenderer ArcCapRenderer;
        public ParticleSystem JudgeEffect;
        public Texture2D DefaultTexture, HighlightTexture;

        public Color ArcGreen;

        public ArcArc Arc
        {
            get
            {
                return arc;
            }
            set
            {
                arc = value;
                Build();
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
                    arcBodySharedMaterial.SetColor(colorShaderId, currentColor);
                    headMaterialInstance.SetColor(colorShaderId, currentColor);
                    foreach (var s in segments)
                    {
                        s.Color = currentColor;
                    }
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
                    arcBodySharedMaterial.SetColor(colorShaderId, currentColor);
                    headMaterialInstance.SetColor(colorShaderId, currentColor);
                    Color c = ShadowColor;
                    c.a = value * 0.3f;
                    arcShadowSharedMaterial.SetColor(colorShaderId, c);
                    foreach (var s in segments)
                    {
                        s.Alpha = value;
                    }
                }
            }
        }
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
                    EnableHead = value;
                    EnableHeightIndicator = value;
                    foreach (ArcArcSegmentComponent s in segments) s.Enable = value;
                    EnableArcCap = value;
                    if (!value) EnableEffect = false;
                    ArcCollider.enabled = value;
                }
            }
        }
        public bool EnableHead
        {
            get
            {
                return headEnable;
            }
            set
            {
                if (headEnable != value)
                {
                    headEnable = value;
                    HeadRenderer.enabled = value;
                    HeadCollider.enabled = value;
                }
            }
        }
        public bool EnableHeightIndicator
        {
            get
            {
                return heightIndicatorEnable;
            }
            set
            {
                if (heightIndicatorEnable != value)
                {
                    heightIndicatorEnable = value;
                    HeightIndicatorRenderer.enabled = value;
                }
            }
        }
        public bool EnableArcCap
        {
            get
            {
                return arcCapEnable;
            }
            set
            {
                if (arcCapEnable != value)
                {
                    arcCapEnable = value;
                    ArcCapRenderer.enabled = value;
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
                    headMaterialInstance.mainTexture = highlighted ? HighlightTexture : DefaultTexture;
                    arcBodySharedMaterial.mainTexture = highlighted ? HighlightTexture : DefaultTexture;
                    foreach (var s in segments) s.Highlight = value;
                }
            }
        }
        public bool EnableEffect
        {
            get
            {
                return effect;
            }
            set
            {
                if (effect != value)
                {
                    effect = value;
                    if (value)
                    {
                        JudgeEffect.Play();
                    }
                    else
                    {
                        JudgeEffect.Stop();
                        JudgeEffect.Clear();
                    }
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
                    arcBodySharedMaterial.renderQueue = value;
                    headMaterialInstance.renderQueue = value;
                    arcShadowSharedMaterial.renderQueue = value;
                    foreach (var s in segments) s.RenderQueue = value;
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
                    int h = value ? 1 : 0;
                    headMaterialInstance.SetInt(highlightShaderId, h);
                    arcBodySharedMaterial.SetInt(highlightShaderId, h);
                    foreach (var s in segments) s.Selected = value;
                    selected = value;
                }
            }
        }
        public bool IsHead
        {
            get
            {
                return arc.RenderHead;
            }
        }

        private void Awake()
        {
            arcBodySharedMaterial = Instantiate(arcMaterial);
            headMaterialInstance = Instantiate(arcMaterial);
            arcShadowSharedMaterial = Instantiate(shadowMaterial);
            HeadRenderer.sortingLayerName = "Arc";
            HeadRenderer.sortingOrder = 1;
            colorShaderId = Shader.PropertyToID("_Color");
            highlightShaderId = Shader.PropertyToID("_Highlight");
        }
        private void OnDestroy()
        {
            Destroy(ArcCollider.sharedMesh);
            Destroy(HeadCollider.sharedMesh);
            Destroy(HeadFilter.sharedMesh);  
            Destroy(arcBodySharedMaterial);
            Destroy(headMaterialInstance);
            Destroy(arcShadowSharedMaterial);
        }

        private int highlightShaderId;
        private int colorShaderId;
        private int segmentCount = 0;
        private int renderQueue = 3000;
        private bool enable;
        private bool selected;
        private bool headEnable;
        private bool heightIndicatorEnable;
        private bool arcCapEnable;
        private bool highlighted;
        private bool effect;
        private ArcArc arc;
        private Color currentColor;
        private List<ArcArcSegmentComponent> segments = new List<ArcArcSegmentComponent>();
        private Material headMaterialInstance;
        private Material arcBodySharedMaterial;
        private Material arcShadowSharedMaterial; 

        private void InstantiateSegment(int quantity)
        {
            int count = segments.Count;
            if (count == quantity) return;
            else if (count < quantity)
            {
                for (int i = 0; i < quantity - count; ++i)
                {
                    GameObject g = Instantiate(SegmentPrefab, transform);
                    segments.Add(g.GetComponent<ArcArcSegmentComponent>());
                }
            }
            else if (count > quantity)
            {
                for (int i = 0; i < count - quantity; ++i)
                {
                    Destroy(segments.Last().gameObject);
                    segments.RemoveAt(segments.Count - 1);
                }
            }
            foreach (ArcArcSegmentComponent s in segments)
            {
                s.transform.SetAsLastSibling();
            }
        }

        public void Build()
        {
            BuildHeightIndicator();
            BuildSegments();
            BuildHead();
            BuildCollider();
        }
        public void BuildHeightIndicator()
        {
            if (arc.IsVoid)
            {
                EnableHeightIndicator = false;
                return;
            }
            HeightIndicatorRenderer.transform.localPosition = new Vector3(ArcAlgorithm.ArcXToWorld(arc.XStart), 0, 0);
            HeightIndicatorRenderer.transform.localScale = new Vector3(2.34f, 100 * (ArcAlgorithm.ArcYToWorld(arc.YStart) - OffsetNormal / 2), 1);
            HeightIndicatorRenderer.color = arc.Color == 1 ? ArcRed : arc.Color == 0 ? ArcBlue : ArcGreen;
        }
        public void BuildSegments()
        {
            if (arc == null) return;

            ArcTimingGroup tg = ArcTimingManager.Instance[arc.TimingGroup];
            int offset = ArcAudioManager.Instance.AudioOffset;
            int duration = arc.EndTiming - arc.Timing;

            int v1 = duration < 1000 ? 14 : 7;
            float v2 = 1f / (v1 * duration / 1000f);
            int segSize = (int)(duration * v2);
            segmentCount = (segSize == 0 ? 0 : duration / segSize) + 1;
            InstantiateSegment(segmentCount);

            Vector3 start = new Vector3();
            Vector3 end = new Vector3(ArcAlgorithm.ArcXToWorld(arc.XStart),
                                        ArcAlgorithm.ArcYToWorld(arc.YStart));

            for (int i = 0; i < segmentCount - 1; ++i)
            {
                start = end;
                end = new Vector3(ArcAlgorithm.ArcXToWorld(ArcAlgorithm.X(arc.XStart, arc.XEnd, (i + 1f) * segSize / duration, arc.LineType)),
                                  ArcAlgorithm.ArcYToWorld(ArcAlgorithm.Y(arc.YStart, arc.YEnd, (i + 1f) * segSize / duration, arc.LineType)),
                                  -tg.CalculatePositionByTimingAndStart(arc.Timing + offset, arc.Timing + offset + segSize * (i + 1)) / 1000f);
                segments[i].BuildSegment(start, end, arc.IsVoid ? OffsetVoid : OffsetNormal, arc.Timing + segSize * i, arc.Timing + segSize * (i + 1));
            }

            start = end;
            end = new Vector3(ArcAlgorithm.ArcXToWorld(arc.XEnd),
                              ArcAlgorithm.ArcYToWorld(arc.YEnd),
                              -tg.CalculatePositionByTimingAndStart(arc.Timing + offset, arc.EndTiming + offset) / 1000f);
            segments[segmentCount - 1].BuildSegment(start, end, arc.IsVoid ? OffsetVoid : OffsetNormal, arc.Timing + segSize * (segmentCount - 1), arc.EndTiming);

            Color = (arc.IsVoid ? ArcVoid : (arc.Color == 0 ? ArcBlue : arc.Color == 1 ? ArcRed : ArcGreen));
        }
        public void BuildHead()
        {
            Vector3 pos = new Vector3(ArcAlgorithm.ArcXToWorld(arc.XStart), ArcAlgorithm.ArcYToWorld(arc.YStart));
            float offset = arc.IsVoid ? OffsetVoid : OffsetNormal;

            Vector3[] vertices = new Vector3[4];
            Vector2[] uv = new Vector2[4];
            int[] triangles = new int[] { 0, 2, 1, 0, 3, 2, 0, 1, 2, 0, 2, 3 };

            vertices[0] = pos + new Vector3(0, offset / 2, 0);
            uv[0] = new Vector2();
            vertices[1] = pos + new Vector3(offset, -offset / 2, 0);
            uv[1] = new Vector2(1, 0);
            vertices[2] = pos + new Vector3(0, -offset / 2, offset / 2);
            uv[2] = new Vector2(1, 1);
            vertices[3] = pos + new Vector3(-offset, -offset / 2, 0);
            uv[3] = new Vector2(1, 1);

            Destroy(HeadFilter.sharedMesh);
            HeadFilter.sharedMesh = new Mesh()
            {
                vertices = vertices,
                uv = uv,
                triangles = triangles.Take(6).ToArray()
            };

            Destroy(HeadCollider.sharedMesh);
            HeadCollider.sharedMesh = new Mesh() 
            { 
                vertices = vertices,
                uv = uv,
                triangles = triangles
            };

            HeadRenderer.material = headMaterialInstance;
        }
        public void BuildCollider()
        {
            if (arc.Timing > arc.EndTiming) return;

            List<Vector3> vert = new List<Vector3>();
            List<int> tri = new List<int>();

            float offset = arc.IsVoid ? OffsetVoid : OffsetNormal;

            Vector3 pos = segments[0].FromPos;
            vert.Add(pos + new Vector3(-offset, -offset / 2, 0));
            vert.Add(pos + new Vector3(0, offset / 2, 0));
            vert.Add(pos + new Vector3(offset, -offset / 2, 0));

            int t = 0;
            foreach (var seg in segments)
            {
                if (seg.FromTiming > seg.ToTiming) break;
                if (seg.FromTiming == seg.ToTiming)
                {
                    if (seg.FromPos.Equals(seg.ToPos))
                        break;
                }
                pos = seg.ToPos;
                vert.Add(pos + new Vector3(-offset, -offset / 2, 0));
                vert.Add(pos + new Vector3(0, offset / 2, 0));
                vert.Add(pos + new Vector3(offset, -offset / 2, 0));

                tri.AddRange(new int[] { t + 1, t, t + 3, t + 1, t + 3, t, t + 1, t + 3, t + 4, t + 1, t + 4, t + 3,
                    t + 1, t + 2, t + 5, t + 1, t + 5, t + 2, t + 1, t + 5, t + 4, t + 1, t + 4, t + 5 });
                t += 3;
            }

            Destroy(ArcCollider.sharedMesh);
            ArcCollider.sharedMesh = new Mesh()
            {
                vertices = vert.ToArray(),
                triangles = tri.ToArray()
            };
        }

        public void UpdateArc()
        {
            if (!enable) return;
            UpdateHead();
            UpdateSegments();
            UpdateHeightIndicator();
            UpdateArcCap();
        }
        private void UpdateSegments()
        {
            int currentTiming = ArcGameplayManager.Instance.Timing;
            ArcTimingGroup tg = ArcTimingManager.Instance[arc.TimingGroup];
            int offset = ArcAudioManager.Instance.AudioOffset;
            float z = arc.transform.localPosition.z;

            foreach (ArcArcSegmentComponent s in segments)
            {
                if (-s.ToPos.z < z)
                {
                    if (arc.Judging || arc.IsVoid)
                    {
                        s.Enable = false;
                        continue;
                    }
                    else
                    {
                        s.Enable = true;
                        continue;
                    }
                }
                if (-s.FromPos.z < z && -s.ToPos.z >= z)
                {
                    s.Enable = true;
                    s.CurrentArcMaterial = null;
                    s.CurrentShadowMaterial = null;
                    s.Alpha = currentColor.a;
                    if (arc.Judging || arc.IsVoid)
                    {
                        s.From = (z + s.FromPos.z) / (-s.ToPos.z + s.FromPos.z);
                    }
                    else
                    {
                        s.From = 0;
                    }
                    continue;
                }
                float pos = -(z + s.FromPos.z);
                if (pos > 90 && pos < 100)
                {
                    s.Enable = true;
                    s.CurrentArcMaterial = null;
                    s.CurrentShadowMaterial = null;
                    s.Alpha = currentColor.a * (100 - pos) / 10f;
                    s.From = 0;
                }
                else if (pos > 100 || pos < -20)
                {
                    s.Enable = false;
                }
                else
                {
                    s.Enable = true;
                    s.Alpha = currentColor.a;
                    s.From = 0;
                    s.CurrentArcMaterial = arcBodySharedMaterial;
                    s.CurrentShadowMaterial = arcShadowSharedMaterial;
                }
            }
        }
        private void UpdateHead()
        {
            if (!IsHead)
            {
                EnableHead = false;
                return;
            }

            int currentTiming = ArcGameplayManager.Instance.Timing;
            int offset = ArcAudioManager.Instance.AudioOffset;

            if (arc.Position > 100000 || arc.Position < -10000)
            {
                EnableHead = false;
                return;
            }
            EnableHead = true;
            if (arc.Position > 90000 && arc.Position <= 100000)
            {
                Head.localPosition = new Vector3();
                HeadRenderer.material = headMaterialInstance;
                Color c = currentColor;
                c.a = currentColor.a * (100000 - arc.Position) / 100000;
                headMaterialInstance.SetColor(colorShaderId, c);
            }
            else if (arc.Position < 0)
            {
                headMaterialInstance.SetColor(colorShaderId, currentColor);
                if (arc.Judging || arc.IsVoid)
                {
                    if (segmentCount >= 1)
                    {
                        ArcArcSegmentComponent s = segments[0];
                        int duration = s.ToTiming - s.FromTiming;
                        float t = duration == 0 ? 0 : ((-arc.Position / 1000f) / (-s.ToPos.z));
                        if (t > 1)
                        {
                            EnableHead = false;
                            return;
                        }
                        else if (t < 0) t = 0;
                        Head.localPosition = (s.ToPos - s.FromPos) * t;
                    }
                }
                else
                {
                    Head.localPosition = new Vector3();
                }
            }
            else
            {
                headMaterialInstance.SetColor(colorShaderId, currentColor);
                Head.localPosition = new Vector3();
            }
        }
        private void UpdateHeightIndicator()
        {
            if (arc.IsVoid || (arc.YEnd == arc.YStart && !IsHead))
            {
                EnableHeightIndicator = false;
                return;
            }

            float pos = transform.position.z;
            int currentTiming = ArcGameplayManager.Instance.Timing;
            if (pos < -90 && pos > -100)
            {
                Color c = currentColor;
                c.a = currentColor.a * (pos + 100) / 10;
                EnableHeightIndicator = true;
                HeightIndicatorRenderer.color = c;
            }
            else if (pos < -100 || pos > 10)
            {
                EnableHeightIndicator = false;
            }
            else
            {
                if (arc.Judging && pos > 0) EnableHeightIndicator = false;
                else EnableHeightIndicator = true;
                HeightIndicatorRenderer.color = currentColor;
            }
        }
        private void UpdateArcCap()
        {
            int currentTiming = ArcGameplayManager.Instance.Timing;
            int duration = arc.EndTiming - arc.Timing;
            int offset = ArcAudioManager.Instance.AudioOffset;

            if (duration == 0)
            {
                EnableArcCap = false;
                return;
            }

            if (arc.Position > 0 && arc.Position < 100000)
            {
                if (IsHead && !arc.IsVoid)
                {
                    float p = 1 - arc.Position / 100000;
                    float scale = 0.35f + 0.5f * (1 - p);
                    EnableArcCap = true;
                    ArcCapRenderer.color = new Color(1, 1, 1, p);
                    ArcCap.localScale = new Vector3(scale, scale);
                    ArcCap.position = new Vector3(ArcAlgorithm.ArcXToWorld(arc.XStart), ArcAlgorithm.ArcYToWorld(arc.YStart));
                }
                else
                {
                    EnableArcCap = false;
                }
            }
            else if (arc.Position < 0 && arc.EndPosition > 0)
            {
                EnableArcCap = true;
                ArcCapRenderer.color = new Color(1, 1, 1, arc.IsVoid ? 0.5f : 1f);
                ArcCap.localScale = new Vector3(arc.IsVoid ? 0.21f : 0.35f, arc.IsVoid ? 0.21f : 0.35f);

                foreach (var s in segments)
                {
                    if (arc.Position / 1000f < s.FromPos.z && arc.Position / 1000f >= s.ToPos.z)
                    {
                        float t = (s.FromPos.z - arc.Position / 1000f) / (s.FromPos.z - s.ToPos.z);
                        ArcCap.position = new Vector3(s.FromPos.x + (s.ToPos.x - s.FromPos.x) * t,
                                                      s.FromPos.y + (s.ToPos.y - s.FromPos.y) * t);
                        if (!arc.IsVoid) ArcArcManager.Instance.ArcJudgePos += ArcCap.position.x;
                        break;
                    }
                }
            }
            else
            {
                EnableArcCap = false;
            }
        }

        public bool IsMyself(GameObject gameObject)
        {
            return ArcCollider.gameObject.Equals(gameObject) || HeadCollider.gameObject.Equals(gameObject);
        }
    }
}