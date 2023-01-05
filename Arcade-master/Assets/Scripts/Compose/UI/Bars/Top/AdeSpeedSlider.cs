﻿using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Arcade.Gameplay;
namespace Arcade.Compose.UI
{
    [RequireComponent(typeof(Slider))]
    public class AdeSpeedSlider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IEndDragHandler
    {
        public static AdeSpeedSlider Instance { get; private set; }
        private void Awake()
        {
            Instance = this;
            speed = GetComponent<Slider>();
        } 

        public Text Value;
        private Slider speed;
        private bool pointerDown;

        private void Update()
        {
            if (!pointerDown) speed.value = ArcTimingManager.Instance.DropRate;
            Value.text = speed.value.ToString();
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            pointerDown = true;
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            pointerDown = false;
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            ArcTimingManager.Instance.DropRate = (int)speed.value;
            ArcArcManager.Instance.Rebuild();
            PlayerPrefs.SetInt("DropRate", ArcTimingManager.Instance.DropRate);
        }
    }
}