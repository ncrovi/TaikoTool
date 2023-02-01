using Autofac;
using AutoMapper;
using CommandLine;

namespace TaikoTool.Console
{
    internal class Program
    {
        static int Main(string[] args)
        {
            // set current directory
            Directory.SetCurrentDirectory(AppContext.BaseDirectory);

            var builder = new ContainerBuilder();

            var currentAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            var coreAssembly = System.Reflection.Assembly.GetAssembly(typeof(Core.Tools));

            if (coreAssembly is null) throw new Exception("missing core assembly");

            builder.RegisterAssemblyModules(currentAssembly, coreAssembly);
            builder.RegisterInstance(new MapperConfiguration(
                r => r.AddProfile<Core.Profile.Fumen.TjaProfile>()).CreateMapper());

            using var container = builder.Build();

            return Parser.Default.ParseArguments<ConvertOption>(args)
                .MapResult(
                (ConvertOption option) => Convert(option,container),
                errors => 1);
        }

        private static int Convert(ConvertOption option,ILifetimeScope container)
        {
            var mapper = container.Resolve<IMapper>();

            foreach (var file in option.InputFiles)
            {
                using var tjaStream = File.OpenRead(file);

                var tjaService = container.Resolve<Core.ITjaService>();
                tjaService.CurrentEncoding = System.Text.Encoding.GetEncoding(option.InputEncoding);
                var tja = tjaService.Deserialize(tjaStream);

                foreach (var fumen in tja.Fumens)
                {
                    var suffix = fumen.Course switch
                    {
                        0 => "e",
                        1 => "n",
                        2 => "h",
                        3 => "m",
                        4 => "x",
                        _ => string.Empty
                    };

                    if (suffix == string.Empty) continue;

                    var gen3 = mapper.Map<Core.Model.Chart.Gen3.Fumen>(fumen);

                    var offset = -tja.Offset - (60D / tja.InitBpm * 4);
                    foreach (var section in gen3.Sections)
                    {
                        section.Position += System.Convert.ToSingle(offset * 1000);
                    }

                    var workspace = Path.Combine(AppContext.BaseDirectory, "Fumens");
                    if (!string.IsNullOrEmpty(option.OutputDirectory)) workspace = option.OutputDirectory;

                    if(!Directory.Exists(workspace)) Directory.CreateDirectory(workspace);

                    var fileName = Path.GetFileNameWithoutExtension(file);
                    if (!string.IsNullOrEmpty(option.OutputFileName)) fileName = option.OutputFileName;

                    using var fumenStream = File.Open(
                        Path.Combine(workspace, $"{fileName}_{suffix}.bin"), FileMode.Create, FileAccess.Write);
                    var service = container.ResolveKeyed<Core.IFumenService>(Core.Model.GameVersion.Switch);
                    service.CurrentEndian = Core.Model.EndianType.Little;
                    service.CurrentCourse = fumen.Course switch
                    {
                        0 => Core.Model.CourseType.Easy,
                        1 => Core.Model.CourseType.Normal,
                        2 => Core.Model.CourseType.Hard,
                        3 => Core.Model.CourseType.Mania,
                        4 => Core.Model.CourseType.Extrem,
                        _ => throw new Exception("unknowed course")
                    };

                    service.Write(gen3, fumenStream);
                }
            }

            return 0;
        }
    }

    [Verb("convert")]
    internal class ConvertOption
    {
        #region output options
        [Option('T', "output-chart", Required = false)]
        public Core.Model.ChartType OutputChart { get; set; } = Core.Model.ChartType.Gen3;

        [Option('C', "output-encoding", Required = false)]
        public string OutputEncoding { get; set; } = "UTF-8";

        [Option('E', "output-endian", Required = false)]
        public Core.Model.EndianType OutputEndian { get; set; } = Core.Model.EndianType.Little;

        [Option('D', "output-directory", Required = false)]
        public string? OutputDirectory { get; set; }

        [Option('N', "output-name", Required = false)]
        public string? OutputFileName { get; set; }
        #endregion

        #region input options
        [Option('t', "input-chart", Required = false)]
        public Core.Model.ChartType InputChart { get; set; } = Core.Model.ChartType.TJA;

        [Option('c', "input-encoding", Required = false)]
        public string InputEncoding { get; set; } = "UTF-8";

        [Option('e', "input-endian", Required = false)]
        public Core.Model.EndianType InputEndian { get; set; } = Core.Model.EndianType.Little;

        [Value(0, Required = true)]
        public IEnumerable<string> InputFiles { get; set; } = new List<string>();
        #endregion
    }
}