using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;

namespace JWLibrary.Utils {
    public class LanguageHandler<T>
        where T : class, new() {
        private static readonly Lazy<LanguageHandler<T>> _instance =
            new(() => new LanguageHandler<T>());

        private readonly Dictionary<string, string> _keyValues = new() {
            {"ko-KR", "./Resource/Lang/ko-KR.json"},
            {"en-US", "./Resource/Lang/en-US.json"}
        };

        private readonly Dictionary<string, T> _languageResources = new();

        private T _languageResource;

        private LanguageHandler() {
            if (_languageResources.Count <= 0) {
                _languageResources.Add("en-US", LoadLanguageSetting("en-US"));
                _languageResources.Add("ko-KR", LoadLanguageSetting("ko-KR"));
            }
        }

        public static LanguageHandler<T> Instance => _instance.Value;

        public T LanguageResource {
            get {
                if (_languageResource == null)
                    _languageResource = _languageResources[Thread.CurrentThread.CurrentCulture.Name];

                return _languageResource;
            }
        }

        public T this[string lang] {
            get {
                _languageResource = _languageResources[lang];
                return _languageResource;
            }
        }

        private T LoadLanguageSetting(string language) {
            var keyValue = _keyValues.FirstOrDefault(m => m.Key == language);
            T langRes = null;

            if (string.IsNullOrEmpty(keyValue.Key)) keyValue = _keyValues.First(m => m.Key == "en-US");

            var resourceJson = File.ReadAllText(keyValue.Value);

            langRes = JsonConvert.DeserializeObject<T>(resourceJson);

            var numberFormatInfo = CultureInfo.CreateSpecificCulture(language).NumberFormat;
            var cultureInfo = new CultureInfo(language) {NumberFormat = numberFormatInfo};

            if (language == "ko-KR") {
                cultureInfo.DateTimeFormat.DateSeparator = "-";
                cultureInfo.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
            }
            else {
                cultureInfo.DateTimeFormat.DateSeparator = "/";
                cultureInfo.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            }

            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            Thread.CurrentThread.CurrentCulture = cultureInfo;

            return langRes;
        }
    }
}