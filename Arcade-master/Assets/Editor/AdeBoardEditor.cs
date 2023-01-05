using Arcade.Compose;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class AdeBoardEditor : EditorWindow
{ 
    private ILocale currentLocale = ILocale.zh_Hans;
    private I18nBoard i18nBoard = new I18nBoard();

    private void OnEnable()
    {
        Task.Run(async () => {
            string str = await new HttpClient().GetStringAsync("https://schwarzer.oss-cn-hangzhou.aliyuncs.com/board_i18n");
            try
            {
                i18nBoard = JsonConvert.DeserializeObject<I18nBoard>(str);
            }
            catch (Exception)
            {

            }
        });
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("内容");  
        currentLocale = (ILocale)EditorGUILayout.EnumPopup("Locale", currentLocale);
        if (!i18nBoard.LocalizedBoard.ContainsKey(currentLocale))
            i18nBoard.LocalizedBoard.Add(currentLocale, string.Empty); 
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.LabelField(i18nBoard.TimeStamp.ToString());
        i18nBoard.LocalizedBoard[currentLocale] =
            EditorGUILayout.TextArea(i18nBoard.LocalizedBoard[currentLocale], GUILayout.Height(200));
        if (GUILayout.Button("更新"))
        {
            i18nBoard.TimeStamp = DateTime.UtcNow.Ticks;
            File.WriteAllText(@"E:\Arcade\Latest\board_i18n", JsonConvert.SerializeObject(i18nBoard, Formatting.Indented));
            ProcessStartInfo p = new ProcessStartInfo
            {
                FileName = @"powershell",
                Arguments = @"E:\Arcade\Latest\updateBoard.bat",
                WorkingDirectory = @"E:\Arcade\Latest",
                UseShellExecute = false,
                RedirectStandardOutput = true
            };
            Task.Run(async () =>
            {
                Process proc = Process.Start(p);
                proc.WaitForExit();
                string str = await new HttpClient().GetStringAsync("https://schwarzer.oss-cn-hangzhou.aliyuncs.com/board_i18n");
                try
                {
                    i18nBoard = JsonConvert.DeserializeObject<I18nBoard>(str);
                }
                catch (Exception)
                {

                }
                UnityEngine.Debug.Log(proc.StandardOutput.ReadToEnd());
            });
        }
    }

    [MenuItem("Arcade/Board")]
    public static void Open()
    {
        GetWindow<AdeBoardEditor>().Show();
    }
}
