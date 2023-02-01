using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaikoTool.Core.Model
{
    public enum GameVersion : uint
    {
        Unknowned = 0,

        AC15_MUIIN = (HardwareType.System357 << 0x10) | 0x01,
        AC15_KATSUDON = (HardwareType.System357 << 0x10) | 0x02,
        AC15_SORAIRO = (HardwareType.System357 << 0x10) | 0x03,
        AC15_MOMOIRO = (HardwareType.System357 << 0x10) | 0x04,
        AC15_KIMIDORI = (HardwareType.System357 << 0x10) | 0x05,
        AC15_MURASAKI = (HardwareType.System357 << 0x10) | 0x06,
        AC15_HOWAITO = (HardwareType.System357 << 0x10) | 0x07,
        AC15_REEDO = (HardwareType.System357 << 0x10) | 0x08,
        AC15_YERO = (HardwareType.System357 << 0x10) | 0x09,
        AC15_BURU = (HardwareType.System357 << 0x10) | 0x10,
        AC15_GURIN = (HardwareType.System357 << 0x10) | 0x0A,

        AC16_2020 = (HardwareType.AC16 << 0x10) | 0x01,
        AC16_2021 = (HardwareType.AC16 << 0x10) | 0x02,

        Switch = (HardwareType.Switch << 0x10) | 0x01,
        Switch2 = (HardwareType.Switch << 0x10) | 0x02,
        SwitchRpg = (HardwareType.Switch << 0x10) | 0x10,

        PS4 = (HardwareType.PS4 << 0x10) | 0x01,

        PSV = (HardwareType.PSV << 0x10) | 0x01,
        PSVIdolMasterRed = (HardwareType.PSV << 0x10) | 0x02,
        PSVIdolMasterBlue = (HardwareType.PSV << 0x10) | 0x03,

        ThreeDS1 = (HardwareType.ThreeDS<<0x10) | 0x01,
        ThreeDS2 = (HardwareType.ThreeDS<<0x10) | 0x02,
        ThreeDS3 = (HardwareType.ThreeDS<<0x10) | 0x03,

        IOS = (HardwareType.IOS << 0x10) | 0x01,
        IOSOnline = (HardwareType.IOS << 0x10) | 0x02,

        IOSArcade = (HardwareType.IOS << 0x10) | 0x10,

        PSP1 = (HardwareType.PSP << 0x10) | 0x01,
        PSP2 = (HardwareType.PSP << 0x10) | 0x02,
        PSPDX = (HardwareType.PSP << 0x10) | 0x03,

        WiiU1 = (HardwareType.WiiU << 0x10) | 0x01,
        WiiU2 = (HardwareType.WiiU << 0x10) | 0x02,
        WiiU3 = (HardwareType.WiiU << 0x10) | 0x03,

        Jiro = (HardwareType.PC << 0x10) | 0x01,
        Tjap3 = (HardwareType.PC << 0x10) | 0x02,
        Tcc = (HardwareType.PC << 0x10) | 0x03,
        OpenTaiko = (HardwareType.PC << 0x10) | 0x04,
    }
}
