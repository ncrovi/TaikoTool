using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaikoTool.Core.Model
{
    public enum HardwareType : uint
    {
        System10 = 0x01,
        System246 = 0x02,
        System357 = 0x03,
        AC16 = 0x04,

        NDS = 0x10,
        ThreeDS = 0x11,
        Wii = 0x12,
        WiiU = 0x13,
        Switch = 0x14,

        PSP = 0x20,
        PSV = 0x21,
        PS2 = 0x22,
        PS4 = 0x23,

        IOS = 0x30,

        PC = 0x40,
        Xbox = 0x41
    }
}
