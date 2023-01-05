using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum ILocale
{
    zh_Hans,
    en
}

public class I : MonoBehaviour
{
    public static I S { get; private set; }

    public ILocale currentLocale = ILocale.zh_Hans; 
    private Dictionary<string, string> strings = new Dictionary<string, string>();

    public UnityEvent OnLocaleChanged = new UnityEvent();

    public void Awake()
    {
        S = this;
        currentLocale = (ILocale)PlayerPrefs.GetInt("Locale", 0);
        ReloadLocale();
    }
    private void OnDestroy()
    {
        PlayerPrefs.SetInt("Locale", (int)currentLocale);
    }
    private void ReloadLocale()
    {
        try
        {
            strings = JsonConvert.DeserializeObject<Dictionary<string, string>>(
                File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "Locales", currentLocale.ToString()) + ".json")
                );
        }
        catch (Exception Ex)
        {
            string log = Path.Combine(Application.temporaryCachePath, "error_log.txt");
            string error = "本地化资源文件读取发生错误\n";
            error += "Locale file loading error\n\n";
            error += Ex.ToString();
            File.WriteAllText(log, error);
            Schwarzer.Windows.Dialog.OpenExplorer(log);
        }
    }
     
    public string this[string id]
    {
        get
        {
            if (!strings.ContainsKey(id))
            {
                Debug.LogWarning("I18n Key Notfound : " + id);
                return "<MISSING LOCALE>";
            }
            return strings[id];
        }
    } 

    public void OnHelpTranslate()
    {
        Application.OpenURL("https://www.transifex.com/schwarzer/arcade");
    }

    public void OnLanguageChanged(Dropdown dropdown)
    {
        currentLocale = (ILocale)dropdown.value;
        ReloadLocale();
        OnLocaleChanged.Invoke();
    }
    public void OnOpenLanguageDialog(Dropdown dropdown)
    {
        dropdown.value = (int)currentLocale;
    }
}
