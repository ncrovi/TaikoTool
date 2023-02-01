using System.Collections.Generic;

namespace TaikoTool.Core.Model.Chart.Gen3
{
    public class Fumen
    {
        public float[] TimingWindows { get; set; }

        public byte Branch { get; set; }

        public byte Unk1 { get; set; }

        public byte Unk2 { get; set; }

        public byte Unk3 { get; set; }

        public uint GaugeMax { get; set; }

        public uint GaugeQuota { get; set; }

        public int GaugeUpGreat { get; set; }

        public int GaugeUpGood { get; set; }

        public int GaugeUpMiss { get; set; }

        public uint HpIncreaseRatioNormal { get; set; }

        public uint HpIncreaseRatioExpert { get; set; }

        public uint HpIncreaseRatioMaster { get; set; }

        public uint GreatDivergePoint { get; set; }

        public uint GoodDivergePoint { get; set; }

        public uint MissDivergePoint { get; set; }

        public uint RendaDivergePoint { get; set; }

        public uint GreatDivergePointBig { get; set; }

        public uint GoodDivergePointBig { get; set; }

        public uint RendaDivergePointBig { get; set; }

        public uint BallonDivergePoint { get; set; }

        public uint BellDivergePoint { get; set; }

        public uint NumDivergePoint { get; set; }

        public uint MaxScoreValue { get; set; }

        public uint Count { get; set; }

        public uint Unk4 { get; set; }

        public List<Section> Sections { get; set; }

        public Fumen()
        {
            TimingWindows = new float[0x6c];
            Sections = new List<Section>();
        }
    }
}
