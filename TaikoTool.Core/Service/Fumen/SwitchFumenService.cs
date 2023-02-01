using System.IO;
using TaikoTool.Core.Model;
using Gen3=TaikoTool.Core.Model.Chart.Gen3;
using System.Linq;

namespace TaikoTool.Core.Service
{
    public class SwitchFumenService : FumenService
    {
        public override EndianType CurrentEndian => EndianType.Little;

        public override GameVersion Version => GameVersion.Switch;

        public override void Write(Gen3.Fumen fumen,Stream stream)
        {
            fumen.Unk1 = 0;
            fumen.Unk2 = 0;
            fumen.Unk3 = 0;

            fumen.Unk4 = 0;

            foreach (var section in fumen.Sections)
            {
                section.Unk1 = 0;
                section.Unk2 = 0;

                section.Unk3 = 0;

                foreach (var branch in section.Branches)
                {
                    branch.Unk1 = 0;

                    foreach (var note in branch.Notes)
                    {
                        note.Unk1 = 0;
                        note.Unk2 = 0;

                        if (note.Type == 0x06 || note.Type == 0x09 || note.Type == 0x0A)
                        { }
                        else
                        {
                            note.Length = 0;
                        }
                    }
                }
            }

            base.Write(fumen, stream);
        }
    }
}
