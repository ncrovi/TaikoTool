namespace TaikoTool.Core.Model.Chart.Gen3
{
    public class Note
    {
        public uint Type { get; set; }

        public float Position { get; set; }

        public int Unk1 { get; set; }

        public int Unk2 { get; set; }

        public ushort Score { get; set; }

        public ushort Diff { get; set; }

        public float Length { get; set; }
    }
}
