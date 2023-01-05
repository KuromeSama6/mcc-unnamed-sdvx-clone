﻿using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Arcade.Gameplay.Chart;

namespace Arcade.Gameplay
{
    public class ArcTapNoteManager : MonoBehaviour
    {
        public static ArcTapNoteManager Instance { get; private set; }
        private void Awake()
        {
            Instance = this;
        }

        [HideInInspector]
        public List<ArcTap> Taps = new List<ArcTap>();
        [HideInInspector]
        public readonly float[] Lanes = { 6.375f, 2.125f, -2.125f, -6.375f };
        public GameObject TapNotePrefab;
        public Transform NoteLayer;
        public Material ShaderdMaterial;

        public void Clean()
        {
            foreach (var t in Taps) t.Destroy();
            Taps.Clear();
        }
        public void Load(List<ArcTap> taps)
        {
            Taps = taps;
            foreach (var t in Taps)
            {
                t.Instantiate();
            }
        }

        public void Add(ArcTap tap)
        {
            tap.Instantiate();
            Taps.Add(tap);
            tap.SetupArcTapConnection();
        }
        public void Remove(ArcTap tap)
        {
            tap.Destroy();
            Taps.Remove(tap);
        }

        private void Update()
        {
            if (Taps == null) return;
            if (ArcGameplayManager.Instance.Auto) JudgeTapNotes();
            RenderTapNotes();
        }

        private void RenderTapNotes()
        {
            int offset = ArcAudioManager.Instance.AudioOffset;

            foreach (var t in Taps)
            {
                ArcTimingGroup timing = ArcTimingManager.Instance[t.TimingGroup];
                if (!timing.ShouldRender(t.Timing + offset) || t.Judged)
                {
                    t.Enable = false;
                    continue;
                }
                t.Position = timing.CalculatePositionByTiming(t.Timing + offset);
                if (t.Position > 100000 || t.Position < -10000)
                {
                    t.Enable = false;
                    continue;
                }
                t.Enable = true;
                float pos = t.Position / 1000f;
                t.transform.localPosition = new Vector3(Lanes[t.Track - 1], pos, 0);
                if (ArcCameraManager.Instance.EditorCamera)
                    t.transform.localScale = new Vector3(1.53f, 2, 1);
                else
                    t.transform.localScale = new Vector3(1.53f, 2f + 5.1f * pos / 100f, 1);
                t.Alpha = pos < 90 ? 1 : (100 - pos) / 10f;
                t.OptimizeMaterial();
            }
        }
        private void JudgeTapNotes()
        { 
            int offset = ArcAudioManager.Instance.AudioOffset;
            int currentTiming = ArcGameplayManager.Instance.Timing;
            foreach (var t in Taps)
            {
                if (t.Judged) continue;
                if (currentTiming > t.Timing + offset && currentTiming <= t.Timing + offset + 150)
                {
                    t.Judged = true;
                    if (ArcGameplayManager.Instance.IsPlaying) ArcEffectManager.Instance.PlayTapNoteEffectAt(new Vector2(Lanes[t.Track - 1], 0));
                }
                else if (currentTiming > t.Timing + offset + 150)
                {
                    t.Judged = true;
                }
            }
        }
    }
}