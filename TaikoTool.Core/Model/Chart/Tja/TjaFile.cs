using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaikoTool.Core.Model.Chart.Tja
{
    public class TjaFile
    {
        public double InitBpm { get; set; }

        public double Offset { get; set; }

        public string Title { get; set; }

        public string SubTitle { get; set; }

        public double DemoStart { get; set; }

        public Fumen[] Fumens { get; set; }
    }
}
