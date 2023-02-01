using Autofac;
using Autofac.Core;
using System.Collections.Generic;
using TaikoTool.Core.Service;
using TaikoTool.Core.Model;

namespace TaikoTool.Core
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SwitchFumenService>()
                .Keyed<IFumenService>(GameVersion.Switch)
                .Keyed<IFumenService>(GameVersion.PS4)
                .Keyed<IFumenService>(GameVersion.AC16_2020)
                .SingleInstance();

            builder.RegisterType<VitaFumenService>()
                .Keyed<IFumenService>(GameVersion.PSV)
                .SingleInstance();

            builder.RegisterType<IosFumenService>()
                .Keyed<IFumenService>(GameVersion.IOS)
                .SingleInstance();

            builder.RegisterType<FumenService>()
                .Keyed<IFumenService>(GameVersion.AC15_GURIN)
                .WithProperties(
                    new List<Parameter> {
                        new NamedParameter(nameof(FumenService.CurrentEndian), EndianType.Big)
                    })
                .SingleInstance();

            builder.RegisterType<TjaService>().As<ITjaService>();
        }
    }
}
