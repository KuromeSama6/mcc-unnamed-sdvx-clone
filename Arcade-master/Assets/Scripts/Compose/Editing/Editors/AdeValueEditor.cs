using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Arcade.Gameplay.Chart;
using Arcade.Compose.Command;

namespace Arcade.Compose.Editing
{
    public class AdeValueEditor : MonoBehaviour, INoteSelectEvent
    {
        public static AdeValueEditor Instance { get; private set; }

        public RectTransform Panel;
        public RectTransform Timing, Track, EndTiming, StartPos, EndPos, LineType, Color, IsVoid, TimingGroup, SelectArc;

        private void OnPlay()
        {

        }
        private void OnPause()
        {

        }
        public void OnNoteSelect(ArcNote note)
        {
            MakeupFields();
        }
        public void OnNoteDeselect(ArcNote note)
        {
            MakeupFields();
        }
        public void OnNoteDeselectAll()
        {
            MakeupFields();
        }

        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            AdeCursorManager.Instance.NoteEventListeners.Add(this);
            ArcadeComposeManager.Instance.OnPlay.AddListener(OnPlay);
            ArcadeComposeManager.Instance.OnPause.AddListener(OnPause);
        }
        private void OnDestroy()
        {
            AdeCursorManager.Instance.NoteEventListeners.Remove(this);
            ArcadeComposeManager.Instance.OnPlay.RemoveListener(OnPlay);
            ArcadeComposeManager.Instance.OnPause.RemoveListener(OnPause);
        } 

        private bool canEdit = false;
        public void MakeupFields()
        {
            canEdit = false;
            List<ArcNote> selected = AdeCursorManager.Instance.SelectedNotes;
            int count = selected.Count;
            bool multiple = count != 1;
            if (count == 0)
            {
                Panel.gameObject.SetActive(false);
                return;
            }
            else
            {
                Timing.gameObject.SetActive(true);
                Track.gameObject.SetActive(true);
                EndTiming.gameObject.SetActive(true);
                StartPos.gameObject.SetActive(true);
                EndPos.gameObject.SetActive(true);
                LineType.gameObject.SetActive(true);
                Color.gameObject.SetActive(true);
                IsVoid.gameObject.SetActive(true);
                TimingGroup.gameObject.SetActive(true);
                SelectArc.gameObject.SetActive(true);
                foreach (var s in selected)
                {
                    if (Track.gameObject.activeSelf) Track.gameObject.SetActive(s is ArcTap || s is ArcHold);
                    if (EndTiming.gameObject.activeSelf) EndTiming.gameObject.SetActive(s is ArcLongNote);
                    if (StartPos.gameObject.activeSelf) StartPos.gameObject.SetActive(s is ArcArc);
                    if (EndPos.gameObject.activeSelf) EndPos.gameObject.SetActive(s is ArcArc);
                    if (LineType.gameObject.activeSelf) LineType.gameObject.SetActive(s is ArcArc);
                    if (Color.gameObject.activeSelf) Color.gameObject.SetActive(s is ArcArc);
                    if (IsVoid.gameObject.activeSelf) IsVoid.gameObject.SetActive(s is ArcArc);
                    if (TimingGroup.gameObject.activeSelf) TimingGroup.gameObject.SetActive(s is ArcTap || s is ArcHold || s is ArcArc);
                    if (SelectArc.gameObject.activeSelf) SelectArc.gameObject.SetActive(s is ArcArcTap && !multiple);
                }
                float p = -20;
                ArcNote note = selected[0];
                Timing.anchoredPosition = new Vector2(0, p);
                Timing.GetComponentInChildren<InputField>().text = multiple ? "-" : note.Timing.ToString();
                p -= 40;
                if (Track.gameObject.activeSelf)
                {
                    Track.anchoredPosition = new Vector2(0, p);
                    Track.GetComponentInChildren<InputField>().text = multiple ? "-" : (note is ArcTap ? (note as ArcTap).Track.ToString() : (note as ArcHold).Track.ToString());
                    p -= 40;
                }
                if (EndTiming.gameObject.activeSelf)
                {
                    EndTiming.anchoredPosition = new Vector2(0, p);
                    EndTiming.GetComponentInChildren<InputField>().text = multiple ? "-" : (note as ArcLongNote).EndTiming.ToString();
                    p -= 40;
                }
                if (StartPos.gameObject.activeSelf)
                {
                    StartPos.anchoredPosition = new Vector2(0, p);
                    StartPos.GetComponentInChildren<InputField>().text = multiple ? "-,-" : $"{(note as ArcArc).XStart:f3},{(note as ArcArc).YStart:f3}";
                    p -= 40;
                }
                if (EndPos.gameObject.activeSelf)
                {
                    EndPos.anchoredPosition = new Vector2(0, p);
                    EndPos.GetComponentInChildren<InputField>().text = multiple ? "-,-" : $"{(note as ArcArc).XEnd:f3},{(note as ArcArc).YEnd:f3}";
                    p -= 40;
                }
                if (LineType.gameObject.activeSelf)
                {
                    LineType.anchoredPosition = new Vector2(0, p);
                    LineType.GetComponentInChildren<Dropdown>().value = multiple ? 0 : (int)(note as ArcArc).LineType;
                    p -= 40;
                }
                if (Color.gameObject.activeSelf)
                {
                    Color.anchoredPosition = new Vector2(0, p);
                    Color.GetComponentInChildren<Dropdown>().value = multiple ? 0 : (note as ArcArc).Color;
                    p -= 40;
                }
                if (IsVoid.gameObject.activeSelf)
                {
                    IsVoid.anchoredPosition = new Vector2(0, p);
                    IsVoid.GetComponentInChildren<Toggle>().isOn = !multiple && (note as ArcArc).IsVoid;
                    p -= 40;
                }
                if (TimingGroup.gameObject.activeSelf)
                {
                    TimingGroup.anchoredPosition = new Vector2(0, p);
                    int timinggroup = 0;
                    switch (note)
                    {
                        case ArcTap tap: timinggroup = tap.TimingGroup; break;
                        case ArcHold hold: timinggroup = hold.TimingGroup; break;
                        case ArcArc arc: timinggroup = arc.TimingGroup; break;
                    }
                    TimingGroup.GetComponentInChildren<InputField>().text = multiple ? "-" : timinggroup.ToString();
                    p -= 40;
                }
                if (SelectArc.gameObject.activeSelf)
                { 
                    SelectArc.anchoredPosition = new Vector2(0, p);
                    p -= 40;
                }
                Panel.sizeDelta = new Vector2(300, -p + 10);
                Panel.gameObject.SetActive(true);
            }
            canEdit = true;
        }

        public void OnTiming(InputField inputField)
        {
            if (!canEdit) return;
            try
            {
                string t = inputField.text;
                int timing = int.Parse(t);
                foreach (var n in AdeCursorManager.Instance.SelectedNotes)
                {
                    var ne = n.Clone();
                    ne.Timing = timing;
                    CommandManager.Instance.Add(new EditArcEventCommand(n, ne));
                }
            }
            catch (Exception Ex)
            {
                AdeToast.Instance.Show(I.S["assignvalerr"]);
                Debug.LogException(Ex);
            }
        }
        public void OnTrack(InputField inputField)
        {
            if (!canEdit) return;
            try
            {
                string t = inputField.text;
                int track = int.Parse(t);
                if (track <= 0 || track >= 5) throw new InvalidDataException(I.S["trackoor"]);
                foreach (var n in AdeCursorManager.Instance.SelectedNotes)
                {
                    if (n is ArcTap)
                    {
                        ArcTap ne = n.Clone() as ArcTap;
                        ne.Track = track;
                        CommandManager.Instance.Add(new EditArcEventCommand(n, ne));
                    }
                    else if (n is ArcHold)
                    {
                        ArcHold ne = n.Clone() as ArcHold;
                        ne.Track = track;
                        CommandManager.Instance.Add(new EditArcEventCommand(n, ne));
                    }
                }
            }
            catch (Exception Ex)
            {
                AdeToast.Instance.Show(I.S["assignvalerr"]);
                Debug.LogException(Ex);
            }
        }
        public void OnEndTiming(InputField inputField)
        {
            if (!canEdit) return;
            try
            {
                string t = inputField.text;
                int endTiming = int.Parse(t);
                foreach (var n in AdeCursorManager.Instance.SelectedNotes)
                {
                    var ne = n.Clone() as ArcLongNote;
                    ne.EndTiming = endTiming;
                    CommandManager.Instance.Add(new EditArcEventCommand(n, ne));
                }
            }
            catch (Exception Ex)
            {
                AdeToast.Instance.Show(I.S["assignvalerr"]);
                Debug.LogException(Ex);
            }
        }
        public void OnStartPos(InputField inputField)
        {
            if (!canEdit) return;
            try
            {
                string t = inputField.text;
                string[] ts = t.Split(',');
                float x = float.Parse(ts[0]);
                float y = float.Parse(ts[1]);
                foreach (var n in AdeCursorManager.Instance.SelectedNotes)
                {
                    var ne = n.Clone() as ArcArc;
                    ne.XStart = x;
                    ne.YStart = y;
                    CommandManager.Instance.Add(new EditArcEventCommand(n, ne));
                }
            }
            catch (Exception Ex)
            {
                AdeToast.Instance.Show(I.S["assignvalerr"]);
                Debug.LogException(Ex);
            }
        }
        public void OnEndPos(InputField inputField)
        {
            if (!canEdit) return;
            try
            {
                string t = inputField.text;
                string[] ts = t.Split(',');
                float x = float.Parse(ts[0]);
                float y = float.Parse(ts[1]);
                foreach (var n in AdeCursorManager.Instance.SelectedNotes)
                {
                    var ne = n.Clone() as ArcArc;
                    ne.XEnd = x;
                    ne.YEnd = y;
                    CommandManager.Instance.Add(new EditArcEventCommand(n, ne));
                }
            }
            catch (Exception Ex)
            {
                AdeToast.Instance.Show(I.S["assignvalerr"]);
                Debug.LogException(Ex);
            }
        }
        public void OnLineType(Dropdown dropdown)
        {
            if (!canEdit) return;
            try
            {
                foreach (var n in AdeCursorManager.Instance.SelectedNotes)
                {
                    var ne = n.Clone() as ArcArc;
                    ne.LineType = (ArcLineType)dropdown.value;
                    CommandManager.Instance.Add(new EditArcEventCommand(n, ne));
                }
            }
            catch (Exception Ex)
            {
                AdeToast.Instance.Show(I.S["assignvalerr"]);
                Debug.LogException(Ex);
            }
        }
        public void OnColor(Dropdown dropdown)
        {
            if (!canEdit) return;
            try
            {
                foreach (var n in AdeCursorManager.Instance.SelectedNotes)
                {
                    var ne = n.Clone() as ArcArc;
                    ne.Color = dropdown.value;
                    CommandManager.Instance.Add(new EditArcEventCommand(n, ne));
                }
            }
            catch (Exception Ex)
            {
                AdeToast.Instance.Show(I.S["assignvalerr"]);
                Debug.LogException(Ex);
            }
        }
        public void OnIsVoid(Toggle toggle)
        {
            if (!canEdit) return;
            try
            {
                foreach (var n in AdeCursorManager.Instance.SelectedNotes)
                {
                    var ne = n.Clone() as ArcArc;
                    ne.IsVoid = toggle.isOn;
                    CommandManager.Instance.Add(new EditArcEventCommand(n, ne));
                }
            }
            catch (Exception Ex)
            {
                AdeToast.Instance.Show(I.S["assignvalerr"]);
                Debug.LogException(Ex);
            }
        }
        public void OnTimingGroup(InputField inputField)
        {
            if (!canEdit) return;
            try
            {
                string t = inputField.text;
                int tg = int.Parse(t);
                foreach (var n in AdeCursorManager.Instance.SelectedNotes)
                {
                    var ne = n.Clone();
                    switch (ne)
                    {
                        case ArcTap tap: tap.TimingGroup = tg; break;
                        case ArcHold hold: hold.TimingGroup = tg; break;
                        case ArcArc arc: arc.TimingGroup = tg; break;
                    }
                    CommandManager.Instance.Add(new EditArcEventCommand(n, ne));
                }
            }
            catch (Exception Ex)
            {
                AdeToast.Instance.Show(I.S["assignvalerr"]);
                Debug.LogException(Ex);
            }
        }
        public void OnSelectArc()
        {
            if(AdeCursorManager.Instance.SelectedNotes.Count == 1)
            {
                if(AdeCursorManager.Instance.SelectedNotes[0] is ArcArcTap)
                {
                    ArcArc arc = (AdeCursorManager.Instance.SelectedNotes[0] as ArcArcTap).Arc;
                    if (!Input.GetKey(KeyCode.LeftControl))
                        AdeCursorManager.Instance.DeselectAllNotes();
                    AdeCursorManager.Instance.SelectNote(arc);
                }
            }
        }
    }
}