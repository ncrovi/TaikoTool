using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaikoTool.Core.Model;
using Gen3 = TaikoTool.Core.Model.Chart.Gen3;
using Tja=TaikoTool.Core.Model.Chart.Tja;
using System.Text.Json;
using System.IO;

namespace TaikoTool.Core.Profile.Fumen
{
    public class TjaProfile:AutoMapper.Profile
    {
        public TjaProfile()
        {
            CreateMap<Tja.Fumen, Gen3.Fumen>().ConvertUsing(new Gen3FumenConverter());
        }
    }

    public class Gen3FumenConverter : AutoMapper.ITypeConverter<Tja.Fumen, Gen3.Fumen>
    {
        public Gen3.Fumen Convert(Tja.Fumen source, Gen3.Fumen destination, AutoMapper.ResolutionContext context)
        {
            if (destination == null) destination = new Gen3.Fumen();

            var noteCounts = source.Sections.SelectMany(r => r.Notes.Where(r => (int)r.Type < 5)).Count();

            destination.GaugeMax = 10000;
            destination.GaugeQuota = source.Course switch
            {
                0 => 6000,
                1 or 2 => 7000,
                _ => 8000,
            };

            destination.GaugeUpGreat = System.Convert.ToInt32(Math.Ceiling(destination.GaugeQuota / (noteCounts * 0.6)));
            destination.GaugeUpGood = source.Course switch
            {
                0 or 1 or 2 => System.Convert.ToInt32(Math.Floor(0.75 * destination.GaugeUpGreat)),
                3 => System.Convert.ToInt32(Math.Floor(0.5 * destination.GaugeUpGreat)),
                _ => System.Convert.ToInt32(Math.Ceiling(0.5 * destination.GaugeUpGreat))
            };
            destination.GaugeUpMiss = source.Course switch
            {
                0 => destination.GaugeUpGreat / -2,
                1 => -destination.GaugeUpGreat,
                2 => System.Convert.ToInt32(Math.Ceiling(-1.25 * destination.GaugeUpGreat)),
                _ => -2 * destination.GaugeUpGreat,
            };

            destination.HpIncreaseRatioNormal = 65535;
            destination.HpIncreaseRatioExpert = 65535;
            destination.HpIncreaseRatioMaster = 65535;

            destination.GreatDivergePoint = 20;
            destination.GoodDivergePoint = 10;
            destination.MissDivergePoint = 0;
            destination.RendaDivergePoint = 1;
            destination.GreatDivergePointBig = 20;
            destination.GoodDivergePointBig = 10;
            destination.RendaDivergePointBig = 1;
            destination.BallonDivergePoint = 30;
            destination.BellDivergePoint = 30;
            destination.NumDivergePoint = 20;

            destination.MaxScoreValue = 1000000;

            var currentBpm = source.Sections[0].BPM;
            var currentPosition = 0D;

            for (int i = 0; i < source.Sections.Count; i++)
            {
                var section = new Gen3.Section
                {
                    Bpm = System.Convert.ToSingle(source.Sections[i].BPM),
                    Fever = System.Convert.ToByte(source.Sections[i].GoGoTime ? 1 : 0),
                    Line = System.Convert.ToByte(source.Sections[i].Barline ? 1 : 0),
                    RequiredDivergePointN2E = -1,
                    RequiredDivergePointN2M = -1,
                    RequiredDivergePointE2E = -1,
                    RequiredDivergePointE2M = -1,
                    RequiredDivergePointM2E = -1,
                    RequiredDivergePointM2M = -1
                };

                section.Position = System.Convert.ToSingle((240000D / currentBpm)
                    - (240000D / source.Sections[i].BPM)
                    + currentPosition
                    + (source.Sections[i].Delay * 1000));
                currentBpm = source.Sections[i].BPM;
                currentPosition = section.Position + source.Sections[i].Length;

                section.Branches[1].Speed = 1;
                section.Branches[2].Speed = 1;

                section.Branches[0].Speed = System.Convert.ToSingle(source.Sections[i].Scroll);

                for (int j = 0; j < source.Sections[i].Notes.Count; j++)
                {
                    if (source.Sections[i].Notes[j].Type == Tja.NoteType.DurationEnd
                        || source.Sections[i].Notes[j].Type == Tja.NoteType.Blank)
                    {
                        continue;
                    }

                    var note = new Gen3.Note
                    {
                        Score = System.Convert.ToUInt16(source.ScoreInit),
                        Diff = System.Convert.ToUInt16(source.ScoreDiff * 4),
                        Position = System.Convert.ToSingle(
                            System.Convert.ToDouble(source.Sections[i].Notes[j].Position)
                            / source.Sections[i].NotesCount
                            * source.Sections[i].Length),
                        Type = source.Sections[i].Notes[j].Type switch
                        {
                            Tja.NoteType.Red => 2,
                            Tja.NoteType.Blue => 5,
                            Tja.NoteType.BigRed => 7,
                            Tja.NoteType.BigBlue => 8,
                            Tja.NoteType.Yellow => 6,
                            Tja.NoteType.BigYellow => 9,
                            Tja.NoteType.Balloon => 10,
                            Tja.NoteType.SweetPotato => 10,
                            _ => throw new Exception("unknowed note type")
                        }
                    };

                    // calculate length
                    if(note is { Type: 6 or 9 or 10})
                    {
                        // escape senerio like '7777778' or '6666668' etc.
                        if (j > 0&& 
                            source.Sections[i].Notes[j].Type== source.Sections[i].Notes[j-1].Type)
                        {
                            continue;
                        }

                        // search the end of note till this section.
                        for (int k = j + 1; k < source.Sections[i].Notes.Count; k++)
                        {
                            if (source.Sections[i].Notes[k].Type == Tja.NoteType.DurationEnd)
                            {
                                note.Length = System.Convert.ToSingle(
                                    System.Convert.ToDouble(source.Sections[i].Notes[k].Position - source.Sections[i].Notes[j].Position)
                                    * (source.Sections[i].Length / source.Sections[i].NotesCount));
                                break;
                            }
                        }

                        if (note.Length == 0)
                        {
                            note.Length = System.Convert.ToSingle(
                                System.Convert.ToDouble(source.Sections[i].NotesCount - source.Sections[i].Notes[j].Position)
                                * (source.Sections[i].Length / source.Sections[i].NotesCount));

                            var reachEnd = false;
                            for (int k = i + 1; (k < source.Sections.Count) && !reachEnd; k++)
                            {
                                for (int m = 0; (m < source.Sections[k].Notes.Count) && !reachEnd; m++)
                                {
                                    if (source.Sections[k].Notes[m].Type == Tja.NoteType.DurationEnd)
                                    {
                                        note.Length += System.Convert.ToSingle(
                                            System.Convert.ToDouble(source.Sections[k].Notes[m].Position)
                                            / source.Sections[k].NotesCount
                                            * source.Sections[k].Length
                                            + source.Sections[k].Delay * 1000);
                                        reachEnd = true;
                                    }
                                }

                                if (!reachEnd)
                                {
                                    note.Length += System.Convert.ToSingle(
                                        source.Sections[k].Length + source.Sections[k].Delay * 1000);
                                }
                            }
                        }
                    }

                    if (note.Type == 10)
                    {
                        note.Score = System.Convert.ToUInt16(source.Sections[i].Notes[j].BalloonCount);
                        note.Diff = 0;
                    }

                    section.Branches[0].Notes.Add(note);
                }

                section.Branches[0].Count = System.Convert.ToUInt16(section.Branches[0].Notes.Count);
                destination.Sections.Add(section);
            }

            destination.Count = System.Convert.ToUInt32(destination.Sections.Count);
            return destination;
        }
    }
}
