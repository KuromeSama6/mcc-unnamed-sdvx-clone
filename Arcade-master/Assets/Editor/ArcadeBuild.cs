using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Diagnostics;

public class ArcadeBuild
{
    [MenuItem("Arcade/Build")]
    public static void BuildArcade()
    {
        DateTime buildTime = DateTime.Now;
        File.WriteAllText(@"E:\Arcade\Assets\Scripts\Compose\ArcadeBuildInfo.cs",
            "namespace Arcade.Compose { public static class ArcadeBuildInfo { public const string BuildString = \"" + $"{PlayerSettings.bundleVersion} Build Time {buildTime.ToString("yyyyMMddHHmmss")}" + "\"; public const long Timestamp = " + buildTime.Ticks + "; } }");
        UnityEngine.Debug.Log(BuildPipeline.BuildPlayer(
             new BuildPlayerOptions()
             {
                 locationPathName = @"E:\Arcade\Latest\x86\Arcade\Arcade.exe",
                 scenes = new string[] { "Assets/_Scenes/ArcEditor.unity" },
                 target = BuildTarget.StandaloneWindows,
                 options = BuildOptions.None,
             }).summary.result.ToString());
        UnityEngine.Debug.Log(BuildPipeline.BuildPlayer(
             new BuildPlayerOptions()
             {
                 locationPathName = @"E:\Arcade\Latest\x64\Arcade\Arcade.exe",
                 scenes = new string[] { "Assets/_Scenes/ArcEditor.unity" },
                 target = BuildTarget.StandaloneWindows64,
                 options = BuildOptions.None,
             }).summary.result.ToString());
    }

    [MenuItem("Arcade/Update")]
    public static void UpdateArcade()
    {
        Process.Start("powershell", @"E:\Arcade\pushArcade.ps1");
    }

    [MenuItem("Arcade/One Key")]
    public static void OneKeyArcade()
    {
        BuildArcade();
        UpdateArcade();
    }

    [MenuItem("Arcade/Landing")]
    public static void Landing()
    {
        ProcessStartInfo processStartInfo = new ProcessStartInfo
        {
            FileName = @"E:\Arcade\Latest\Landing\ArcadeLanding.exe",
            WorkingDirectory = @"E:\Arcade\Latest\Landing"
        };
        Process.Start(processStartInfo);
    } 
}
