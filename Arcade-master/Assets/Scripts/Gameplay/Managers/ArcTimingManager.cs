using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Arcade.Gameplay.Chart;
using UnityEngine;

namespace Arcade.Gameplay
{
    public class ArcTimingGroup
    {
        public ArcTimingGroup(int id)
        {
            TimingGroup = id;
        }

        public int TimingGroup;
        public float BaseBpm 
        { 
            get
            {
                if (Timings.Count == 0 || Timings[0].Timing != 0)
                    Timings.Insert(0, new ArcTiming { TimingGroup = TimingGroup, BeatsPerLine = 4, Bpm = 128, Timing = 0 });
                return Timings[0].Bpm;
            } 
        }
        public float DropRate { get => ArcTimingManager.Instance.DropRate; }

        public List<ArcTiming> Timings = new List<ArcTiming>();
        private List<float> keyTimes = new List<float>();
        private readonly float[] starts = new float[25];
        private readonly float[] ends = new float[25];
        private int pairCount = 0;

        public void Add(ArcTiming timing)
        {
            Timings.Add(timing);
            Timings.Sort((ArcTiming a, ArcTiming b) => a.Timing.CompareTo(b.Timing));
        }
        public void Remove(ArcTiming timing)
        {
            Timings.Remove(timing);
        }

        public bool ShouldRender(int timing, int delay = 120)
        {
            for (int i = 0; i < pairCount; ++i)
            {
                if (timing >= starts[i] - delay && timing <= ends[i]) return true;
            }
            return false;
        }
        public void UpdateRenderRange()
        {
            pairCount = 0;
            keyTimes.Clear();

            keyTimes.Add(ArcGameplayManager.Instance.Timing);
            keyTimes.Add(ArcGameplayManager.Instance.Timing);

            keyTimes.Add(CalculateTimingByPosition(0, 0));
            keyTimes.Add(CalculateTimingByPosition(100000, 0));

            for (int i = 0; i < keyTimes.Count; i += 2)
            {
                starts[pairCount] = keyTimes[i];
                ends[pairCount] = keyTimes[i + 1];
                pairCount++;
            }
        }

        public float CalculatePositionByTiming(int timing)
        {
            return CalculatePositionByTimingAndStart(ArcGameplayManager.Instance.Timing, timing);
        }
        public float CalculatePositionByTimingAndStart(int startTiming, int timing)
        {
            int offset = ArcAudioManager.Instance.AudioOffset;
            int currentTiming = startTiming > timing ? timing : startTiming;
            int targetTiming = startTiming > timing ? startTiming : timing;
            bool reverse = startTiming > timing;
            float position = 0;
            int start = 0, end = 0;
            for (int i = 0; i < Timings.Count - 1; ++i)
            {
                if (currentTiming >= Timings[i].Timing + offset && currentTiming < Timings[i + 1].Timing + offset) { start = i; break; }
            }
            for (int i = 0; i < Timings.Count - 1; ++i)
            {
                if (targetTiming >= Timings[i].Timing + offset && targetTiming < Timings[i + 1].Timing + offset) { end = i; break; }
            }
            if (Timings.Count != 0)
            {
                if (currentTiming >= Timings[Timings.Count - 1].Timing + offset) start = Timings.Count - 1;
                if (targetTiming >= Timings[Timings.Count - 1].Timing + offset) end = Timings.Count - 1;
            }
            if (start == end)
            {
                position += (targetTiming - currentTiming) * Timings[start].Bpm / BaseBpm * DropRate;
            }
            else
            {
                for (int i = start; i <= end; ++i)
                {
                    if (i == start) position += (Timings[i + 1].Timing + offset - currentTiming) * Timings[i].Bpm / BaseBpm * DropRate;
                    else if (i != start && i != end) position += (Timings[i + 1].Timing - Timings[i].Timing) * Timings[i].Bpm / BaseBpm * DropRate;
                    else if (i == end) position += (targetTiming - Timings[i].Timing - offset) * Timings[i].Bpm / BaseBpm * DropRate;
                }
            }
            return reverse ? -position : position;
        }
        public int CalculateTimingByPosition(float position, int depth = 0)
        {
            int start = 0;
            int end = Timings.Count - 1;
            int breakPos = -1;
            float startPosition = 0;
            float endPosition = position;
            ArcGameplayManager gm = ArcGameplayManager.Instance;
            int offset = ArcAudioManager.Instance.AudioOffset;
            int currentTiming = gm.Timing;
            int songLength = gm.Length;
            for (int i = 0; i < Timings.Count - 1; ++i)
            {
                if (currentTiming >= Timings[i].Timing + offset && currentTiming < Timings[i + 1].Timing + offset) start = i;
            }
            if (currentTiming >= Timings[end].Timing + offset) start = end;

            int depthCount = 0;
            float delta = 0, endTime = 0;
            if (start != end)
            {
                for (int i = start; i <= end; ++i)
                {
                    if (i == start)
                    {
                        delta = (Timings[i + 1].Timing + offset - currentTiming) * (Timings[i].Bpm / BaseBpm) * DropRate;
                        if ((startPosition + delta <= endPosition && startPosition >= endPosition) || (startPosition + delta >= endPosition && startPosition <= endPosition)) { if (depth == depthCount) { breakPos = i; break; } else { depthCount++; } }
                        startPosition += delta;
                    }
                    else if (i != end && i != start)
                    {
                        delta = (Timings[i + 1].Timing - Timings[i].Timing) * (Timings[i].Bpm / BaseBpm) * DropRate;
                        if ((startPosition + delta < endPosition && startPosition > endPosition) || (startPosition + delta > endPosition && startPosition < endPosition)) { if (depth == depthCount) { breakPos = i; break; } else { depthCount++; } }
                        startPosition += delta;
                    }
                    else if (i == end)
                    {
                        delta = (songLength - Timings[i].Timing - offset) * (Timings[i].Bpm / BaseBpm) * DropRate;
                        if ((startPosition + delta < endPosition && startPosition > endPosition) || (startPosition + delta > endPosition && startPosition < endPosition)) { if (depth == depthCount) { breakPos = i; break; } else { depthCount++; } }
                        startPosition += delta;
                    }
                }
            }
            else if (start == end)
            {
                delta = (endPosition - startPosition);
                endTime = delta / ((Timings[end].Bpm / BaseBpm) * DropRate) + currentTiming;
                if (endTime > songLength) return songLength;
                else return (int)endTime;
            }
            if (breakPos == start)
            {
                delta = (endPosition - startPosition);
                if (delta == 0)
                {
                    if (Timings[breakPos].Bpm == 0) endTime = Timings[breakPos].Timing + offset;
                    else endTime = currentTiming;
                }
                else endTime = delta / ((Timings[breakPos].Bpm / BaseBpm) * DropRate) + currentTiming;
            }
            else if (breakPos != -1)
            {
                delta = (endPosition - startPosition);
                endTime = delta / ((Timings[breakPos].Bpm / BaseBpm) * DropRate) + Timings[breakPos].Timing + offset;
            }
            else if (breakPos == -1) endTime = songLength;
            if (endTime > songLength) return songLength;
            else return (int)endTime;
        }
        public float CalculateBpmByTiming(int timing)
        {
            for (int i = 0; i < Timings.Count - 1; ++i)
            {
                if (timing >= Timings[i].Timing && timing < Timings[i + 1].Timing) return Timings[i].Bpm;
            }
            return Timings.Last().Bpm;
        }
    }

    public class ArcTimingManager : MonoBehaviour
    {
        public static ArcTimingManager Instance { get; private set; } 
        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            speedShaderId = Shader.PropertyToID("_Speed");
        }

        public int DropRate = 100;
        public float BaseBpm { get => this[0].BaseBpm; }
        public Transform BeatlineLayer;
        public GameObject BeatlinePrefab; 
        public SpriteRenderer TrackRenderer;

        private List<float> beatlineTimings = new List<float>();
        private List<SpriteRenderer> beatLineInstances = new List<SpriteRenderer>();  
        private int speedShaderId = 0; 

        public float CurrentSpeed { get; set; }

        private ArcTimingGroup dummyTimingGroup = new ArcTimingGroup(0);
        private Dictionary<int, ArcTimingGroup> tgList = new Dictionary<int, ArcTimingGroup>();
        public IEnumerable<ArcTimingGroup> TimingGroups { get => tgList.Values; }
         
        /// <summary>
        /// Access the underlying TimingGroup
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ArcTimingGroup this[int index] 
        { 
            get
            {
                if (!tgList.ContainsKey(index))
                    return dummyTimingGroup;
                return tgList[index];
            }
        }

        private void Update()
        {
            if (tgList == null) return;
            if (tgList.Count == 0) return;
            UpdateChartSpeedStatus();
            UpdateRenderRange();
            UpdateBeatline();
            UpdateTrackSpeed();
        }

        public void Clean()
        { 
            CurrentSpeed = 0; 
            tgList.Clear();
            TrackRenderer.sharedMaterial.SetFloat(speedShaderId, 0);
            HideExceededBeatlineInstance(0);
        }
        public void Load(List<ArcTiming> timings)
        {
            tgList.Clear();
            foreach(var t in timings)
            {
                if (!tgList.ContainsKey(t.TimingGroup))
                    tgList[t.TimingGroup] = new ArcTimingGroup(t.TimingGroup);
                tgList[t.TimingGroup].Timings.Add(t);
                tgList[t.TimingGroup].Timings.Sort((ArcTiming a, ArcTiming b) => a.Timing.CompareTo(b.Timing));
            }
            CalculateBeatlineTimes();
        }
        private void HideExceededBeatlineInstance(int quantity)
        {
            int count = beatLineInstances.Count;
            while (count > quantity)
            {
                beatLineInstances[count - 1].enabled = false;
                count--;
            }
        }
        private SpriteRenderer GetBeatlineInstance(int index)
        {
            while (beatLineInstances.Count < index + 1)
            {
                beatLineInstances.Add(Instantiate(BeatlinePrefab, BeatlineLayer).GetComponent<SpriteRenderer>());
            }
            return beatLineInstances[index];
        }
        public void CalculateBeatlineTimes()
        {
            beatlineTimings.Clear();
            HideExceededBeatlineInstance(0);
            List<ArcTiming> Timings = this[0].Timings;

            for (int i = 0; i < Timings.Count - 1; ++i)
            {
                float segment = Timings[i].Bpm == 0 ? (Timings[i + 1].Timing - Timings[i].Timing) : (60000 / Mathf.Abs(Timings[i].Bpm) * Timings[i].BeatsPerLine);
                if (segment == 0) continue;
                int n = 0; 
                while (true)
                {
                    float j = Timings[i].Timing + n++ * segment;
                    if (j >= Timings[i + 1].Timing)
                        break;
                    beatlineTimings.Add(j);
                }
            }

            if (Timings.Count >= 1)
            {
                float segmentRemain = Timings[Timings.Count - 1].Bpm == 0 ? (ArcGameplayManager.Instance.Length - Timings[Timings.Count - 1].Timing)
              : 60000 / Mathf.Abs(Timings[Timings.Count - 1].Bpm) * Timings[Timings.Count - 1].BeatsPerLine;
                if (segmentRemain != 0)
                {
                    int n = 0;
                    float j = Timings[Timings.Count - 1].Timing;
                    while (j < ArcGameplayManager.Instance.Length)
                    {
                        j = Timings[Timings.Count - 1].Timing + n++ * segmentRemain;
                        beatlineTimings.Add(j);
                    }
                }
            }

            if (Timings.Count >= 1 && Timings[0].Bpm != 0 && Timings[0].BeatsPerLine != 0)
            {
                float t = 0;
                float delta = 60000 / Mathf.Abs(Timings[0].Bpm) * Timings[0].BeatsPerLine;
                int n = 0;
                if (delta != 0)
                {
                    while (t >= -3000)
                    {
                        n++;
                        t = -n * delta;
                        beatlineTimings.Insert(0, t);
                    }
                }
            }
        } 

        private void UpdateChartSpeedStatus()
        {
            int offset = ArcAudioManager.Instance.AudioOffset;
            int currentTiming = ArcGameplayManager.Instance.Timing;
            List<ArcTiming> Timings = this[0].Timings;
            if (Timings.Count == 0)
            {
                CurrentSpeed = 1;
                return;
            }
            for (int i = 0; i < Timings.Count - 1; ++i)
            {
                if (currentTiming >= Timings[i].Timing + offset && currentTiming < Timings[i + 1].Timing + offset)
                {
                    CurrentSpeed = Timings[i].Bpm / BaseBpm; 
                }
            }
            if (currentTiming >= Timings[Timings.Count - 1].Timing + offset)
            {
                CurrentSpeed = Timings[Timings.Count - 1].Bpm / BaseBpm; 
            }
        }
        private void UpdateRenderRange()
        {
            foreach (var tg in tgList.Values)
                tg.UpdateRenderRange();
        }
        private void UpdateBeatline()
        {
            int index = 0;
            int offset = ArcAudioManager.Instance.AudioOffset;
            foreach (float t in beatlineTimings)
            {
                if (!this[0].ShouldRender((int)(t + offset), 0))
                {
                    continue;
                }
                SpriteRenderer s = GetBeatlineInstance(index);
                s.enabled = true;
                float z = this[0].CalculatePositionByTiming((int)(t + offset)) / 1000f;
                s.transform.localPosition = new Vector3(0, 0, -z);
                s.transform.localScale = new Vector3(1700, 20 + z);
                index++;
            }
            HideExceededBeatlineInstance(index);
        }
        private void UpdateTrackSpeed()
        {
            TrackRenderer.sharedMaterial.SetFloat(speedShaderId, ArcGameplayManager.Instance.IsPlaying ? CurrentSpeed : 0);
        }

        public void Add(ArcTiming timing)
        {
            if (!tgList.ContainsKey(timing.TimingGroup))
                return;
            this[timing.TimingGroup].Add(timing); 
        }
        public void Remove(ArcTiming timing)
        {
            if (!tgList.ContainsKey(timing.TimingGroup))
                return;
            this[timing.TimingGroup].Remove(timing); 
        }  

        public void TryAddTimingGroup(int id)
        {
            if (tgList.ContainsKey(id))
                return;
            tgList.Add(id, new ArcTimingGroup(id));
            tgList[id].Timings.Insert(0, new ArcTiming { TimingGroup = id, BeatsPerLine = 4, Bpm = 128, Timing = 0 });
        }
    }
}