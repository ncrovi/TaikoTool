using System.IO;
using TaikoTool.Core.Model;
using TaikoTool.Core.Model.Chart.Gen3;

namespace TaikoTool.Core
{
    public interface IFumenService
    {
        EndianType CurrentEndian { get; set; }

        CourseType CurrentCourse { get; set; }

        void Write(Fumen fumen, Stream stream);

        Fumen Read(Stream stream);
    }
}
