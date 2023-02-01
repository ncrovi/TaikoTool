using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaikoTool.Core.Model.Chart.Tja
{
    public class Note
    {
        public int Position { get; set; }

        public NoteType Type { get; set; }

        public int BalloonCount { get; set; }
    }

    public enum NoteType
    {
        Red = 1,
        Blue = 2,
        BigRed = 3,
        BigBlue = 4,
        Yellow = 5,
        BigYellow = 6,
        Balloon = 7,

        DurationEnd = 8,
        SweetPotato = 9,
        Blank = 0
    }
}
