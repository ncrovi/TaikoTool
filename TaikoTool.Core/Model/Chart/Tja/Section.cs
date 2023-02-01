using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaikoTool.Core.Model.Chart.Tja
{
    public class Section
    {
        public double BPM { get; set; }

        public int NotesCount { get; set; }

        public double Length { get; set; }

        public bool GoGoTime { get; set; }

        public bool Barline { get; set; }

        public double Delay { get; set; }

        public double Scroll { get; set; }

        public double MeasureBeats { get; set; }

        public IList<Note> Notes { get; set; }
            = new List<Note>();
    }
}
