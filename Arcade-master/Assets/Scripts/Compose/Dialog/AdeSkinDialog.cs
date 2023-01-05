using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Arcade.Gameplay;

namespace Arcade.Compose.Dialog
{
    public class SkinPreference
    {
        public Dictionary<string, int> SkinValues = new Dictionary<string, int>()
        {
            {"Tap",0 },
            {"Hold",0 },
            {"ArcTap",0 },
            {"Track",0 },
            {"CriticalLine",0},
            {"Background",2 },
            {"Combo" ,0}
        };
    }

    [Serializable]
    public class DropdownDictionary
    {
        public Dropdown[] dropdowns;
        public Dropdown this[string name]
        {
            get
            {
                foreach (var d in dropdowns) if (d.name == name) return d;
                return null;
            }
        }
    }

    public class AdeSkinDialog : MonoBehaviour
    {
        public static AdeSkinDialog Instance { get; private set; }
        public string PreferencesSavePath
        {
            get
            {
                return ArcadeComposeManager.ArcadePersistentFolder + "/Skin.json";
            }
        }
        public string ExternalBackgroundPath
        {
            get
            {
                return new DirectoryInfo(Application.dataPath).Parent.FullName + "/自定义背景(User Backgrounds)";
            }
        }

        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            if (!Directory.Exists(ExternalBackgroundPath)) Directory.CreateDirectory(ExternalBackgroundPath);
            LoadExternalBackgrounds();
            LoadPreferences();
        }

        private SkinPreference p = new SkinPreference();
        private bool shouldSave = true;
        public DropdownDictionary Dropdowns = new DropdownDictionary();

        private List<Sprite> externalBackgrounds = new List<Sprite>();
        public void LoadExternalBackgrounds()
        {
            UnloadExternalBackgrounds();
            Dropdown d = Dropdowns["Background"];
            foreach (var f in Directory.GetFiles(ExternalBackgroundPath))
            {
                if (f.EndsWith(".jpg") || f.EndsWith(".png"))
                {
                    try
                    {
                        Texture2D t = new Texture2D(1, 1);
                        t.LoadImage(File.ReadAllBytes(f), true);
                        Sprite s = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0.5f, 0.5f));
                        externalBackgrounds.Add(s);
                        d.options.Insert(0, new Dropdown.OptionData("*" + Path.GetFileNameWithoutExtension(f), s));
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            ArcSkinManager.Instance.SetBackgroundSkin(d.options[d.value].image);
        }
        public void UnloadExternalBackgrounds()
        {
            Dropdown d = Dropdowns["Background"];
            d.options.RemoveAll((b) => b.text.StartsWith("*"));
            foreach (var s in externalBackgrounds)
            {
                Destroy(s.texture);
                Destroy(s);
            }
            externalBackgrounds.Clear();
        }
        private void OnDestroy()
        {
            UnloadExternalBackgrounds();
        }
        public void OpenExternalBackgroundFolder()
        {
            Schwarzer.Windows.Dialog.OpenExplorer(ExternalBackgroundPath);
        }

        public void OnItemSelect(Dropdown dropdown)
        {
            string name = dropdown.name;
            if (shouldSave) p.SkinValues[name] = dropdown.value;
            switch (name)
            {
                case "Tap":
                    ArcSkinManager.Instance.SetTapNoteSkin(dropdown.value);
                    break;
                case "Hold":
                    ArcSkinManager.Instance.SetHoldNoteSkin(dropdown.value);
                    break;
                case "ArcTap":
                    ArcSkinManager.Instance.SetArcTapSkin(dropdown.value);
                    break;
                case "Track":
                    ArcSkinManager.Instance.SetTrackSkin(dropdown.value);
                    break;
                case "CriticalLine":
                    ArcSkinManager.Instance.SetCriticalLineSkin(dropdown.value);
                    break;
                case "Background":
                    ArcSkinManager.Instance.SetBackgroundSkin(dropdown.options[dropdown.value].image);
                    break;
                case "Combo":
                    ArcSkinManager.Instance.SetComboTextSkin(dropdown.value);
                    break;
            }
        }

        public void LoadPreferences()
        {
            try
            {
                if (File.Exists(PreferencesSavePath))
                {
                    PlayerPrefs.SetString("AdeSkinDialog", File.ReadAllText(PreferencesSavePath));
                    File.Delete(PreferencesSavePath);
                }
                p = JsonConvert.DeserializeObject<SkinPreference>(PlayerPrefs.GetString("AdeSkinDialog", ""));
                if (p == null) p = new SkinPreference();
                shouldSave = false;
                foreach (var s in p.SkinValues) Dropdowns[s.Key].value = s.Value;
                shouldSave = true;
            }
            catch (Exception Ex)
            {
                p = new SkinPreference();
                Debug.Log(Ex);
            }
        }
        public void SavePreferences()
        {
            PlayerPrefs.SetString("AdeSkinDialog", JsonConvert.SerializeObject(p));
        }
        private void OnApplicationQuit()
        {
            SavePreferences();
        }
    }
}