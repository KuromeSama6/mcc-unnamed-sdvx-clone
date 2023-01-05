using Arcade.Compose.Editing;
using Arcade.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcade.Compose
{
    public class AdeRangeSelect : MonoBehaviour
    {
        private Coroutine co = null;

        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    RangeSelect();
                }
            }
        }

        public void RangeSelect()
        {
            if (!ArcGameplayManager.Instance.IsLoaded)
            {
                AdeToast.Instance.Show(I.S["loadchartfirst"]);
                return;
            }
            if (co != null)
            {
                StopCoroutine(co);
                AdeCursorManager.Instance.Mode = CursorMode.Idle;
            }
            co = StartCoroutine(RangeSelectCoroutine()); 
        }

        private IEnumerator RangeSelectCoroutine()
        {
            AdeCursorManager.Instance.Mode = CursorMode.Horizontal;
            AdeCursorManager.Instance.EnableVerticalPanel = true;
            AdeToast.Instance.Show(I.S["rangeselect1"]);
            while (true)
            {
                yield return null;
                if (Input.GetMouseButtonDown(0) && AdeCursorManager.Instance.IsHorizontalHit)
                    break;
            }
            int start = (int)AdeCursorManager.Instance.AttachedTiming;
            AdeToast.Instance.Show(I.S["rangeselect2"]);
            while (true)
            {
                yield return null;
                if (Input.GetMouseButtonDown(0) && AdeCursorManager.Instance.IsHorizontalHit)
                    break;
            }
            int end = (int)AdeCursorManager.Instance.AttachedTiming;
            AdeCursorManager.Instance.Mode = CursorMode.Idle;
            AdeCursorManager.Instance.EnableVerticalPanel = false;
            AdeCursorManager.Instance.SelectNotesInRange(start, end);
            AdeToast.Instance.Show(string.Format(I.S["rangeselect"], start, end));
            co = null;
        }
    }
}