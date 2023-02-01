using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TaikoTool.Core.Model;
using TaikoTool.Core.Model.Chart.Gen3;

namespace TaikoTool.Core.Service
{
    public class FumenService : IFumenService
    {
        public virtual EndianType CurrentEndian { get; set; }

        public virtual CourseType CurrentCourse { get; set; }

        public virtual GameVersion Version { get; set; }

        public virtual void Write(Fumen fumen,Stream stream)
        {
            using var writer = new BinaryWriter(stream, Encoding.UTF8, true);

            var templateFile = GetTemplateFileName();
            var template = Directory.GetFiles(Path.Combine(
                AppContext.BaseDirectory, "Template", "Chart", Version.ToString()), templateFile).FirstOrDefault();

            if (template == null)
            {
                throw new Exception("can not find template");
            }

            using (var templateStream = File.OpenRead(template))
            using (var reader = new BinaryReader(templateStream))
            {
                for (int i = 0; i < fumen.TimingWindows.Length; i++)
                {
                    fumen.TimingWindows[i] = reader.ReadSingle(EndianType.Little);
                }
            }

            foreach (var t in fumen.TimingWindows)
            {
                writer.Write(t, CurrentEndian);
            }

            writer.Write(fumen.Branch);
            writer.Write(fumen.Unk1);
            writer.Write(fumen.Unk2);
            writer.Write(fumen.Unk3);

            writer.Write(fumen.GaugeMax, CurrentEndian);
            writer.Write(fumen.GaugeQuota, CurrentEndian);

            writer.Write(fumen.GaugeUpGreat, CurrentEndian);
            writer.Write(fumen.GaugeUpGood, CurrentEndian);
            writer.Write(fumen.GaugeUpMiss, CurrentEndian);

            writer.Write(fumen.HpIncreaseRatioNormal, CurrentEndian);
            writer.Write(fumen.HpIncreaseRatioExpert, CurrentEndian);
            writer.Write(fumen.HpIncreaseRatioMaster, CurrentEndian);

            writer.Write(fumen.GreatDivergePoint, CurrentEndian);
            writer.Write(fumen.GoodDivergePoint, CurrentEndian);
            writer.Write(fumen.MissDivergePoint, CurrentEndian);
            writer.Write(fumen.RendaDivergePoint, CurrentEndian);
            writer.Write(fumen.GreatDivergePointBig, CurrentEndian);
            writer.Write(fumen.GoodDivergePointBig, CurrentEndian);
            writer.Write(fumen.RendaDivergePointBig, CurrentEndian);
            writer.Write(fumen.BallonDivergePoint, CurrentEndian);
            writer.Write(fumen.BellDivergePoint, CurrentEndian);
            writer.Write(fumen.NumDivergePoint, CurrentEndian);

            writer.Write(fumen.MaxScoreValue, CurrentEndian);
            writer.Write(fumen.Count, CurrentEndian);
            writer.Write(fumen.Unk4, CurrentEndian);

            foreach (var section in fumen.Sections)
            {
                writer.Write(section.Bpm, CurrentEndian);
                writer.Write(section.Position,CurrentEndian);

                writer.Write(section.Fever);
                writer.Write(section.Line);
                writer.Write(section.Unk1);
                writer.Write(section.Unk2);

                writer.Write(section.RequiredDivergePointN2E, CurrentEndian);
                writer.Write(section.RequiredDivergePointN2M, CurrentEndian);
                writer.Write(section.RequiredDivergePointE2E, CurrentEndian);
                writer.Write(section.RequiredDivergePointE2M, CurrentEndian);
                writer.Write(section.RequiredDivergePointM2E, CurrentEndian);
                writer.Write(section.RequiredDivergePointM2M, CurrentEndian);

                writer.Write(section.Unk3, CurrentEndian);

                foreach (var branch in section.Branches)
                {
                    writer.Write(branch.Count, CurrentEndian);
                    writer.Write(branch.Unk1, CurrentEndian);
                    writer.Write(branch.Speed, CurrentEndian);

                    foreach (var note in branch.Notes)
                    {
                        writer.Write(note.Type, CurrentEndian);
                        writer.Write(note.Position, CurrentEndian);
                        writer.Write(note.Unk1, CurrentEndian);
                        writer.Write(note.Unk2, CurrentEndian);
                        writer.Write(note.Score, CurrentEndian);
                        writer.Write(note.Diff, CurrentEndian);
                        writer.Write(note.Length, CurrentEndian);

                        if (note.Type == 6 || note.Type == 9)
                        {
                            writer.Write(0, CurrentEndian);
                            writer.Write(0, CurrentEndian);
                        }
                    }
                }
            }
        }

        public virtual Fumen Read(Stream stream)
        {
            using var reader = new BinaryReader(stream, Encoding.UTF8, true);
            var result = new Fumen();

            for (int i = 0; i < result.TimingWindows.Length;i++)
            {
                result.TimingWindows[i] = reader.ReadSingle(CurrentEndian);
            }

            result.Branch = reader.ReadByte();
            result.Unk1 = reader.ReadByte();
            result.Unk2 = reader.ReadByte();
            result.Unk3 = reader.ReadByte();

            result.GaugeMax = reader.ReadUInt32(CurrentEndian);
            result.GaugeQuota = reader.ReadUInt32(CurrentEndian);

            result.GaugeUpGreat= reader.ReadInt32(CurrentEndian);
            result.GaugeUpGood= reader.ReadInt32(CurrentEndian);
            result.GaugeUpMiss= reader.ReadInt32(CurrentEndian);

            result.HpIncreaseRatioNormal=reader.ReadUInt32(CurrentEndian);
            result.HpIncreaseRatioExpert= reader.ReadUInt32(CurrentEndian);
            result.HpIncreaseRatioMaster=reader.ReadUInt32(CurrentEndian);

            result.GreatDivergePoint=reader.ReadUInt32(CurrentEndian);
            result.GoodDivergePoint=reader.ReadUInt32(CurrentEndian);
            result.MissDivergePoint=reader.ReadUInt32(CurrentEndian);
            result.RendaDivergePoint=reader.ReadUInt32(CurrentEndian);
            result.GreatDivergePointBig=reader.ReadUInt32(CurrentEndian);
            result.GoodDivergePointBig=reader.ReadUInt32(CurrentEndian);
            result.RendaDivergePointBig=reader.ReadUInt32(CurrentEndian);
            result.BallonDivergePoint=reader.ReadUInt32(CurrentEndian);
            result.BellDivergePoint=reader.ReadUInt32(CurrentEndian);
            result.NumDivergePoint=reader.ReadUInt32(CurrentEndian);

            result.MaxScoreValue=reader.ReadUInt32(CurrentEndian);
            result.Count=  reader.ReadUInt32(CurrentEndian);
            result.Unk4=reader.ReadUInt32(CurrentEndian);

            if (result.Count > 300)
            {
                throw new ArgumentOutOfRangeException(
                    "section count", 
                    "section count is more than 300,endianness maybe wrong.");
            }

            for(int i = 0; i < result.Count; i++) 
            {
                var section = new Section
                {
                    Bpm = reader.ReadSingle(CurrentEndian),
                    Position = reader.ReadSingle(CurrentEndian),

                    Fever = reader.ReadByte(),
                    Line = reader.ReadByte(),
                    Unk1 = reader.ReadByte(),
                    Unk2 = reader.ReadByte(),

                    RequiredDivergePointN2E = reader.ReadInt32(CurrentEndian),
                    RequiredDivergePointN2M = reader.ReadInt32(CurrentEndian),
                    RequiredDivergePointE2E = reader.ReadInt32(CurrentEndian),
                    RequiredDivergePointE2M = reader.ReadInt32(CurrentEndian),
                    RequiredDivergePointM2E = reader.ReadInt32(CurrentEndian),
                    RequiredDivergePointM2M = reader.ReadInt32(CurrentEndian),

                    Unk3 = reader.ReadUInt32(CurrentEndian)
                };

                for (int j = 0; j < 3; j++)
                {
                    var branch = section.Branches[j] = new Branch
                    {
                        Count = reader.ReadUInt16(CurrentEndian),
                        Unk1 = reader.ReadUInt16(CurrentEndian),
                        Speed = reader.ReadSingle(CurrentEndian),
                    };

                    for (int k = 0; k < branch.Count; k++)
                    {
                        branch.Notes.Add(new Note
                        {
                            Type = reader.ReadUInt32(CurrentEndian),
                            Position = reader.ReadSingle(CurrentEndian),
                            Unk1 = reader.ReadInt32(CurrentEndian),
                            Unk2 = reader.ReadInt32(CurrentEndian),
                            Score = reader.ReadUInt16(CurrentEndian),
                            Diff = reader.ReadUInt16(CurrentEndian),
                            Length = reader.ReadSingle(CurrentEndian),
                        });

                        if (branch.Notes[k].Type == 6
                            || branch.Notes[k].Type == 9)
                        {
                            _ = reader.ReadBytes(8);
                        }
                    }
                }

                result.Sections.Add(section);
            }

            return result;
        }

        protected virtual string GetTemplateFileName()
        => CurrentCourse switch
        {
            CourseType.Easy => "*_e.bin",
            CourseType.Normal => "*_n.bin",
            CourseType.Hard => "*_h.bin",
            CourseType.Mania => "*_m.bin",
            CourseType.Extrem => "*_x.bin",
            _ => throw new InvalidDataException("unsupported course")
        };
    }
}
