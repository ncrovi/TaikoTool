using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaikoTool.Core.Model.Chart.Tja;
using Autofac;

namespace TaikoTool.Core.Service
{
    public class TjaService : ITjaService
    {
        public Encoding CurrentEncoding { get; set; }

        public TjaFile Deserialize(Stream stream)
        {
            using var reader = new StreamReader(stream, CurrentEncoding);

            var lines = ReadAllLines(reader).ToList();
            var fumenRanges = new List<Range> { new Range(0, 0) };
            var result = new TjaFile();

            // get fumen counts;
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].Contains("#End",StringComparison.OrdinalIgnoreCase))
                {
                    fumenRanges.Last().End = i;
                    fumenRanges.Add(new Range(i + 1, 0));
                }
            }

            // remove redudant range;
            fumenRanges.RemoveAt(fumenRanges.Count - 1);

            // remove fumen with invalidate course
            fumenRanges.RemoveAll(r => r.Start < 0);

            // read basic infos
            for (int i = 0;i<fumenRanges[0].End ; i++)
            {
                if (lines[i].StartsWith("#"))
                {
                    break;
                }

                var parameters = lines[i].Split(":");

                if (parameters.Length < 2) continue;

                var key = parameters[0];
                var value = parameters[1];

                switch (key.ToUpper())
                {
                    case "BPM":
                        result.InitBpm = double.Parse(value);
                        break;
                    case "OFFSET":
                        result.Offset = double.Parse(value);
                        break;
                    case "TITLE":
                        result.Title = value;
                        break;
                    case "SUBTITLE":
                        result.SubTitle = value;
                        break;
                }
            }

            // read fumens
            result.Fumens = new Fumen[fumenRanges.Count];

            for (int i = 0; i < result.Fumens.Length; i++)
            {
                var range = fumenRanges[i];
                result.Fumens[i] = GetFumen(
                    lines.Skip(range.Start)
                    .Take(range.End - range.Start + 1).ToList(),
                    result.InitBpm);
            }

            return result;
        }

        public byte[] Serialize(TjaFile tja)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// remove comments
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private IEnumerable<string> ReadAllLines(StreamReader reader)
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();

                // comment
                if (line.StartsWith("//"))
                {
                    continue;
                }

                var realLine = line.Split("//").First().Trim();

                if (string.IsNullOrEmpty(realLine)) continue;

                yield return realLine;
            }
        }

        private Fumen GetFumen(IList<string> lines,double initBpm)
        {
            var fumen = new Fumen();
            var sectionRange = new Range(0, lines.Count - 1);
            var balloons = new Queue<int>();

            // read basic info
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].StartsWith("#"))
                {
                    sectionRange.Start = i + 1;
                    break;
                }

                var parameters = lines[i].Split(":");

                if (parameters.Length < 2)
                {
                    continue;
                }

                var key = parameters[0];
                var value = parameters[1];

                switch (key.ToUpper())
                {
                    case "LEVEL":
                        fumen.Level = int.Parse(value);
                        break;
                    case "COURSE":
                        {
                            if (int.TryParse(value, out int course))
                            {
                                fumen.Course = course;
                            }
                            else
                            {
                                switch (value.Trim().ToUpper())
                                {
                                    case "EASY":
                                        fumen.Course = 0;
                                        break;
                                    case "NORMAL":
                                        fumen.Course = 1;
                                        break;
                                    case "HARD":
                                        fumen.Course = 2;
                                        break;
                                    case "ONI":
                                        fumen.Course = 3;
                                        break;
                                    case "URA":
                                    case "EDIT":
                                        fumen.Course = 4;
                                        break;
                                    default:
                                        fumen.Course = -1;
                                        break;
                                }
                            }
                        }
                        break;
                    case "SCOREINIT":
                        if (value.Contains(','))
                        {
                            var split = value.Split(',');
                            fumen.ScoreInit = int.Parse(split[0].Trim());
                            fumen.ScoreShin = int.Parse(split[1].Trim());
                        }
                        else
                        {
                            fumen.ScoreInit = int.Parse(value);
                        }
                        break;
                    case "SCOREDIFF":
                        fumen.ScoreDiff = int.Parse(value);
                        break;
                    case "BALLOON":
                        {
                            if (string.IsNullOrWhiteSpace(value))
                            {
                                balloons = new Queue<int>();
                            }
                            else
                            {
                                var items = value.Split(',')
                                    .Where(r => !string.IsNullOrWhiteSpace(r))
                                    .Select(r => int.Parse(r));
                                balloons = new Queue<int>(items);
                            }
                        }
                        break;
                }
            }

            // generate sections
            var sectionLines = new List<string>();

            var note = false;
            var newSection = true;
            var previouseNewSection = true;
            var currentBpm = initBpm;

            var currentTotalNotesCount = 0;
            var currentMeasure = "4/4";
            var currentMeasureBeats = 0D;

            var currentFever = false;
            var currentBarline = true;
            var currentScroll = 1D;

            for (int i = sectionRange.Start; i < sectionRange.End; i++)
            {
                bool ready = false;

                if (note)
                {
                    if (lines[i].StartsWith("#"))
                    {
                        ready = true;
                        previouseNewSection = newSection;
                        newSection = false;
                        i--;
                    }
                    else
                    {
                        sectionLines[sectionLines.Count - 1] += lines[i];

                        if (lines[i].EndsWith(','))
                        {
                            ready = true;
                            previouseNewSection = newSection;
                            newSection = true;
                        }
                    }
                }
                else
                {
                    if (lines[i].StartsWith("#"))
                    {
                        sectionLines.Add(lines[i]);
                    }
                    else
                    {
                        note = true;
                        sectionLines.Add(lines[i]);

                        if (lines[i].EndsWith(','))
                        {
                            ready = true;
                            previouseNewSection = newSection;
                            newSection = true;
                        }
                    }
                }

                if (!ready) continue;

                var section = new Section();

                // read command
                for (int j = 0; j < sectionLines.Count; j++)
                {
                    var line = sectionLines[j];

                    if (!line.StartsWith("#")) continue;

                    var parameters = line.Split(' ');

                    // none parameters command
                    if (parameters.Length < 2)
                    {
                        switch (parameters[0].ToUpper())
                        {
                            case "#BARLINEON":
                                currentBarline = true;
                                break;
                            case "#BARLINEOFF":
                                currentBarline = false;
                                break;
                            case "#GOGOSTART":
                                currentFever = true;
                                break;
                            case "#GOGOEND":
                                currentFever = false;
                                break;
                        }
                    }
                    else
                    {
                        var key = parameters[0];
                        var value = parameters[1];

                        switch (key.ToUpper())
                        {
                            case "#BPMCHANGE":
                                currentBpm = double.Parse(value);
                                break;
                            case "#MEASURE":
                                currentMeasure = value;
                                break;
                            case "#DELAY":
                                section.Delay = double.Parse(value);
                                break;
                            case "#SCROLL":
                                currentScroll = double.Parse(value);
                                break;
                        }
                    }
                }

                section.BPM = currentBpm;
                section.GoGoTime = currentFever;
                section.Barline = currentBarline && newSection;
                section.Scroll = currentScroll;

                // prepare note line.
                var noteLine = sectionLines.First(r => !r.StartsWith('#')).Trim(',');

                // calculate measure if necessary
                if (previouseNewSection)
                {
                    // calculate notes count.
                    currentTotalNotesCount = noteLine.Length;

                    if (!sectionLines.First(r => !r.StartsWith('#')).EndsWith(','))
                    {
                        for (int j = i + 1; j < sectionRange.End; j++)
                        {
                            if (lines[j].StartsWith('#')) continue;

                            if (lines[j].EndsWith(','))
                            {
                                currentTotalNotesCount += lines[j].TrimEnd(',').Length;
                                break;
                            }
                            else
                            {
                                currentTotalNotesCount += lines[j].Length;
                            }
                        }
                    }

                    var measureParams = currentMeasure.Split('/');
                    currentMeasureBeats = double.Parse(measureParams[0]) * 4 / double.Parse(measureParams[1]);
                }

                // generate note
                for (int j = 0; j < noteLine.Length; j++)
                {
                    var target = new Note
                    {
                        Position = j,
                        Type = (NoteType)int.Parse(noteLine[j].ToString())
                    };

                    if ((int)target.Type < 1 || (int)target.Type > 9) continue;

                    // dequeue balloon count if neccessary
                    if (target.Type == NoteType.Balloon)
                    {
                        // incase of content like '77778' will
                        // dequeue multiple balloons.
                        if (section.Notes.Count > 0)
                        {
                            if (section.Notes.Last().Type == target.Type)
                            {
                                target.BalloonCount = section.Notes.Last().BalloonCount;
                            }
                            else
                            {
                                target.BalloonCount = balloons.Dequeue();
                            }
                        }
                        else
                        {
                            target.BalloonCount = balloons.Dequeue();
                        }
                    }

                    section.Notes.Add(target);
                }

                section.NotesCount = noteLine.Length;
                if (currentTotalNotesCount == 0)
                {
                    section.MeasureBeats = currentMeasureBeats;
                    section.Length = currentMeasureBeats * (60D / currentBpm) * 1000;
                }
                else
                {
                    section.MeasureBeats = 1D * noteLine.Length / currentTotalNotesCount * currentMeasureBeats;
                    section.Length = section.MeasureBeats * (60D / currentBpm) * 1000;
                }

                fumen.Sections.Add(section);

                // finally set status
                sectionLines.Clear();
                ready = false;
                note = false;
            }

            return fumen;
        }

        private class Range
        {
            public int Start { get; set; }

            public int End { get; set; }

            public Range(int start, int end)
            {
                Start = start;
                End = end;
            }
        }
    }
}
