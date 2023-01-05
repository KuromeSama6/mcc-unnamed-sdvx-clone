using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Arcade.Compose
{
    public class AdeShutterManager : MonoBehaviour
    {
        public const float Duration = 0.65f;
        public static AdeShutterManager Instance { get; private set; }
        private void Awake()
        { 
            Instance = this; 
        }

        public RectTransform Left, Right;
        public AudioClip CloseAudio, OpenAudio; 

        public void Open()
        {
            Left.DOAnchorPosX(-1165, Duration).SetEase(Ease.InCubic);
            Right.DOAnchorPosX(459, Duration).SetEase(Ease.InCubic);
            AudioSource.PlayClipAtPoint(OpenAudio, new Vector3());
        }
        public void Close()
        {
            Left.DOAnchorPosX(0, Duration).SetEase(Ease.OutCubic);
            Right.DOAnchorPosX(0, Duration).SetEase(Ease.OutCubic);
            AudioSource.PlayClipAtPoint(CloseAudio, new Vector3());
        }
        public IEnumerator OpenCoroutine()
        {
            Open();
            yield return new WaitForSeconds(Duration);
        }
        public IEnumerator CloseCoroutine()
        {
            Close();
            yield return new WaitForSeconds(Duration);
        }
    }
}