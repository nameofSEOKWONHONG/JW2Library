﻿using System;

namespace JWLibrary.Core {
    public static class JDateHelper {
        public static DateTime toDate(this string date) {
            var datetime = DateTime.MinValue;
            DateTime.TryParse(date, out datetime);
            return datetime;
        }

        public static string toDate(this DateTime date, ENUM_DATE_FORMAT format = null ) {
            if (format.isNotNull()) date.ToString(format.Value);
            return date.ToString(ENUM_DATE_FORMAT.DEFAULT.Value);
        }

        public static string toDate(this DateTime date, string format = null) {
            if (format.isNullOrEmpty()) format = "yyyy-MM-dd";
            return date.ToString(format);
        }
    }

    public class ENUM_DATE_FORMAT : JENUM_BASE<ENUM_DATE_FORMAT> {
        public static readonly ENUM_DATE_FORMAT DEFAULT = define("yyyy-MM-dd");
        public static readonly ENUM_DATE_FORMAT YYYY_MM_DD = define("yyyy-MM-dd");
        public static readonly ENUM_DATE_FORMAT YYYY_MM_DD_HH_MM_SS = define("yyyy-MM-dd HH:mm:ss");
        public static readonly ENUM_DATE_FORMAT YYYYMMDD = define("yyyyMMdd");
        public static readonly ENUM_DATE_FORMAT YYYY_FS_MM_FS_DD = define("yyyy/MM/dd");
        public static readonly ENUM_DATE_FORMAT YYYYMMDDHHMMSS = define("yyyyMMddHHmmss");
        public static readonly ENUM_DATE_FORMAT HHMMSS = define("HHmmss");
    }
}