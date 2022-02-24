using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Test2
{
    /// <summary>
    /// Class that implements single- and multi- threaded versions of checksum function
    /// </summary>
    public static class CheckSum
    {
        private static void PathCorrect(string path)
       {
           if (!Directory.Exists(path) && !File.Exists(path))
           {
               throw new ArgumentException("There is no fle or directory there");
           }
       }

        /// <summary>
        /// single-threaded version
        /// </summary>
        public static byte[] CheckSumSimple(string path)
        {
            PathCorrect(path);
            using var mdHash = MD5.Create();
            if (Directory.Exists(path))
            {
                var filesInDir = Directory.GetFiles(path).OrderBy(x => x).ToArray();
                var sum = Encoding.ASCII.GetBytes(Path.GetFileName(Path.GetDirectoryName(path)) ?? throw new InvalidOperationException());
                
                foreach (var file in filesInDir)
                {
                    sum = sum.Concat(CheckSumSimple(file)).ToArray();
                }
                
                return sum;
            }
            var readStream = File.OpenRead(path);
            return mdHash.ComputeHash(readStream);
        }

        /// <summary>
        /// multi-threaded version
        /// </summary>
        public static async Task<byte[]> CheckSumMultiThreaded(string path)
        {
            PathCorrect(path);
            using var mdHash = MD5.Create();
            if (Directory.Exists(path))
            {
                var filesInDir = Directory.GetFiles(path).OrderBy(x => x).ToArray();
                var sum = Encoding.ASCII.GetBytes(Path.GetFileName(Path.GetDirectoryName(path)) ?? throw new InvalidOperationException());
                
                foreach (var file in filesInDir)
                {
                    sum = sum.Concat(await CheckSumMultiThreaded(file)).ToArray();
                }
                
                return sum;
            }
            var readStream = File.OpenRead(path);
            return await mdHash.ComputeHashAsync(readStream);
        }

    }
}