using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaikoTool.Core.Model.Chart.Gen2
{
    public class Fumen
    {
        public TableHeader Header { get; set; }

        public List<Frame> Frames { get; set; }

        public List<Drum> Drums { get; set; }

        public Fumen()
        {
            Header = new TableHeader();
            Frames = new List<Frame>();
            Drums = new List<Drum>();
        }
    }
}
