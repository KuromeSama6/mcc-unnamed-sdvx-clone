using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdeFPSDialog : MonoBehaviour
{
    public InputField iFrameRate;
    public Toggle tVsync;
    public Toggle tFrameCounter;
    public Text tFpsCounter;

    public bool useFrameCounter = false;
    public double timeCount = 0;
    public int frameCount = 0;
    public const int frameCountThreshold = 30;

    public DateTime lastDateTime = DateTime.MinValue;
    // Start is called before the first frame update
    private void Start()
    { 
        if (!PlayerPrefs.HasKey("Framerate"))
        {
            int framerate = Screen.currentResolution.refreshRate;
            if (framerate < 60)
                framerate = 60;
            PlayerPrefs.SetInt("Framerate", framerate);
        }

        if (!PlayerPrefs.HasKey("Vsync"))
        {
            int vsync = QualitySettings.vSyncCount;
            if (vsync > 1)
                vsync = 1;
            else if (vsync < 0)
                vsync = 0;
            PlayerPrefs.SetInt("Vsync", vsync);
        }

        if (!PlayerPrefs.HasKey("FrameCounter"))
        {
            PlayerPrefs.SetInt("FrameCounter", 0);
        }

        Application.targetFrameRate = PlayerPrefs.GetInt("Framerate", 60);
        iFrameRate.text = Application.targetFrameRate.ToString();
        QualitySettings.vSyncCount = PlayerPrefs.GetInt("Vsync", 1);
        tVsync.isOn = QualitySettings.vSyncCount == 1;
        useFrameCounter = PlayerPrefs.GetInt("FrameCounter") != 0;
        tFrameCounter.isOn = useFrameCounter;
    }

    // Update is called once per frame
    private void Update()
    {
        DateTime currentDateTime = DateTime.Now;
        if (lastDateTime != DateTime.MinValue)
        {
            if (useFrameCounter)
            {
                if (frameCount >= frameCountThreshold)
                {
                    tFpsCounter.text = (1.0 / timeCount * frameCountThreshold).ToString("f1");
                    frameCount = 0;
                    timeCount = 0;
                }
                frameCount++;
                TimeSpan deltaTime = currentDateTime - lastDateTime;
                timeCount += deltaTime.TotalSeconds;
            }
            else
            {
                tFpsCounter.text = string.Empty;
                timeCount = 0;
                frameCount = 0;
            }
        }
        lastDateTime = currentDateTime;
    }

    public void OnFramerateChanged()
    {
        if (int.TryParse(iFrameRate.text, out int newFps))
        {
            newFps = Mathf.Clamp(newFps, 30, 300);
            PlayerPrefs.SetInt("Framerate", newFps);
            Application.targetFrameRate = newFps;
            iFrameRate.text = newFps.ToString();
        }
    }
    public void OnVsyncToggle()
    {
        QualitySettings.vSyncCount = tVsync.isOn ? 1 : 0;
        PlayerPrefs.SetInt("Vsync", QualitySettings.vSyncCount);
    }
    public void OnFramerateCounterToggle()
    {
        useFrameCounter = tFrameCounter.isOn;
        PlayerPrefs.SetInt("FrameCounter", useFrameCounter ? 1 : 0);
    }
}
