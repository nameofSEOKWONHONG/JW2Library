using System.IO;
using System.IO.Compression;
using System.Text;

namespace JWLibrary.Core {
    public static class JString {
        public static int jToCount(this string str) {
            return str.jIsNull() ? 0 : str.Length;
        }

        public static bool jIsNullOrEmpty(this string str) {
            return string.IsNullOrEmpty(str);
        }

        public static string jReplace(this string text, string oldValue, string newValue) {
            return text.jIfNullOrEmpty(x => string.Empty).Replace(oldValue, newValue);
        }

        private static void jCopyTo(Stream src, Stream dest) {
            var bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0) dest.Write(bytes, 0, cnt);
        }

        public static byte[] jStringZip(this string str) {
            var bytes = Encoding.UTF8.GetBytes(str);

            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream()) {
                using (var gs = new GZipStream(mso, CompressionMode.Compress)) {
                    //msi.CopyTo(gs);
                    jCopyTo(msi, gs);
                }

                return mso.ToArray();
            }
        }

        public static string jStringUnzip(this byte[] bytes) {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream()) {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress)) {
                    //gs.CopyTo(mso);
                    jCopyTo(gs, mso);
                }

                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }

        public static string jJoin(this string[] value, string separator) {
            return string.Join(separator, value);
        }

        public static int jIndexOf(this string src, string value) {
            if (value.jIsNullOrEmpty()) return -1;
            return src.IndexOf(value);
        }

        public static int jIndexOfAny(this string src, string value) {
            if (value.jIsNullOrEmpty()) return -1;
            return src.IndexOfAny(value.ToCharArray());
        }

        public static int jLastIndexOf(this string src, string value) {
            if (value.jIsNullOrEmpty()) return -1;
            return src.LastIndexOf(value);
        }

        public static int jLastIndexOfAny(this string src, string value) {
            if (value.jIsNullOrEmpty()) return -1;
            return src.LastIndexOfAny(value.ToCharArray());
        }
    }
}