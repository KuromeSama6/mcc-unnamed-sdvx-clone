﻿using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Arcade.Gameplay
{
    public class ArcTapNoteEffectComponent : MonoBehaviour
    {
        public bool Available { get; set; } = true;
        public ParticleSystem Effect;

        public void PlayAt(Vector2 pos)
        {
            Available = false;
            transform.position = pos;
            Effect.Play();
            StartCoroutine(WaitForEnd());
        }
        IEnumerator WaitForEnd()
        {
            yield return new WaitForSeconds(0.5f);
            Effect.Stop();
            Effect.Clear();
            yield return null;
            Available = true;
        }
    }
}