﻿using System;
using System.ComponentModel;

namespace JWLibrary.Core {
    public static class JEnum {
        public static T ToEnum<T>(this string value, T defaultValue) where T : struct {
            if (value.jIsNullOrEmpty()) return defaultValue;

            return Enum.TryParse(value, true, out T result) ? result : defaultValue;
        }

        public static string jToString(this Enum value) {
            var da = (DescriptionAttribute[]) value.GetType().GetField(value.ToString())
                ?.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return da != null && da.Length > 0 ? da[0].Description : value.ToString();
        }
    }
}