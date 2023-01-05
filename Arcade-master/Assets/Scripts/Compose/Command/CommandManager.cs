using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Arcade.Compose.Dialog; 

namespace Arcade.Compose.Command
{
    public class CommandManager : MonoBehaviour
    {
        public static CommandManager Instance { get; private set; }
        private void Awake()
        {
            Instance = this;
        }
        private bool undoClickable, redoClickable;
        private bool UndoClickable
        {
            get
            {
                return undoClickable;
            }
            set
            {
                if (undoClickable != value)
                {
                    UndoButton.interactable = value;
                    undoClickable = value;
                }
            }
        }
        private bool RedoClickable
        {
            get
            {
                return redoClickable;
            }
            set
            {
                if (redoClickable != value)
                {
                    RedoButton.interactable = value;
                    redoClickable = value;
                }
            }
        }
        public Button UndoButton, RedoButton;
        public Text Status;

        private Stack<ICommand> undo = new Stack<ICommand>();
        private Stack<ICommand> redo = new Stack<ICommand>();

        private void Start()
        {
            Status.text = string.Format(I.S["addcmd"], undo.Count, redo.Count);
            I.S.OnLocaleChanged.AddListener(OnLocaleChanged);
        }
        private void OnDestroy()
        {
            I.S.OnLocaleChanged.RemoveListener(OnLocaleChanged);
        }
        private void OnLocaleChanged()
        {
            Status.text = string.Format(I.S["addcmd"], undo.Count, redo.Count);
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (Input.GetKeyDown(KeyCode.Z)) Undo();
                else if (Input.GetKeyDown(KeyCode.Y)) Redo();
            }
            UndoClickable = undo.Count != 0;
            RedoClickable = redo.Count != 0;
        }

        public void Add(ICommand command)
        {
            command.Do();
            AdeToast.Instance.Show(string.Format(I.S["execcmd"], command.Name));
            undo.Push(command);
            redo.Clear();
            Status.text = string.Format(I.S["addcmd"], undo.Count, redo.Count);
        }
        public void Undo()
        {
            if (undo.Count == 0) return;
            ICommand cmd = undo.Pop();
            cmd.Undo();
            AdeToast.Instance.Show(string.Format(I.S["undocmd"], cmd.Name));
            redo.Push(cmd);
        }
        public void Redo()
        {
            if (redo.Count == 0) return;
            ICommand cmd = redo.Pop();
            cmd.Do();
            AdeToast.Instance.Show(string.Format(I.S["redocmd"], cmd.Name));
            undo.Push(cmd);
        }
        public void Free()
        {
            AdeDualDialog.Instance.Show(I.S["confirmclearcmd"], null, null, null, () => FreeConfirmed());
        }
        public void FreeConfirmed()
        {
            undo.Clear();
            redo.Clear();
            AdeToast.Instance.Show(I.S["clearedcmd"]);
            Status.text = string.Format(I.S["addcmd"], undo.Count, redo.Count);
        }
        public void FreeSilent()
        {
            undo.Clear();
            redo.Clear(); 
            Status.text = string.Format(I.S["addcmd"], undo.Count, redo.Count);
        }
    }
}
