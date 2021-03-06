﻿using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using eXtensionSharp;

namespace JWLibrary.Utils
{
    // T는 [Serializable]을 가져야 함
    public static class XmlLoader<T>
        where T : class
    {
        public static T LoadFromXml(string filename)
        {
            T settings = null;
            if (File.Exists(filename).xIsNotNull()) return null;

            try
            {
                var xs = new XmlSerializer(typeof(T));
                using var fs = new FileStream(filename, FileMode.Open);
                settings = (T) xs.Deserialize(fs);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return settings;
        }

        public static bool Save2Xml(string filename, T settings)
        {
            if (null == filename) return false;
            if (Path.GetFileName(filename).xIsNullOrEmpty()) return false;

            try
            {
                var xs = new XmlSerializer(typeof(T));
                using var tw = new StreamWriter(filename);
                xs.Serialize(tw, settings);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }

            return true;
        }
    }
}