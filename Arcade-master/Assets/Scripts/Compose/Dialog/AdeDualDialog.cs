using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Schwarzer.UnityExtension;

namespace Arcade.Compose.Dialog
{
    public class AdeDualDialog : MonoBehaviour
    {
        public static AdeDualDialog Instance { get; private set; }
        private void Awake()
        {
            Instance = this;
        }

        public RectTransform Dialog, View;
        public Text Title, Button1, Button2;
        public Text Content;

        private Action callback1, callback2;

        public void Show(string Content, string Title = null, string Button1 = null, string Button2 = null, Action Callback1 = null, Action Callback2 = null)
        {
            this.Content.text = Content;
            this.Title.text = Title ?? I.S["hint"];
            this.Button1.text = Button1 ?? I.S["confirm"];
            this.Button2.text = Button2 ?? I.S["cancel"];
            Dialog.sizeDelta = new Vector2(801, 210);
            float height = this.Content.CalculateHeight(Content);
            if (height > 800) height = 700;
            Dialog.sizeDelta = new Vector2(801, 210 + height);
            callback1 = Callback1;
            callback2 = Callback2;
            View.gameObject.SetActive(true);
        }
        public void Ok()
        {
            View.gameObject.SetActive(false);
            callback1?.Invoke();
            callback1 = null;
        }
        public void No()
        {
            View.gameObject.SetActive(false);
            callback2?.Invoke();
            callback2 = null;
        }
    }
}