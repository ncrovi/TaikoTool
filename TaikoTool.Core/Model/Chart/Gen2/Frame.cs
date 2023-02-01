using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaikoTool.Core.Model.Chart.Gen2
{
    public class Frame
    {
        public float Time1 { get; set; }

        public float BPM { get; set; }

        public uint Flag1 { get; set; }

        public uint Flag2 { get; set; }

        public uint Unknowned1 { get; set; }

        public uint Flag3 { get; set; }

        /// <summary>
        /// length 0x14
        /// </summary>
        public int[] Unknowned2 { get; set; }

        public float Speed { get; set; }

        /// <summary>
        /// length 0x14
        /// </summary>
        public int[] Unknowned3 { get; set; }

        public uint DrumsOffset { get; set; }

        public uint DrumsCount { get; set; }

        public uint Flag4 { get; set; }

        /// <summary>
        /// length 0x1c
        /// </summary>
        public int[] Unknowned4 { get; set; }

        public uint DrumsCount2 { get; set; }

        /// <summary>
        /// length 0x1c
        /// </summary>
        public int[] Unknowned5 { get; set; }
    }
}
