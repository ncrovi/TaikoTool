using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaikoTool.Core.Model;
using TaikoTool.Core.Model.Chart.Gen3;

namespace TaikoTool.Core.Service
{
    public class IosFumenService:FumenService
    {
        public override EndianType CurrentEndian => EndianType.Little;

        public override GameVersion Version => GameVersion.IOS;

        public override void Write(Fumen fumen, Stream stream)
        {
            fumen.Unk1 = 1;
            fumen.Unk2 = 57;
            fumen.Unk3 = 0;

            fumen.NumDivergePoint = 0;
            fumen.Unk4 = 4208470;
            fumen.MaxScoreValue = 1000000;

            foreach (var section in fumen.Sections)
            {
                section.Unk3 = 1244404;

                foreach (var branch in section.Branches)
                {
                    branch.Unk1 = 57;

                    foreach (var note in branch.Notes)
                    {
                        note.Unk1 = 1244160;
                        note.Unk2 = 4429192;
                        note.Diff = Convert.ToUInt16(note.Diff / 4);
                    }
                }
            }

            base.Write(fumen, stream);
        }
    }
}
