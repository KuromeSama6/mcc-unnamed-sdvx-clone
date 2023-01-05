﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Arcade.Compose.Dialog
{
    public class AdeHotkeyDialog : MonoBehaviour
    {
        public GameObject View;
        public InputField NoReturn;
        public void Show()
        {
            View.SetActive(true);
            NoReturn.text = PlayerPrefs.GetString("HotKeyNoReturn", "q");
        }
        public void OnNoReturnHotKey(InputField inputField)
        {
            PlayerPrefs.SetString("HotKeyNoReturn", inputField.text.ToLower());
            inputField.text = inputField.text.ToLower();
        }
    }
}