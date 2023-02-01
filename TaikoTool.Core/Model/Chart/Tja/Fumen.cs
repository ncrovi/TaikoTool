using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaikoTool.Core.Model.Chart.Tja
{
    public class Fumen
    {
        public int Level { get; set; }

        public int Course { get; set; }

        public int ScoreInit { get; set; }

        public int ScoreDiff { get; set; }

        public int ScoreShin { get; set; }

        public IList<Section> Sections { get; set; }
            = new List<Section>();
    }
}
