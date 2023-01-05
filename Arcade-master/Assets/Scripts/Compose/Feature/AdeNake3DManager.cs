using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Arcade.Compose.Feature
{
    public class AdeNake3DManager : MonoBehaviour
    {
        public bool IsEnabled { get; private set; } = false;

        public static AdeNake3DManager Instance { get; private set; }
        public Camera LCam, RCam;
        public GameObject LBackground, RBackground;
        public Image LBackgroundImage, RBackgroundImage;
        public GameObject[] Controls;
        public GameObject Anchor;
        public int eyeDistance = 70;

        private void Awake()
        {
            Instance = this;
        }
        public void Switch()
        {
            IsEnabled = !IsEnabled;
            foreach (var g in Controls)
            {
                g.SetActive(!IsEnabled);
            }
            RBackground.SetActive(IsEnabled);
            RCam.gameObject.SetActive(IsEnabled);
            if (IsEnabled)
            {
                LCam.rect = new Rect(-0.5f, 0, 1, 1);
                RCam.rect = new Rect(0.5f, 0, 1, 1);
                RBackgroundImage.sprite = LBackgroundImage.sprite;
            }
            else
            {
                LCam.rect = new Rect(0, 0, 1, 1);
            }
            Arcade.Gameplay.ArcCameraManager.Instance.Force4By3 = IsEnabled;
            Arcade.Gameplay.ArcCameraManager.Instance.ResetCamera();
            RCam.fieldOfView = LCam.fieldOfView;
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (Input.GetKeyDown(KeyCode.T)) Switch();
                if (Input.GetKeyDown(KeyCode.LeftBracket)) eyeDistance -= 5;
                if (Input.GetKeyDown(KeyCode.RightBracket)) eyeDistance += 5;
                if (Input.GetKeyDown(KeyCode.Slash)) Anchor.SetActive(!Anchor.activeInHierarchy);
            }
            if (!IsEnabled) return;
            RCam.transform.SetPositionAndRotation(LCam.transform.position, LCam.transform.rotation);
            LCam.transform.localPosition = new Vector3(LCam.transform.localPosition.x + eyeDistance / 100f, LCam.transform.localPosition.y, LCam.transform.localPosition.z);
            RCam.transform.localPosition = new Vector3(RCam.transform.localPosition.x - eyeDistance / 100f, RCam.transform.localPosition.y, RCam.transform.localPosition.z);
        }
    }
}