using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaikoTool.Core.Model.Chart.Tja;

namespace TaikoTool.Core
{
    public interface ITjaService
    {
        Encoding CurrentEncoding { get; set; }

        TjaFile Deserialize(Stream stream);

        byte[] Serialize(TjaFile tja);
    }
}
