﻿using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Arcade.Compose
{
    public class EndOfFrame : MonoBehaviour
    {
        public static EndOfFrame Instance { get; private set; }
        private void Awake()
        {
            Instance = this;
        }

        [HideInInspector]
        public UnityEvent Listeners = new UnityEvent();

        private void Update()
        {
            Listeners.Invoke();
            Listeners.RemoveAllListeners();
        }
    }
}