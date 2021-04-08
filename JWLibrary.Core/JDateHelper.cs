using System;

namespace JWLibrary.Core {
    public static class JDateHelper {
        public static DateTime jToDate(this string date) {
            var datetime = DateTime.MinValue;
            DateTime.TryParse(date, out datetime);
            return datetime;
        }

        public static string jToDate(this DateTime date, ConvertFormat format = ConvertFormat.Default) {
            return date.ToString(format.jEnumToString());
        }

        public static string jToDate(this DateTime date, string format = null) {
            if (format.jIsNullOrEmpty()) format = "yyyy-MM-dd";
            return date.ToString(format);
        }
    }

    public enum ConvertFormat {
        [StringValue("yyyy-MM-dd")] Default,
        [StringValue("yyyy-MM-dd")] yyyy_MM_dd,
        [StringValue("yyyy-MM-dd HH:mm:ss")] yyyy_MM_dd_S_HH_C_mm_C_ss,
        [StringValue("yyyyMMdd")] yyyyMMdd,
        [StringValue("yyyy/MM/dd")] yyyy_FS_MM_FS_dd,
        [StringValue("yyyyMMddHHmmss")] yyyyMMddHHmmss,
        [StringValue("HHmmss")] HHmmss
    }
}