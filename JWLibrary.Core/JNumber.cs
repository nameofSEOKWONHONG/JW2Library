using System;
using System.Text.RegularExpressions;

namespace JWLibrary.Core {
    public static class JNumber {
        public static string jToNumber<T>(this T val, ENUM_NUMBER_FORMAT_TYPE type, ENUM_GET_ALLOW_TYPE allow) {
            if (val.GetType() == typeof(DateTime)) throw new NotSupportedException("DateTime is not support.");
            if (val.GetType() == typeof(float)) throw new NotSupportedException("float is not support.");

            var result = type switch {
                ENUM_NUMBER_FORMAT_TYPE.Comma => string.Format("{0:#,###}", val),
                ENUM_NUMBER_FORMAT_TYPE.Rate => string.Format("{0:##.##}", val),
                ENUM_NUMBER_FORMAT_TYPE.Mobile => allow switch {
                    ENUM_GET_ALLOW_TYPE.Allow => string.Format("{0}-{1}-{2}", val.ToString().getFirst(3),
                        val.ToString().getMiddle(3, 4),
                        val.ToString().getLast(4)),
                    _ => string.Format("{0}-{1}-****", val.ToString().getFirst(3),
                        val.ToString().getMiddle(3, 4))
                },
                ENUM_NUMBER_FORMAT_TYPE.Phone => MakePhoneString(val, allow),
                ENUM_NUMBER_FORMAT_TYPE.RRN => MakeRRNString(val, allow),
                ENUM_NUMBER_FORMAT_TYPE.CofficePrice => string.Format("{0}.{1}", val.ToString().getFirst(1),
                    val.ToString().getMiddle(1, 1)),
                _ => throw new NotSupportedException("do not convert value")
            };

            return result;
        }

        private static string MakePhoneString<T>(T val, ENUM_GET_ALLOW_TYPE allow) {
            if (allow == ENUM_GET_ALLOW_TYPE.Allow) {
                var temp = val.ToString();
                if (temp.getFirst(2) == "02") {
                    if (temp.Length == 10)
                        return string.Format("{0}-{1}-{2}", temp.getFirst(2),
                            temp.getMiddle(2, 4),
                            temp.getLast(4));
                    return string.Format("{0}-{1}-{2}", temp.getFirst(2),
                        temp.getMiddle(2, 3),
                        temp.getLast(4));
                }

                if (temp.Length == 11)
                    return string.Format("{0}-{1}-{2}", temp.getFirst(3),
                        temp.getMiddle(3, 4),
                        temp.getLast(4));
                return string.Format("{0}-{1}-{2}", temp.getFirst(3),
                    temp.getMiddle(3, 3),
                    temp.getLast(4));
            }
            else {
                var temp = val.ToString();
                if (temp.getFirst(2) == "02") {
                    if (temp.Length == 10)
                        return string.Format("{0}-{1}-****", temp.getFirst(2),
                            temp.getMiddle(2, 4));
                    return string.Format("{0}-{1}-****", temp.getFirst(2),
                        temp.getMiddle(2, 3));
                }

                if (temp.Length == 11)
                    return string.Format("{0}-{1}-****", temp.getFirst(3),
                        temp.getMiddle(3, 4));
                return string.Format("{0}-{1}-****", temp.getFirst(3),
                    temp.getMiddle(3, 3));
            }
        }

        private static string MakeRRNString<T>(T val, ENUM_GET_ALLOW_TYPE allow) {
            if (allow == ENUM_GET_ALLOW_TYPE.Allow)
                return string.Format("{0}-{1}", val.ToString().getFirst(6),
                    val.ToString().getLast(7));
            return string.Format("{0}-*******", val.ToString().getFirst(6));
        }

        public static string getMiddle(this string value, int fromLen, int getLen) {
            return value.Substring(fromLen, getLen);
        }

        public static string getFirst(this string value, int length) {
            return value.Substring(0, length);
        }

        public static string getLast(this string value, int length) {
            return value.Substring(value.Length - length, length);
        }

        public static bool isNumber(this string str) {
            str = str.jIfNullOrEmpty(x => string.Empty);
            var regex = new Regex("^[0-9]*$", RegexOptions.ExplicitCapture | RegexOptions.Compiled);
            return regex.Match(str).Success;
        }

        public static bool isAlphabet(this string str) {
            str = str.jIfNullOrEmpty(x => string.Empty);
            var regex = new Regex(@"^[a-zA-Z\-_]+$", RegexOptions.ExplicitCapture | RegexOptions.Compiled);
            return regex.Match(str).Success;
        }

        public static bool isAlphabetAndNumber(this string str) {
            str = str.jIfNullOrEmpty(x => string.Empty);
            var regex = new Regex(@"^[a-zA-Z0-9]+$", RegexOptions.ExplicitCapture | RegexOptions.Compiled);
            return regex.Match(str).Success;
        }

        public static bool isNumeric(this string str) {
            str = str.jIfNullOrEmpty(x => string.Empty);
            var regex = new Regex(@"^(?<digit>-?\d+)(\.(?<scale>\d*))?$",
                RegexOptions.ExplicitCapture | RegexOptions.Compiled);
            return regex.Match(str).Success;
        }
    }

    public enum ENUM_GET_ALLOW_TYPE {
        Allow,
        NotAllow
    }

    public enum ENUM_NUMBER_FORMAT_TYPE {
        Comma,
        Rate,
        Mobile,
        RRN,
        CofficePrice,
        Phone
    }
}