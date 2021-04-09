using System;
using System.Globalization;
using System.Text;
using Microsoft.Extensions.ObjectPool;

namespace JWLibrary.Core {
    /// <summary>
    /// StringBuilder Pool
    /// 메모리 소비를 늘리는 대신 GC를 줄입니다.
    /// </summary>
    public class JStringBuilder : IDisposable {
        private ObjectPool<StringBuilder> _stringBuilderPool = null;
        private StringBuilder _stringBuilder = null;
        
        public JStringBuilder() {
            var objectPoolProvider = new DefaultObjectPoolProvider();
            _stringBuilderPool = objectPoolProvider.CreateStringBuilderPool();
            _stringBuilder = _stringBuilderPool.Get();
        }

        public JStringBuilder(int capacity) {
            var objectPoolProvider = new DefaultObjectPoolProvider();
            _stringBuilderPool = objectPoolProvider.CreateStringBuilderPool(capacity, capacity * 2);
            _stringBuilder = _stringBuilderPool.Get();
        }

        public JStringBuilder(int initCapacity, int maxCapacity) {
            var objectPoolProvider = new DefaultObjectPoolProvider();
            _stringBuilderPool = objectPoolProvider.CreateStringBuilderPool(initCapacity, maxCapacity);
            _stringBuilder = _stringBuilderPool.Get();
        }

        public void append(string str) {
            _stringBuilder.Append(str);
        }

        public void append(char c) {
            _stringBuilder.Append(c);
        }

        public void appendLine(string str) {
            _stringBuilder.AppendLine(str);
        }

        public void appendFormat(CultureInfo cultureInfo, string format, params object[] objs) {
            _stringBuilder.AppendFormat(cultureInfo, format, objs);
        }

        public void appendJoin(string seperator, params object[] objs) {
            _stringBuilder.AppendJoin(seperator, objs);
        }

        public override string ToString() {
            return _stringBuilder.ToString();
        }

        public void Dispose() {
            release();
        }

        public void release() {
            _stringBuilderPool.Return(_stringBuilder);
            _stringBuilderPool = null;
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}