using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaikoTool.Core.Model.Chart.Gen2
{
    public class TableHeader
    {
        public int FramesCount { get; set; }

        public uint DrumTableOffset { get; set; }

        public uint DrumsCount { get; set; }

        public uint Unknowned { get; set; }
    }
}
