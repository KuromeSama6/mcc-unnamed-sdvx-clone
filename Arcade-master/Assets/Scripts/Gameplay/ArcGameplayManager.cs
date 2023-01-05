using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Arcade.Gameplay.Chart;
using Arcade.Gameplay;
using UnityEngine.Events;
using Arcade.Gameplay.Events;
using UnityEngine.EventSystems;
using Arcade.Aff;
using System.Linq;
using Arcade.Aff.Advanced;

namespace Arcade.Gameplay
{
    namespace Events
    {
        public class OnMusicFinishedEvent : UnityEvent
        {

        }
    }

    public class ArcGameplayManager : MonoBehaviour
    {
        public static ArcGameplayManager Instance { get; private set; }

        public ShaderVariantCollection Shaders;
        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            AudioSync = PlayerPrefs.GetInt("AudioSync", 0) == 1;
            AudioSyncButton.image.color = AudioSync ? new Color(0.59f, 0.55f, 0.65f, 1f) : new Color(0.9f, 0.9f, 0.9f, 1);
            Shaders.WarmUp();
        }

        public bool Auto { get; set; }
        public bool IsPlaying { get; set; }
        private float timing;
        public int Timing
        {
            get
            {
                return (int)(timing * 1000);
            }
            set
            {
                timing = value / 1000f;
                ArcAudioManager.Instance.Timing = timing;
            }
        }
        public float Timingf
        {
            get
            {
                return timing;
            }
            set
            {
                timing = value;
                ArcAudioManager.Instance.Timing = timing;
            }
        }
        public int Length { get; private set; }
        public bool IsLoaded { get; set; }

        public UnityEvent OnChartLoad = new UnityEvent();
        public OnMusicFinishedEvent OnMusicFinished = new OnMusicFinishedEvent();

        private DateTime lastDt = DateTime.MinValue;
        
        public bool AudioSync = false;
        public Button AudioSyncButton;
        public void OnAudioSyncClicked()
        {
            AudioSync = !AudioSync;
            AudioSyncButton.image.color = AudioSync ? new Color(0.59f, 0.55f, 0.65f, 1f) : new Color(0.9f, 0.9f, 0.9f, 1);
            PlayerPrefs.SetInt("AudioSync", AudioSync ? 1 : 0);
        }

        private void Update()
        {
            DateTime currentDt = DateTime.Now;
            if (lastDt != DateTime.MinValue)
            {
                TimeSpan deltaDt = currentDt - lastDt;
                if (IsPlaying)
                {
                    timing += (float)deltaDt.TotalSeconds;
                    if (AudioSync)
                    {
                        float t = ArcAudioManager.Instance.Timing;
                        float delta = timing - t;
                        if (Mathf.Abs(delta) > 0.032f) timing = t;
                    }
                }
                if (Timing > Length)
                {
                    OnMusicFinished.Invoke();
                    Stop();
                }
            }
            lastDt = currentDt;
        }
        public bool Load(ArcChart chart, AudioClip audio)
        {
            if (audio == null || chart == null) return false;

            Clean();
            IsLoaded = true;
            Length = (int)(audio.length * 1000);

            ArcCameraManager.Instance.ResetCamera();
            ArcAudioManager.Instance.Load(audio, chart.AudioOffset);
            ArcTimingManager.Instance.Load(chart.Timings);
            ArcTapNoteManager.Instance.Load(chart.Taps);
            ArcHoldNoteManager.Instance.Load(chart.Holds);
            ArcArcManager.Instance.Load(chart.Arcs);
            ArcCameraManager.Instance.Load(chart.Cameras);
            ArcSpecialManager.Instance.Load(chart.Specials);

            OnChartLoad.Invoke();
            return true;
        }
        public void Clean()
        {
            Timing = 0;
            ArcTimingManager.Instance.Clean();
            ArcTapNoteManager.Instance.Clean();
            ArcHoldNoteManager.Instance.Clean();
            ArcArcManager.Instance.Clean();
            ArcCameraManager.Instance.Clean();
            ArcSpecialManager.Instance.Clean();
            IsLoaded = false;
            Length = 0;
        }

        public void ResetJudge()
        {
            if (IsLoaded)
            {
                foreach (var t in ArcArcManager.Instance.Arcs) { foreach (var a in t.ArcTaps) { a.Judged = false; a.Judging = false; } t.Judged = false; t.Judging = false; t.AudioPlayed = false; };
                foreach (var t in ArcHoldNoteManager.Instance.Holds) { t.Judged = false; t.Judging = false; t.AudioPlayed = false; };
                foreach (var t in ArcTapNoteManager.Instance.Taps) { t.Judged = false; t.Judging = false; };
                ArcEffectManager.Instance.ResetJudge();
                ArcSpecialManager.Instance.ResetJudge();
            }
        }
        public void PlayDelayed()
        {
            timing = -3f;
            ResetJudge();
            ArcAudioManager.Instance.Source.Stop();
            ArcAudioManager.Instance.Source.time = 0;
            ArcAudioManager.Instance.Source.PlayDelayed(3);
            IsPlaying = true;
        }

        public void Play()
        {
            ArcAudioManager.Instance.Play();
            ArcAudioManager.Instance.Timing = timing;
            IsPlaying = true;
        }
        public void Pause()
        {
            ArcAudioManager.Instance.Pause();
            IsPlaying = false;
            ResetJudge();
        }
        public void Stop()
        {
            timing = 0;
            ArcAudioManager.Instance.Pause();
            ArcAudioManager.Instance.Timing = 0;
            IsPlaying = false;
        }

        public ArcNote FindNoteByInstance(GameObject gameObject)
        {
            foreach (var tap in ArcTapNoteManager.Instance.Taps) if (tap.Instance.Equals(gameObject)) return tap;
            foreach (var hold in ArcHoldNoteManager.Instance.Holds) if (hold.Instance.Equals(gameObject)) return hold;
            foreach (var arc in ArcArcManager.Instance.Arcs)
            {
                if (arc.IsMyself(gameObject)) return arc;
                foreach (var arctap in arc.ArcTaps)
                {
                    if (arctap.IsMyself(gameObject)) return arctap;
                }
            }
            return null;
        }

        public void SerializeChart(Stream stream, ChartSortMode mode = ChartSortMode.Timing)
        {
            using (ArcaeaAffWriter writer = new ArcaeaAffWriter(stream, ArcAudioManager.Instance.AudioOffset))
            {
                foreach (var tg in ArcTimingManager.Instance.TimingGroups)
                {
                    if (tg.TimingGroup != 0)
                        writer.WriteTimingGroupStart();

                    ArcTiming[] timingBaseResult = tg.Timings.Where((t) => t.Timing == 0).ToArray();
                    if (timingBaseResult.Length != 1)
                        throw new ArcaeaAffFormatException(I.S["timingzerooor"]);
                    ArcTiming timingBase = timingBaseResult[0];

                    writer.WriteEvent(new ArcaeaAffTiming() { Timing = timingBase.Timing, Bpm = timingBase.Bpm, BeatsPerLine = timingBase.BeatsPerLine, Type = Aff.EventType.Timing });
                    List<ArcEvent> events = new List<ArcEvent>();
                    events.AddRange(ArcTapNoteManager.Instance.Taps.Where((t) => t.TimingGroup == tg.TimingGroup));
                    events.AddRange(ArcHoldNoteManager.Instance.Holds.Where((h) => h.TimingGroup == tg.TimingGroup));
                    events.AddRange(tg.Timings);
                    events.Remove(timingBase);
                    events.AddRange(ArcArcManager.Instance.Arcs.Where((a) => a.TimingGroup == tg.TimingGroup));
                    if (tg.TimingGroup == 0)
                    {
                        events.AddRange(ArcCameraManager.Instance.Cameras);
                        events.AddRange(ArcSpecialManager.Instance.Specials);
                    }
                    switch (mode)
                    {
                        case ChartSortMode.Timing: events.Sort((ArcEvent a, ArcEvent b) => a.Timing.CompareTo(b.Timing)); break;
                        case ChartSortMode.Type:
                            events.Sort((ArcEvent a, ArcEvent b) =>
                            {
                                int atype = (a is ArcTiming ? 1 : a is ArcTap ? 2 : a is ArcHold ? 3 : a is ArcArc ? 4 : 5);
                                int btype = (b is ArcTiming ? 1 : b is ArcTap ? 2 : b is ArcHold ? 3 : b is ArcArc ? 4 : 5);
                                int c1 = atype.CompareTo(btype);
                                return c1 == 0 ? a.Timing.CompareTo(b.Timing) : c1;
                            });
                            break;
                    }
                    foreach (var e in events)
                    {
                        if (e is ArcTap)
                        {
                            var tap = e as ArcTap;
                            writer.WriteEvent(new ArcaeaAffTap() { Timing = tap.Timing, Track = tap.Track, Type = Aff.EventType.Tap });
                        }
                        else if (e is ArcHold)
                        {
                            var hold = e as ArcHold;
                            writer.WriteEvent(new ArcaeaAffHold() { Timing = hold.Timing, Track = hold.Track, EndTiming = hold.EndTiming, Type = Aff.EventType.Hold });
                        }
                        else if (e is ArcTiming)
                        {
                            var timing = e as ArcTiming;
                            writer.WriteEvent(new ArcaeaAffTiming() { Timing = timing.Timing, BeatsPerLine = timing.BeatsPerLine, Bpm = timing.Bpm, Type = Aff.EventType.Timing });
                        }
                        else if (e is ArcArc)
                        {
                            var arc = e as ArcArc;
                            var a = new ArcaeaAffArc()
                            {
                                Timing = arc.Timing,
                                EndTiming = arc.EndTiming,
                                XStart = arc.XStart,
                                XEnd = arc.XEnd,
                                LineType = ArcChart.ToLineTypeString(arc.LineType),
                                YStart = arc.YStart,
                                YEnd = arc.YEnd,
                                Color = arc.Color,
                                IsVoid = arc.IsVoid,
                                Type = Aff.EventType.Arc
                            };
                            if (arc.ArcTaps != null && arc.ArcTaps.Count != 0)
                            {
                                a.ArcTaps = new List<int>();
                                foreach (var arctap in arc.ArcTaps)
                                {
                                    a.ArcTaps.Add(arctap.Timing);
                                }
                            }
                            writer.WriteEvent(a);
                        }
                        else if (e is ArcCamera)
                        {
                            var cam = e as ArcCamera;
                            writer.WriteEvent(new ArcaeaAffCamera()
                            {
                                Timing = cam.Timing,
                                Move = cam.Move,
                                Rotate = cam.Rotate,
                                CameraType = ArcChart.ToCameraTypeString(cam.CameraType),
                                Duration = cam.Duration,
                                Type = Aff.EventType.Camera
                            });
                        }
                        else if (e is ArcSpecial)
                        {
                            var spe = e as ArcSpecial;
                            writer.WriteEvent(new ArcadeAffSpecial
                            {
                                Timing = spe.Timing,
                                Type = Aff.EventType.Special,
                                SpecialType = spe.Type,
                                param1 = spe.Param1,
                                param2 = spe.Param2,
                                param3 = spe.Param3
                            });
                        }
                    }
                    if (tg.TimingGroup != 0)
                        writer.WriteTimingGroupEnd();
                }
                writer.Flush();
                writer.Close();
            }
        }
    }
}
