using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Arcade.Gameplay.Chart;
using Arcade.Compose;
using Arcade.Compose.MarkingMenu;
using Arcade.Gameplay;
using Arcade.Compose.Command;
using System.Linq;
using UnityEngine.EventSystems;
using Arcade.Compose.Editing;

namespace Arcade.Compose
{
    public class AdeCopyPaste : MonoBehaviour, IMarkingMenuItemProvider
    {
        public static AdeCopyPaste Instance { get; private set; }

        public MarkingMenuItem CopyItem;
        public MarkingMenuItem[] CopyingItems;

        public bool IsOnly => enable;
        public MarkingMenuItem[] Items
        {
            get
            {
                if (!enable)
                {
                    if (!ArcGameplayManager.Instance.IsLoaded) return null;
                    if (AdeCursorManager.Instance == null) return null;
                    if (AdeCursorManager.Instance.SelectedNotes.Count == 0) return null;
                    return new MarkingMenuItem[] { CopyItem };
                }
                else return CopyingItems;
            }
        }

        private bool enable;
        private IEnumerable<ArcEvent> notes = null;
        private CursorMode cursorMode = CursorMode.Idle;

        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            AdeMarkingMenuManager.Instance.Providers.Add(this);
        }
        private void OnDestroy()
        {
            AdeMarkingMenuManager.Instance.Providers.Remove(this);
        }
        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.C) && EventSystem.current.currentSelectedGameObject == null)
            {
                CopySelectedNotes();
            }
            if (!enable) return;
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Z))
            {
                CancelPaste();
                return;
            }
            UpdateTiming();
        }
        public void CopySelectedNotes()
        {
            Copy(AdeCursorManager.Instance.SelectedNotes);
        }
        public void Copy(IEnumerable<ArcNote> notes)
        {
            if (notes.Count() == 0) return;
            List<ICommand> commands = new List<ICommand>();
            List<ArcNote> newNotes = new List<ArcNote>();
            foreach (var n in notes)
            {
                ArcEvent ne = n.Clone();
                if (ne is ArcArcTap)
                {
                    if (!notes.Contains((n as ArcArcTap).Arc))
                    {
                        commands.Add(new AddArcTapCommand((n as ArcArcTap).Arc, ne as ArcArcTap));
                        newNotes.Add(ne as ArcNote);
                    }
                }
                else if (ne is ArcArc)
                {
                    commands.Add(new AddArcEventCommand(ne));
                    newNotes.Add(ne as ArcNote);
                    foreach (var at in (ne as ArcArc).ArcTaps)
                    {
                        newNotes.Add(at);
                    }
                }
                else
                {
                    commands.Add(new AddArcEventCommand(ne));
                    newNotes.Add(ne as ArcNote);
                }
            }
            CommandManager.Instance.Add(new BatchCommand(commands.ToArray(), I.S["copy"]));
            this.notes = newNotes;
            enable = true;
            cursorMode = AdeCursorManager.Instance.Mode;
            AdeCursorManager.Instance.Mode = CursorMode.Horizontal;
        }
        private void UpdateTiming()
        {
            if (!AdeCursorManager.Instance.IsHorizontalHit) return;
            Vector3 pos = AdeCursorManager.Instance.AttachedHorizontalPoint;
            int offset = ArcAudioManager.Instance.AudioOffset;
            bool canEnd = true;
            int beginTiming = notes.Min((n) => n.Timing);

            foreach (var n in notes)
            { 
                int dif = n.Timing - beginTiming;
                switch (n)
                {
                    case ArcArc note:
                        {
                            note.Judged = false;
                            int timing = ArcTimingManager.Instance[note.TimingGroup].CalculateTimingByPosition(-pos.z * 1000) - offset;
                            int duration = note.EndTiming - note.Timing;
                            note.Timing = timing + dif;
                            note.EndTiming = timing + duration + dif;
                            break;
                        }
                    case ArcHold note:
                        {
                            note.Judged = false;
                            int timing = ArcTimingManager.Instance[note.TimingGroup].CalculateTimingByPosition(-pos.z * 1000) - offset;
                            int duration = note.EndTiming - note.Timing;
                            note.Timing = timing + dif;
                            note.EndTiming = timing + duration + dif;
                            break;
                        }
                    case ArcArcTap note:
                        {
                            note.Judged = false;
                            int timing = ArcTimingManager.Instance[note.Arc.TimingGroup].CalculateTimingByPosition(-pos.z * 1000) - offset;
                            if (note.Arc.Timing > timing + dif || note.Arc.EndTiming < timing + dif)
                            {
                                canEnd = false;
                            }
                            note.RemoveArcTapConnection();
                            note.Timing = timing + dif;
                            note.Relocate();
                            note.SetupArcTapConnection();
                            break;
                        }
                    case ArcTap note:
                        {
                            note.Judged = false;
                            int timing = ArcTimingManager.Instance[note.TimingGroup].CalculateTimingByPosition(-pos.z * 1000) - offset;
                            note.Timing = timing + dif;
                            note.SetupArcTapConnection();
                            break;
                        }
                    case ArcTiming note:
                        {
                            int timing = ArcTimingManager.Instance[note.TimingGroup].CalculateTimingByPosition(-pos.z * 1000) - offset;
                            note.Timing = timing + dif;
                            break;
                        }
                }
            }

            foreach (var t in ArcTapNoteManager.Instance.Taps)
            {
                if (ArcTimingManager.Instance[t.TimingGroup].ShouldRender(t.Timing + offset))
                {
                    t.SetupArcTapConnection();
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (canEnd) Paste();
                else
                {
                    AdeToast.Instance.Show(I.S["copyerrinvalidnote"]);
                }
            }
        }
        public void CancelPaste()
        {
            CommandManager.Instance.Undo();
            Paste();
        }
        private void Paste()
        {
            EndOfFrame.Instance.Listeners.AddListener(() =>
            {
                enable = false;
                notes = null;
                AdeCursorManager.Instance.Mode = cursorMode;
            });
        }
    }
}