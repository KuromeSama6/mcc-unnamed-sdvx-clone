using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;


public class I18nEditor : EditorWindow
{
    [MenuItem("I18n/Strings")]
    public static void Open()
    {
        GetWindow<I18nEditor>().Show();
    }

    private const string localeFilePath = "Assets/StreamingAssets/Locales";
    private ILocale currentLocale = ILocale.zh_Hans;
    private Dictionary<string, string> strings = new Dictionary<string, string>();
    private string newStringKey = string.Empty;
    private string newStringValue = string.Empty;
    private Vector2 stringsScrollPosition = Vector2.zero;

    private void Save()
    {
        string file = Path.Combine(localeFilePath, currentLocale.ToString()) + ".json";
        File.WriteAllText(file, JsonConvert.SerializeObject(strings, Formatting.Indented));
    }

    private void Refresh()
    {
        string file = Path.Combine(localeFilePath, currentLocale.ToString()) + ".json";
        if (!File.Exists(file))
        {
            File.WriteAllText(file, "{}");
        }
        strings = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(file));
    }

    public void OnEnable()
    {
        Refresh();
    }

    public void OnGUI()
    {
        GUILayout.BeginHorizontal();
        ILocale old = currentLocale;
        currentLocale = (ILocale)EditorGUILayout.EnumPopup("Locale", currentLocale);
        if(old != currentLocale)
        {
            Refresh();
            return;
        }

        if (GUILayout.Button("Refresh"))
        {
            Refresh();
            return;
        }

        if (GUILayout.Button("Save"))
        {
            Save();
            return;
        } 

        if(GUILayout.Button("Sort"))
        {
            strings = strings.OrderBy(kvp => kvp.Key).ToDictionary((k) => k.Key, (k) => k.Value);
            Save();
            return;
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        newStringKey = EditorGUILayout.TextField(newStringKey);
        newStringValue = EditorGUILayout.TextField(newStringValue);
        if (GUILayout.Button("Add", GUILayout.Width(30)))
        {
            strings.Add(newStringKey, newStringValue);
        }
        GUILayout.EndHorizontal();

        stringsScrollPosition= EditorGUILayout.BeginScrollView(stringsScrollPosition); 
        string[] keys = strings.Keys.ToArray();   
        foreach (var k in keys)
        {
            GUILayout.BeginHorizontal();
            string oldStr = strings[k];
            string newStr = EditorGUILayout.TextField(k.ToString(), oldStr.Replace("\n", "\\n").Replace("\t", "\\t"))
                .Replace("\\t", "\t").Replace("\\n", "\n").Trim(' ').Replace("\"", "");
            if (!oldStr.Equals(newStr))
            {
                strings[k] = newStr;
                Save();
            }
            if (GUILayout.Button("X", GUILayout.Width(30)))
            {
                strings.Remove(k);
            }
            GUILayout.EndHorizontal();
        } 
        EditorGUILayout.EndScrollView();
    }
}
