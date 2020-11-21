﻿using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;

namespace JWLibrary.Utils {

    public static class JsonLoader<T> where T : class {

        public static T LoadFromJson(string filename) {
            if (false == File.Exists(filename)) return null;

            try {
                // read JSON directly from a file
                using (var file = File.OpenText(filename)) {
                    var json = file.ReadToEnd();
                    return JsonConvert.DeserializeObject<T>(json);
                }
            } catch (Exception e) {
                Debug.Assert(false);
                e.ToString();
            }

            return null;
        }

        public static bool SaveToJson(string filename, T settings) {
            if (null == filename || string.Empty == Path.GetFileName(filename)) return false;

            var json = JsonConvert.SerializeObject(settings, Formatting.Indented);

            try {
                using (var sw = File.CreateText(filename)) {
                    sw.Write(json);
                }
            } catch (Exception e) {
                Debug.Assert(false);
                e.ToString();
                return false;
            }

            return true;
        }
    }
}