using System.Collections.Generic;

namespace TaikoTool.Core.Model.Chart.Gen3
{
    public class Branch
    {
        public ushort Count { get; set; }

        public ushort Unk1 { get; set; }

        public float Speed { get; set; }

        public List<Note> Notes { get; set; }

        public Branch()
        {
            Notes=new List<Note>();
        }
    }
}
