using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using NetFabric.Hyperlinq;

namespace JWLibrary.Core {
    ref struct SplitSpanEnumerator {
        private ReadOnlySpan<char> text;
        private readonly char splitChar;
        
        public ReadOnlySpan<char> Current { get; private set; }
        
        public SplitSpanEnumerator(ReadOnlySpan<char> text, char splitChar) {
            this.text = text;
            this.splitChar = splitChar;
            this.Current = default;
        }

        public SplitSpanEnumerator GetEnumerator() => this;

        public bool MoveNext() {
            var index = text.IndexOf(splitChar);
            if (index == -1)
                return false;

            Current = text[..index];
            text = text[(index + 1)..];

            return true;
        }
    }
    
    public static class JString {
        /*
         * Span변환의 장점 : 스택에 메모리 할당되므로 GC가 발생하지 않도록 해줌.
         * Memory<T> (T:Class) -> Span<T> (T:Struct)로 변환하여 사용 가능
         * new 대신 stackalloc 사용할 경우 GC 압력이 줄어듬.(struct type에 대하여, int, char, byte 등등)
         * windows stack 최대 할당 용량은 1MB
         */
        public static string substring(this string str, int startIndex, int length = 0) {
            if (length > 0) {
                return str.AsSpan()[startIndex..(startIndex + length)].ToString();
            }

            return str.AsSpan()[startIndex..str.Length].ToString();
        }

        public static IEnumerable<string> split(this string str, char splitChar) {
            JList<string> result = new JList<string>();
            foreach (var @char in new SplitSpanEnumerator(str.AsSpan(), splitChar)) {
                result.Add(@char.ToString());
            }

            return result;
        }

        public static int toCount(this string str) {
            return str.isNull() ? 0 : str.Length;
        }

        public static bool isNullOrEmpty(this string str) {
            return string.IsNullOrEmpty(str);
        }

        public static string replace(this string text, string oldValue, string newValue) {
            return text.ifNullOrEmpty(x => string.Empty).Replace(oldValue, newValue);
        }

        private static void copyTo(Stream src, Stream dest) {
            var bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0) dest.Write(bytes, 0, cnt);
        }

        public static byte[] toByteArray(this string str) {
            var bytes = Encoding.UTF8.GetBytes(str);

            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream()) {
                using (var gs = new GZipStream(mso, CompressionMode.Compress)) {
                    //msi.CopyTo(gs);
                    copyTo(msi, gs);
                }

                return mso.ToArray();
            }
        }

        public static string toString(this byte[] bytes) {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream()) {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress)) {
                    //gs.CopyTo(mso);
                    copyTo(gs, mso);
                }

                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }

        public static string @join(this string[] value, string separator) {
            return string.Join(separator, value);
        }

        public static int indexOf(this string src, string value) {
            if (value.isNullOrEmpty()) return -1;
            return src.IndexOf(value);
        }

        public static int indexOfAny(this string src, string value) {
            if (value.isNullOrEmpty()) return -1;
            return src.IndexOfAny(value.ToCharArray());
        }

        public static int lastIndexOf(this string src, string value) {
            if (value.isNullOrEmpty()) return -1;
            return src.LastIndexOf(value);
        }

        public static int lastIndexOfAny(this string src, string value) {
            if (value.isNullOrEmpty()) return -1;
            return src.LastIndexOfAny(value.ToCharArray());
        }
    }
}