using Arcade.Compose;
using Arcade.Gameplay;
using Arcade.Gameplay.Chart;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 
using System;
using System.Linq;
using UnityEngine.UI;
using System.IO;

namespace Arcade.Compose.Feature
{
    public abstract class Fault
    {
        public Fault(string Reason)
        {
            this.Reason = Reason;
        }
        public List<ArcEvent> Faults = new List<ArcEvent>();
        public string Reason;

        public abstract void Check();
    }

    public class ShortHoldFault : Fault
    {
        public ShortHoldFault() : base(I.S["shortholdfault"])
        {

        }
        public override void Check()
        {
            foreach (var h in ArcHoldNoteManager.Instance.Holds)
            {
                if (h.EndTiming <= h.Timing)
                {
                    Faults.Add(h);
                }
            }
            Faults = Faults.Distinct().ToList();
        }
    }
    public class ShortArcFault : Fault
    {
        public ShortArcFault() : base(I.S["shortarcfault"])
        {

        }
        public override void Check()
        {
            foreach (var a in ArcArcManager.Instance.Arcs)
            {
                if (a.EndTiming <= a.Timing)
                {
                    if (a.XEnd == a.XStart)
                    {
                        if (a.YEnd == a.YStart)
                        {
                            Faults.Add(a);
                        }
                    }
                }
            }
            Faults = Faults.Distinct().ToList();
        }
    }
    public class TileTapFault : Fault
    {
        public TileTapFault() : base(I.S["tiletapfault"])
        {

        }
        public override void Check()
        {
            for (int i = 0; i < ArcTapNoteManager.Instance.Taps.Count - 1; ++i)
            {
                for (int k = i + 1; k < ArcTapNoteManager.Instance.Taps.Count; ++k)
                {
                    if (ArcTapNoteManager.Instance.Taps[i].Timing == ArcTapNoteManager.Instance.Taps[k].Timing
                        && ArcTapNoteManager.Instance.Taps[i].Track == ArcTapNoteManager.Instance.Taps[k].Track)
                    {
                        Faults.Add(ArcTapNoteManager.Instance.Taps[i]);
                    }
                }
            }
            Faults = Faults.Distinct().ToList();
        }
    }
    public class TileArcTapFault : Fault
    {
        public TileArcTapFault() : base(I.S["tilearctapfault"])
        {

        }
        public override void Check()
        {
            List<(ArcArcTap, float, float)> ats = new List<(ArcArcTap, float, float)>();
            foreach (var arc in ArcArcManager.Instance.Arcs)
            {
                foreach (var at in arc.ArcTaps)
                {
                    float t = 1f * (at.Timing - arc.Timing) / (arc.EndTiming - arc.Timing);
                    float x = ArcAlgorithm.X(arc.XStart, arc.XEnd, t, arc.LineType);
                    float y = ArcAlgorithm.Y(arc.YStart, arc.YEnd, t, arc.LineType);
                    ats.Add((at, x, y));
                }
            }
            for (int i = 0; i < ats.Count - 1; ++i)
            {
                for (int k = i + 1; k < ats.Count; ++k)
                {
                    if (ats[i].Item2 == ats[k].Item2
                        && ats[i].Item3 == ats[k].Item3
                        && ats[i].Item1.Timing == ats[k].Item1.Timing)
                    {
                        Faults.Add(ats[i].Item1);
                    }
                }
            }
            Faults = Faults.Distinct().ToList();
        }
    }
    public class TileTapHoldFault : Fault
    {
        public TileTapHoldFault() : base(I.S["tiletapholdfault"])
        {

        }
        public override void Check()
        {
            foreach (var h in ArcHoldNoteManager.Instance.Holds)
            {
                foreach (var t in ArcTapNoteManager.Instance.Taps)
                {
                    if (t.Timing >= h.Timing && t.Timing <= h.EndTiming
                        && t.Track == h.Track)
                    {
                        Faults.Add(t);
                    }
                }
            }
            Faults = Faults.Distinct().ToList();
        }
    }
    public class TileHoldFault : Fault
    {
        public TileHoldFault() : base(I.S["tileholdfault"])
        {

        }
        public override void Check()
        {
            for (int i = 0; i < ArcHoldNoteManager.Instance.Holds.Count - 1; ++i)
            {
                for (int k = i + 1; k < ArcHoldNoteManager.Instance.Holds.Count; ++k)
                {
                    if (ArcHoldNoteManager.Instance.Holds[k].Timing <= ArcHoldNoteManager.Instance.Holds[i].EndTiming
                        && ArcHoldNoteManager.Instance.Holds[k].Timing >= ArcHoldNoteManager.Instance.Holds[i].Timing
                        && ArcHoldNoteManager.Instance.Holds[i].Track == ArcHoldNoteManager.Instance.Holds[k].Track)
                    {
                        Faults.Add(ArcHoldNoteManager.Instance.Holds[k]);
                    }
                }
            }
            Faults = Faults.Distinct().ToList();
        }
    }
    public class CrossTimingFault : Fault
    {
        public CrossTimingFault() : base(I.S["crosstimingfault"])
        {

        }
        public override void Check()
        {
            //foreach (var h in ArcHoldNoteManager.Instance.Holds)
            //{
            //    foreach (var t in ArcTimingManager.Instance[h.TimingGroup].Timings)
            //    {
            //        if (t.Timing > h.Timing && t.Timing < h.EndTiming)
            //        {
            //            Faults.Add(h);
            //        }
            //    }
            //}
            foreach (var a in ArcArcManager.Instance.Arcs)
            {
                foreach (var t in ArcTimingManager.Instance[a.TimingGroup].Timings)
                {
                    if (t.Timing > a.Timing && t.Timing < a.EndTiming)
                    {
                        Faults.Add(a);
                    }
                }
            }
            Faults = Faults.Distinct().ToList();
        }
    }

    public class AdeFaultDetector : MonoBehaviour
    {
        public Text Status;
        public void OnInvoke()
        {
            if (!ArcGameplayManager.Instance.IsLoaded)
            {
                AdeToast.Instance.Show(I.S["loadchartfirst"]);
                return;
            }

            Status.text = I.S["faultclickcheck"];
             
            Fault[] checks = new Fault[] {new ShortHoldFault(), new ShortArcFault(), new TileTapFault(), new TileArcTapFault(), new TileTapHoldFault(),
                                      new TileHoldFault(), new CrossTimingFault()};
            string path = AdeProjectManager.Instance.CurrentProjectFolder + "/Arcade/ChartFault.txt";
            FileStream fs = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            int count = 0;
            sw.WriteLine(I.S["errorfaultreport"]);
            sw.WriteLine(I.S["errorfaultreportexplain1"]);
            sw.WriteLine(I.S["errorfaultreportexplain2"]);
            sw.WriteLine();
            foreach (var c in checks)
            {
                c.Check();
                if (c.Faults.Count > 0)
                {
                    sw.WriteLine(c.Reason);
                    foreach (var f in c.Faults)
                    {
                        sw.WriteLine(string.Format(I.S["errorfaultreportitem"], f.Timing, f.Timing + ArcAudioManager.Instance.AudioOffset));
                    }
                    count += c.Faults.Count;
                }
            }

            sw.Close();
            Status.text = string.Format(I.S["errorfaultreportend"], count);
            Schwarzer.Windows.Dialog.OpenExplorer(path);
        }
    }
}