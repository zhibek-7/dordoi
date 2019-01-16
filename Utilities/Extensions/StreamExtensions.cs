using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Utilities.Extensions
{
    public static class StreamExtensions
    {

        public static async ValueTask WriteAsync(this Stream stream, string stringToWrite)
        {
            await stream.WriteAsync(stringToWrite.Select(x => (byte)x).ToArray());
        }

    }
}
