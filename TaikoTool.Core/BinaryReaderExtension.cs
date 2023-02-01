using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaikoTool.Core.Model;

namespace TaikoTool.Core
{
    public static class BinaryIOExtension
    {
        #region reader
        public static float ReadSingle(this BinaryReader reader, EndianType endian)
        {
            if (NeedRerverse(endian))
            {
                var buffer = reader.ReadBytes(4);
                Array.Reverse(buffer);
                return BitConverter.ToSingle(buffer);
            }
            else
            {
                return reader.ReadSingle();
            }
        }

        public static int ReadInt32(this BinaryReader reader, EndianType endian)
        {
            if (NeedRerverse(endian))
            {
                var buffer = reader.ReadBytes(4);
                Array.Reverse(buffer);
                return BitConverter.ToInt32(buffer);
            }
            else
            {
                return reader.ReadInt32();
            }
        }

        public static uint ReadUInt32(this BinaryReader reader, EndianType endian)
        {
            if (NeedRerverse(endian))
            {
                var buffer = reader.ReadBytes(4);
                Array.Reverse(buffer);
                return BitConverter.ToUInt32(buffer);
            }
            else
            {
                return reader.ReadUInt32();
            }
        }

        public static ushort ReadUInt16(this BinaryReader reader, EndianType endian)
        {
            if (NeedRerverse(endian))
            {
                var buffer = reader.ReadBytes(2);
                Array.Reverse(buffer);
                return BitConverter.ToUInt16(buffer);
            }
            else
            {
                return reader.ReadUInt16();
            }
        }
        #endregion

        #region writer
        public static void Write(this BinaryWriter writer, float value, EndianType endian)
        {
            if (NeedRerverse(endian))
            {
                var buffer = BitConverter.GetBytes(value);
                Array.Reverse(buffer);
                writer.Write(buffer);
            }
            else
            {
                writer.Write(value);
            }
        }

        public static void Write(this BinaryWriter writer, int value, EndianType endian)
        {
            if (NeedRerverse(endian))
            {
                var buffer = BitConverter.GetBytes(value);
                Array.Reverse(buffer);
                writer.Write(buffer);
            }
            else
            {
                writer.Write(value);
            }
        }

        public static void Write(this BinaryWriter writer, uint value, EndianType endian)
        {
            if (NeedRerverse(endian))
            {
                var buffer = BitConverter.GetBytes(value);
                Array.Reverse(buffer);
                writer.Write(buffer);
            }
            else
            {
                writer.Write(value);
            }
        }

        public static void Write(this BinaryWriter writer, ushort value, EndianType endian)
        {
            if (NeedRerverse(endian))
            {
                var buffer = BitConverter.GetBytes(value);
                Array.Reverse(buffer);
                writer.Write(buffer);
            }
            else
            {
                writer.Write(value);
            }
        }
        #endregion

        private static bool NeedRerverse(EndianType endian)
        {
            return BitConverter.IsLittleEndian ? endian != EndianType.Little : endian != EndianType.Big;
        }
    }
}
