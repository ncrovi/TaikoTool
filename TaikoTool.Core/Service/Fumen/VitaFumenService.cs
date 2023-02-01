using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Configuration;
using TaikoTool.Core.Model;
using TaikoTool.Core.Model.Chart.Gen3;

namespace TaikoTool.Core.Service
{
    public class VitaFumenService : FumenService
    {
        private readonly IConfiguration config;
        public override EndianType CurrentEndian => EndianType.Little;

        public VitaFumenService(IConfiguration config)
        {
            this.config = config.GetSection(nameof(Tools));
        }

        public override Fumen Read(Stream stream)
        {
            try
            {
                CleanFile();

                using (var compressedStream = File.OpenWrite(Path.Combine(config.GetSection(nameof(Tools.VitaTool))["Path"], "tempIn.bin")))
                {
                    stream.CopyTo(compressedStream);
                }

                using var process = Process.Start(new ProcessStartInfo
                {
                    FileName = Path.Combine(config.GetSection(nameof(Tools.VitaTool))["Path"], "psvita-l7ctool.exe"),
                    Arguments = $"d tempIn.bin tempOut.bin",
                    WorkingDirectory = config.GetSection(nameof(Tools.VitaTool))["Path"]
                });

                process.WaitForExit();

                using var decompressedStream = File.OpenRead(Path.Combine(config.GetSection(nameof(Tools.VitaTool))["Path"], "tempOut.bin"));

                return base.Read(decompressedStream);
            }
            finally
            {
                CleanFile();
            }

            void CleanFile()
            {
                if (File.Exists("tempIn.bin"))
                {
                    File.Delete("tempIn.bin");
                }
                if (File.Exists("tempOut.bin"))
                {
                    File.Delete("tempOut.bin");
                }
            }
        }
    }
}
