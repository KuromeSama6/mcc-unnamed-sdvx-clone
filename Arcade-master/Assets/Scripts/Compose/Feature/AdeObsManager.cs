using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OBSWebsocketDotNet;
using Arcade.Compose;
using Arcade.Gameplay;
using System.Threading.Tasks; 
using Arcade.Compose.Dialog;

namespace Arcade.Compose.Feature
{
    public class AdeObsManager : MonoBehaviour
    {
        private OBSWebsocket obs = new OBSWebsocket();
        public static AdeObsManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        { 
            Connect();
        }
        private void OnDestroy()
        {
            if (obs.IsConnected) obs.Disconnect();
        }

        public void Record()
        {
            if (!obs.IsConnected)
            {
                AdeSingleDialog.Instance.Show(I.S["obserrhint"], I.S["obserrhintcaption"], I.S["retry"], Connect);
                return;
            }
            if (AdeProjectManager.Instance.CurrentProject == null)
            {
                AdeSingleDialog.Instance.Show(I.S["loadchartfirst"], I.S["error"]);
                return;
            }
            if (recording != null) StopCoroutine(recording);
            recording = StartCoroutine(RecordCoroutine());
        }
        public void ForceClose()
        {
            if (recording != null)
            {
                StopCoroutine(recording);
                if (obs.IsConnected) obs.StopRecording();
                AdeSingleDialog.Instance.Show(I.S["obsrecordbreak"], I.S["hint"]);
                recording = null;
            }
        }
        private void Connect()
        {
            Task.Run(() => obs.Connect("ws://localhost:4444", null));
        }

        private Coroutine recording = null;
        private IEnumerator RecordCoroutine()
        {
            try
            {
                if (obs.IsConnected) obs.StartRecording();
            }
            catch (Exception)
            {
                AdeSingleDialog.Instance.Show(I.S["obsrecordstartfailed"], I.S["error"]);
                yield break;
            }
            yield return new WaitForSeconds(0.5f);
            ArcadeComposeManager.Instance.Play();
            ArcGameplayManager.Instance.PlayDelayed();
            yield return new WaitForSeconds(ArcAudioManager.Instance.Clip.length + 3);
            yield return new WaitForSeconds(0.5f);
            if (obs.IsConnected) obs.StopRecording();
            recording = null;
        }
    }
}