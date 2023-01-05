using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Arcade.Gameplay
{
    public class ArcEffectManager : MonoBehaviour
    {
        public static ArcEffectManager Instance { get; private set; }
        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            for (int i = 0; i < 10; ++i)
            {
                ArcTapNoteEffectComponent n = Instantiate(TapNoteJudgeEffect, EffectLayer).GetComponent<ArcTapNoteEffectComponent>();
                tapNoteEffectInstances.Add(n);
            }
        }

        public GameObject TapNoteJudgeEffect;
        public GameObject[] LaneHits = new GameObject[4];
        public ParticleSystem[] HoldNoteEffects = new ParticleSystem[4];
        public Transform EffectLayer;
        public AudioClip TapAudio, ArcAudio;
        public AudioSource Source;

        private bool[] holdEffectStatus = new bool[4];
        private List<ArcTapNoteEffectComponent> tapNoteEffectInstances = new List<ArcTapNoteEffectComponent>();

        public ArcTapNoteEffectComponent GetTapNoteEffectInstance()
        {
            foreach (var i in tapNoteEffectInstances) if (i.Available) return i;
            ArcTapNoteEffectComponent n = Instantiate(TapNoteJudgeEffect, EffectLayer).GetComponent<ArcTapNoteEffectComponent>();
            tapNoteEffectInstances.Add(n);
            return n;
        }
        public void SetHoldNoteEffect(int track, bool show)
        {
            if (holdEffectStatus[track - 1] != show)
            {
                holdEffectStatus[track - 1] = show;
                if (show)
                {
                    HoldNoteEffects[track - 1].Play();
                }
                else
                {
                    HoldNoteEffects[track - 1].Stop();
                    HoldNoteEffects[track - 1].Clear();
                }
                LaneHits[track - 1].SetActive(show);
            }
        }
        public void PlayTapNoteEffectAt(Vector2 pos, bool isArc = false)
        {
            ArcTapNoteEffectComponent a = GetTapNoteEffectInstance();
            a.PlayAt(pos);
            Source.PlayOneShot(isArc ? ArcAudio : TapAudio);
        }
        public void PlayTapSound()
        {
            Source.PlayOneShot(TapAudio);
        }
        public void PlayArcSound()
        {
            Source.PlayOneShot(ArcAudio);
        }
        public void ResetJudge()
        {
            for (int i = 1; i < 5; ++i) SetHoldNoteEffect(i, false);
        }
    }
}