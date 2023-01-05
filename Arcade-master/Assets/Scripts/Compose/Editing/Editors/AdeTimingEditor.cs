using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Arcade.Gameplay.Chart;
using System.Linq;
using Arcade.Gameplay;
using Arcade.Compose.Command;
using UnityEngine.EventSystems;

namespace Arcade.Compose.Editing
{
    public class AdeTimingEditor : MonoBehaviour
    {
        public static AdeTimingEditor Instance { get; private set; }

        public GameObject View; 
        public InputField TimingGroup;
        public Scrollbar ViewScrollBar;

        public int EditingTimingGroup = 0;

        public List<AdeTimingItem> TimingInstances = new List<AdeTimingItem>();
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            ArcGameplayManager.Instance.OnChartLoad.AddListener(BuildList);
        }
        private void OnDestroy()
        {
            ArcGameplayManager.Instance.OnChartLoad.RemoveListener(BuildList);
        }  
        private string GetTimingString(ArcTiming timing)
        {
            return $"{timing.Timing},{timing.Bpm:f2},{timing.BeatsPerLine:f2}";
        }

        public void Add(ArcTiming caller)
        {
            ArcTiming clone = caller.Clone() as ArcTiming;
            clone.TimingGroup = EditingTimingGroup;
            if (clone.Timing == 0)
                clone.Timing = 1;
            CommandManager.Instance.Add(new AddArcEventCommand(clone));
            BuildList();
        }
        public void Delete(ArcTiming caller)
        {
            CommandManager.Instance.Add(new RemoveArcEventCommand(caller));
            BuildList();
        }
        public void BuildList()
        { 
            List<ArcTiming> timings = ArcTimingManager.Instance[EditingTimingGroup].Timings;
            int count = timings.Count;
            int extracount = count - 10;
            extracount = extracount < 0 ? 0 : extracount;
            float size = 10f / (extracount + 10);
            ViewScrollBar.size = size;

            float pos = ViewScrollBar.value;
            int startpos = Mathf.RoundToInt(pos * extracount); 
            int endpos = startpos + 10;
            endpos = endpos > count ? count : endpos;

            int itemidx = 0;
            for(int k = startpos; k < endpos; ++k)
            {
                AdeTimingItem item = TimingInstances[itemidx++];
                ArcTiming t = timings[k];
                item.Text = GetTimingString(t);
                item.TimingReference = t;
                item.RemoveBtn.interactable = t.Timing != 0;
                item.ItemInputField.interactable = true;
                item.AddBtn.interactable = true;
            } 

            //Disable unused
            for(int k = itemidx; k < 10; ++k)
            {
                AdeTimingItem item = TimingInstances[itemidx++]; 
                item.Text = string.Empty;
                item.TimingReference = null;
                item.RemoveBtn.interactable = false;
                item.ItemInputField.interactable = false;
                item.AddBtn.interactable = false;
            }
        }
        public void OnViewScroll()
        {
            BuildList();
        }
        public void OnViewMouseScroll(BaseEventData e) 
        {
            PointerEventData pe = e as PointerEventData;
            float delta = pe.scrollDelta.y; 
            List<ArcTiming> timings = ArcTimingManager.Instance[EditingTimingGroup].Timings;
            int count = timings.Count;
            int extracount = count - 10;
            extracount = extracount < 0 ? 0 : extracount;
            float val = 1 / (extracount + 1f);
            float newval = ViewScrollBar.value - val * delta;
            ViewScrollBar.value = Mathf.Clamp(newval, 0, 1);
        }

        public void SwitchStatus()
        {
            View.SetActive(!View.activeSelf);
            if (View.activeSelf)
            {
                BuildList();
            } 
        }

        //v3.0.0
        public void OnEditingTimingGroupChanged(int editing)
        {
            EditingTimingGroup = editing;
            TimingGroup.text = editing.ToString();
            BuildList();
        }
        public void OnEditingTimingGroupChanged(InputField i)
        {
            EditingTimingGroup = int.Parse(i.text);
            ArcTimingManager.Instance.TryAddTimingGroup(EditingTimingGroup);
            BuildList();
        }
        public void OnEditingTimingGroupAdd()
        {
            OnEditingTimingGroupChanged(EditingTimingGroup + 1);
        }
        public void OnEditingTimingGroupMinus()
        {
            OnEditingTimingGroupChanged(EditingTimingGroup - 1 < 0 ? 0 : EditingTimingGroup - 1);
        }
    }
}