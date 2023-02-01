using System.Collections.Generic;

namespace TaikoTool.Core.Model.Chart.Gen3
{
    public class Section
    {
        public float Bpm { get; set; }

        public float Position { get; set; }

        public byte Fever { get; set; }

        public byte Line { get; set; }

        public byte Unk1 { get; set; }

        public byte Unk2 { get; set; }

        public int RequiredDivergePointN2E { get; set; }

        public int RequiredDivergePointN2M { get; set; }

        public int RequiredDivergePointE2E { get; set; }

        public int RequiredDivergePointE2M { get; set; }

        public int RequiredDivergePointM2M { get; set; }

        public int RequiredDivergePointM2E { get; set; }

        public uint Unk3 { get; set; }

        public Branch[] Branches { get; set; }

        public Section()
        {
            Branches = new Branch[]
            {
                new Branch(),
                new Branch(),
                new Branch()
            };
        }
    }
}
