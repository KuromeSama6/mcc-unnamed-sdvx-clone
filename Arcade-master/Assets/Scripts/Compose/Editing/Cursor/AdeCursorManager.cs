﻿using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Arcade.Gameplay;
using Schwarzer.UnityExtension;
using Arcade.Aff;
using Arcade.Gameplay.Chart;
using UnityEngine.Events;
using Arcade.Compose.Command;
using Arcade.Compose.Editing;
using Arcade.Compose.MarkingMenu;
using System.Linq;

namespace Arcade.Compose
{
    public enum CursorMode
    {
        Idle,
        Horizontal,
        Vertical
    }
    public class OnNoteEvent : UnityEvent<ArcNote>
    {

    }
    public interface INoteSelectEvent
    {
        void OnNoteSelect(ArcNote note);
        void OnNoteDeselect(ArcNote note);
        void OnNoteDeselectAll();
    }

    public class AdeCursorManager : MonoBehaviour, IMarkingMenuItemProvider
    {
        public static AdeCursorManager Instance { get; private set; }

        public Camera GameplayCamera;
        public MeshCollider HorizontalCollider, VerticalCollider;
        public LineRenderer HorizontalX, HorizontalY, VerticalX, VerticalY;
        public MeshRenderer VerticalRenderer;

        public Transform ArcTapCursor;
        public MeshRenderer ArcTapCursorRenderer;

        public Text InfoText;
        public GameObject InfoGameObject;
        public RectTransform InfoRect;

        public MarkingMenuItem DeleteItem;

        private CursorMode mode;
        private bool enableHorizontal, enableVertical, enableVerticalPanel;
        private bool enableArcTapCursor, enableInfo;
        private RaycastHit horizontalHit, verticalHit;

        public bool EnableHorizontal
        {
            get
            {
                return enableHorizontal;
            }
            set
            {
                if (enableHorizontal != value)
                {
                    HorizontalX.enabled = value;
                    HorizontalY.enabled = value;
                    HorizontalX.positionCount = 0;
                    HorizontalY.positionCount = 0;
                    enableHorizontal = value;
                }
            }
        }
        public bool EnableVertical
        {
            get
            {
                return enableVertical;
            }
            set
            {
                if (enableVertical != value)
                {
                    VerticalX.enabled = value;
                    VerticalY.enabled = value;
                    VerticalX.positionCount = 0;
                    VerticalY.positionCount = 0;
                    EnableVerticalPanel = value;
                    enableVertical = value;
                }
            }
        }
        public bool EnableVerticalPanel
        {
            get
            {
                return enableVerticalPanel;
            }
            set
            {
                if (enableVerticalPanel != value)
                {
                    VerticalRenderer.enabled = value;
                    enableVerticalPanel = value;
                }
            }
        }
        public bool EnableArcTapCursor
        {
            get
            {
                return enableArcTapCursor;
            }
            set
            {
                if (enableArcTapCursor != value)
                {
                    ArcTapCursorRenderer.enabled = value;
                    enableArcTapCursor = value;
                }
            }
        }
        public bool EnableInfo
        {
            get
            {
                return enableInfo;
            }
            set
            {
                if (enableInfo != value)
                {
                    InfoGameObject.SetActive(value);
                    enableInfo = value;
                }
            }
        }
        public Vector2 ArcTapCursorPosition
        {
            get
            {
                return ArcTapCursor.localPosition;
            }
            set
            {
                ArcTapCursor.localPosition = value;
            }
        }
        public CursorMode Mode
        {
            get
            {
                return mode;
            }
            set
            {
                mode = value;
                if (mode != CursorMode.Horizontal) EnableHorizontal = false;
                if (mode != CursorMode.Vertical) EnableVertical = false;
            }
        }

        public bool IsHorizontalHit { get; set; }
        public bool IsVerticalHit { get; set; }
        public Vector3 HorizontalPoint
        {
            get
            {
                return horizontalHit.point;
            }
        }
        public Vector3 VerticalPoint
        {
            get
            {
                return verticalHit.point;
            }
        }
        public Vector3 AttachedHorizontalPoint
        {
            get
            {
                float z = AdeGridManager.Instance.AttachBeatline(horizontalHit.point.z);
                return new Vector3(horizontalHit.point.x, horizontalHit.point.y, z);
            }
        }
        public Vector3 AttachedVerticalPoint
        {
            get
            {
                return new Vector3(ArcAlgorithm.ArcXToWorld(AdeGridManager.Instance.AttachVerticalX(ArcAlgorithm.WorldXToArc(VerticalPoint.x))),
                   ArcAlgorithm.ArcYToWorld(AdeGridManager.Instance.AttachVerticalY(ArcAlgorithm.WorldYToArc(VerticalPoint.y))));
            }
        }

        public float AttachedTiming
        {
            get
            {
                if (!ArcGameplayManager.Instance.IsLoaded) return 0;
                Vector3 pos = AttachedHorizontalPoint;
                return ArcTimingManager.Instance[AdeTimingEditor.Instance.EditingTimingGroup].CalculateTimingByPosition(-pos.z * 1000) - ArcAudioManager.Instance.AudioOffset;
            }
        }

        public bool IsOnly
        {
            get
            {
                return false;
            }
        }
        public MarkingMenuItem[] Items
        {
            get
            {
                return SelectedNotes.Count == 0 ? new MarkingMenuItem[] { } : new MarkingMenuItem[] { DeleteItem };
            }
        }

        public List<ArcNote> SelectedNotes = new List<ArcNote>();

        public List<INoteSelectEvent> NoteEventListeners = new List<INoteSelectEvent>();

        private void Start()
        {
            AdeMarkingMenuManager.Instance.Providers.Add(this);
        }
        private void OnDestroy()
        {
            AdeMarkingMenuManager.Instance.Providers.Remove(this);
        }

        private void Awake()
        {
            Instance = this;
        }
        private void Update()
        {
            UpdateHorizontal();
            UpdateVertical();
            RaycastSelecting();
            UpdateInfo();
            DeleteListener();
        }

        private void UpdateHorizontal()
        {
            Ray ray = GameplayCamera.ScreenPointToRay();
            IsHorizontalHit = HorizontalCollider.Raycast(ray, out horizontalHit, 120);
            if (Mode != CursorMode.Horizontal) return;
            EnableHorizontal = IsHorizontalHit;
            if (IsHorizontalHit)
            {
                float z = AdeGridManager.Instance.AttachBeatline(horizontalHit.point.z);
                HorizontalX.DrawLine(new Vector3(-8.5f, z), new Vector3(8.5f, z));
                HorizontalY.DrawLine(new Vector3(horizontalHit.point.x, 0), new Vector3(horizontalHit.point.x, -100));
                VerticalCollider.transform.localPosition = new Vector3(0, 0, z);
            }
        }
        private void UpdateVertical()
        {
            Ray ray = GameplayCamera.ScreenPointToRay();
            IsVerticalHit = VerticalCollider.Raycast(ray, out verticalHit, 120);
            if (Mode != CursorMode.Vertical) return;
            EnableVertical = IsVerticalHit;
            if (IsVerticalHit)
            {
                VerticalX.DrawLine(new Vector3(-8.5f, AttachedVerticalPoint.y), new Vector3(8.5f, AttachedVerticalPoint.y));
                VerticalY.DrawLine(new Vector3(AttachedVerticalPoint.x, 0), new Vector3(AttachedVerticalPoint.x, 5.5f));
            }
        }
        private void RaycastSelecting()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = GameplayCamera.ScreenPointToRay();

                RaycastHit[] hits = Physics.RaycastAll(ray, 120, 1 << 9);
                ArcNote n = null;
                float distance = float.MaxValue;
                foreach (var h in hits)
                {
                    ArcNote t = ArcGameplayManager.Instance.FindNoteByInstance(h.transform.gameObject);
                    if (t is ArcArc && h.point.z > 0)
                    {
                        continue;
                    }
                    if (h.distance < distance)
                    {
                        distance = h.distance;
                        n = t;
                    }
                }
                if (n != null)
                {
                    if (!Input.GetKey(KeyCode.LeftControl))
                    {
                        DeselectAllNotes();
                        SelectNote(n);
                    }
                    else
                    {
                        if (SelectedNotes.Contains(n)) DeselectNote(n);
                        else SelectNote(n);
                    }
                }
                else
                {
                    if (!Input.GetKey(KeyCode.LeftControl) && IsHorizontalHit)
                    {
                        DeselectAllNotes();
                    }
                }

                /*RaycastHit hit;
                bool result = Physics.Raycast(ray, out hit, 120, 1 << 9);
                if (result)
                {
                    ArcNote note = ArcGameplayManager.Instance.FindNoteByInstance(hit.transform.gameObject);
                    if (note is ArcArc && hit.point.z > 0) return;
                    if (note != null)
                    {
                        if (!Input.GetKey(KeyCode.LeftControl))
                        {
                            DeselectAllNotes();
                            SelectNote(note);
                        }
                        else
                        {
                            if (SelectedNotes.Contains(note)) DeselectNote(note);
                            else SelectNote(note);
                        }
                    }
                }
                else
                {
                    if (!Input.GetKey(KeyCode.LeftControl) && IsHorizontalHit)
                    {
                        DeselectAllNotes();
                    }
                }*/
            }
        }
        private void DeleteListener()
        {
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                DeleteSelectedNotes();
            }
        }
        private void UpdateInfo()
        {
            EnableInfo = EnableVertical || EnableHorizontal;
            string content = string.Empty;
            if (!EnableInfo) return;
            content += string.Format(I.S["curmusictime"], AttachedTiming + ArcAudioManager.Instance.AudioOffset);
            content += string.Format(I.S["curcharttime"], AttachedTiming);
            if (EnableVertical)
            {
                Vector3 pos = AttachedVerticalPoint;
                content += string.Format(I.S["curcoord"], ArcAlgorithm.WorldXToArc(pos.x).ToString("f3"), ArcAlgorithm.WorldYToArc(pos.y).ToString("f3"));
            }
            if (AdeClickToCreate.Instance.Enable && AdeClickToCreate.Instance.Mode != ClickToCreateMode.Idle)
            {
                content += string.Format(I.S["curctc"], AdeClickToCreate.Instance.Mode);
                if (AdeClickToCreate.Instance.Mode == ClickToCreateMode.Arc)
                {
                    content += $"\n{AdeClickToCreate.Instance.CurrentArcColor}/{AdeClickToCreate.Instance.CurrentArcIsVoid}/{AdeClickToCreate.Instance.CurrentArcType}";
                }
            }
            if (SelectedNotes.Count == 1 && SelectedNotes[0] is ArcArc)
            {
                ArcArc arc = SelectedNotes[0] as ArcArc;
                float p = (AttachedTiming - arc.Timing) / (arc.EndTiming - arc.Timing);
                if (p >= 0 || p <= 1)
                {
                    float x = ArcAlgorithm.X(arc.XStart, arc.XEnd, p, arc.LineType);
                    float y = ArcAlgorithm.Y(arc.YStart, arc.YEnd, p, arc.LineType);
                    content += $"\nArc: {(p * 100):f2}%, {x:f3}, {y:f3}";
                }
            }
            InfoText.text = content;
            InfoRect.sizeDelta = new Vector2(InfoText.CalculateWidth(content) + 50, InfoText.CalculateHeight(content) + 20);
        }

        public void SelectNotesInRange(int start, int end)
        {
            if (!Input.GetKey(KeyCode.LeftControl))
                DeselectAllNotes();
            List<ArcNote> notes = new List<ArcNote>();
            notes.AddRange(ArcTapNoteManager.Instance.Taps.Where((a) => a.Timing >= start && a.Timing <= end));
            notes.AddRange(ArcHoldNoteManager.Instance.Holds.Where((a) => a.Timing >= start && a.Timing <= end
                    && a.EndTiming >= start && a.EndTiming <= end));
            var arcs = ArcArcManager.Instance.Arcs.Where((a) => a.Timing >= start && a.Timing <= end
                    && a.EndTiming >= start && a.EndTiming <= end);
            notes.AddRange(arcs);
            foreach (var a in arcs)
                notes.AddRange(a.ArcTaps);
            foreach(var note in notes)
            {
                if (note.Instance != null) note.Selected = true;
                if (!SelectedNotes.Contains(note))
                {
                    SelectedNotes.Add(note);
                    foreach (var l in NoteEventListeners) l.OnNoteSelect(note);
                }
            }
            AdeTimingEditor.Instance.OnEditingTimingGroupChanged(0);
        }
        public void SelectNote(ArcNote note)
        {
            if (note.Instance != null) note.Selected = true;
            if (!SelectedNotes.Contains(note))
            {
                SelectedNotes.Add(note);
                foreach (var l in NoteEventListeners) l.OnNoteSelect(note);
            }
            switch (note)
            {
                case ArcTap tap:
                    AdeTimingEditor.Instance.OnEditingTimingGroupChanged(tap.TimingGroup);
                    break;
                case ArcHold hold:
                    AdeTimingEditor.Instance.OnEditingTimingGroupChanged(hold.TimingGroup);
                    break;
                case ArcArc arc:
                    AdeTimingEditor.Instance.OnEditingTimingGroupChanged(arc.TimingGroup);
                    break;
                case ArcArcTap arctap:
                    AdeTimingEditor.Instance.OnEditingTimingGroupChanged(arctap.Arc.TimingGroup);
                    break;
            } 
        }
        public void DeselectNote(ArcNote note)
        {
            if (note.Instance != null) note.Selected = false;
            if (SelectedNotes.Contains(note))
            {
                SelectedNotes.Remove(note);
                foreach (var l in NoteEventListeners) l.OnNoteDeselect(note);
            }
            AdeTimingEditor.Instance.OnEditingTimingGroupChanged(0);
        }
        public void DeselectAllNotes()
        {
            foreach (var note in SelectedNotes) if (note.Instance != null) note.Selected = false;
            SelectedNotes.Clear();
            foreach (var l in NoteEventListeners) l.OnNoteDeselectAll();
        }
        public void DeleteSelectedNotes()
        {
            List<ICommand> deleteCommands = new List<ICommand>();
            foreach (var s in SelectedNotes)
            {
                if (s is ArcArcTap) deleteCommands.Add(new RemoveArcTapCommand((s as ArcArcTap).Arc, s as ArcArcTap));
                else deleteCommands.Add(new RemoveArcEventCommand(s));
            }
            if (deleteCommands.Count != 0) CommandManager.Instance.Add(new BatchCommand(deleteCommands.ToArray(),I.S["delete"]));
            SelectedNotes.Clear();
        }
    }
}