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
    public class AdeSingleDialog : MonoBehaviour
    {
        public static AdeSingleDialog Instance { get; private set; }
        private void Awake()
        {
            Instance = this;
        }

        public RectTransform Dialog, View;
        public Text Title, Button1;
        public Text Content;

        private Action callback;

        public void Show(string Content, string Title = null, string Button1 = null, Action Callback = null)
        {
            this.Content.text = Content;
            this.Title.text = Title ?? I.S["hint"];
            this.Button1.text = Button1 ?? I.S["confirm"];
            Dialog.sizeDelta = new Vector2(801, 210);
            float height = this.Content.CalculateHeight(Content);
            if (height > 800) height = 700;
            Dialog.sizeDelta = new Vector2(801, 210 + height);
            callback = Callback;
            View.gameObject.SetActive(true);
        }
        public void Hide()
        {
            View.gameObject.SetActive(false);
            callback?.Invoke();
            callback = null;
        }
    }
}