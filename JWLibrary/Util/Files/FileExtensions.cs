using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using JWLibrary.Core;

namespace JWLibrary.Utils.Files {
    public static class FileExtensions {
        public static string[] jFileReadLines(this string fileName) {
            if (!File.Exists(fileName)) throw new Exception($"not exists {fileName}");
            return File.ReadAllLines(fileName);
        }

        public static async Task<string[]> jFileReadLineAsync(this string fileName) {
            if (!File.Exists(fileName)) throw new Exception($"not exists {fileName}");
            return await File.ReadAllLinesAsync(fileName);
        }

        public static byte[] jFileReadBytes(this string fileName) {
            if (!File.Exists(fileName)) throw new Exception($"not exists {fileName}");
            return File.ReadAllBytes(fileName);
        }

        public static async Task<byte[]> jFileReadBytesAsync(this string fileName) {
            if (!File.Exists(fileName)) throw new Exception($"not exists {fileName}");
            return await File.ReadAllBytesAsync(fileName);
        }

        public static void jFileWriteAllLines(this string fileName, string[] lines, Encoding encoding = null) {
            if (encoding.isNotNull())
                File.WriteAllLines(fileName, lines, encoding);

            File.WriteAllLines(fileName, lines);
        }

        public static async Task
            jFileWriteAllLinesAsync(this string fileName, string[] lines, Encoding encoding = null) {
            if (encoding.isNotNull())
                await File.WriteAllLinesAsync(fileName, lines, encoding);

            await File.WriteAllLinesAsync(fileName, lines);
        }

        public static void jFileWriteBytes(this string fileName, byte[] bytes) {
            File.WriteAllBytes(fileName, bytes);
        }

        public static async Task jFileWriteBytesAsync(this string fileName, byte[] bytes) {
            await File.WriteAllBytesAsync(fileName, bytes);
        }

        public static bool jFileExists(this string fileName) {
            return File.Exists(fileName);
        }

        public static string jFileUniqueId(this string fileName) {
            var ret = string.Empty;
            if (!File.Exists(fileName)) throw new Exception($"not exists {fileName}");
            using (var md5 = MD5.Create()) {
                using (var stream = File.OpenRead(fileName)) {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                }
            }
        }

        public static bool jIsFileExtension(this string fileName,
            string pattern = @"^.*\.(zip|ZIP|jpg|JPG|gif|GIF|doc|DOC|pdf|PDF)$") {
            var match = Regex.Match(fileName, pattern);
            return match.Success;
        }

        public static void jFileZip(this string srcDir, string destZipFileName,
            CompressionLevel compressionLevel = CompressionLevel.Fastest) {
            ZipFile.CreateFromDirectory(srcDir, destZipFileName, compressionLevel, false);
        }

        public static void jFileUnzip(this string srcFileName, string destdir) {
            if (srcFileName.jIsFileExtension()) ZipFile.ExtractToDirectory(srcFileName, destdir, null, true);
        }
    }
}