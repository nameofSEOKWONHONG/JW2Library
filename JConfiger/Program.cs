using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using eXtensionSharp;
using JWLibrary.Database;
using JWLibrary.Utils;
using JWLibrary.Web.Consts;

namespace JConfiger {
    class Program {
        static void Main(string[] args) {
            if (args.xIsEmpty()) {
                args = new[] {
                    "SURFACE",
                    CONFIG_CONST.DATABASE_CONFIG_PATH
                };
            }
            var settingText = args[0];
            var filePath = args[1];

            if (settingText.xIsNullOrEmpty()) throw new Exception("settingText is empty");
            if (filePath.xIsNullOrEmpty()) throw new Exception("filePath is empty.");
            
            WriteSettingFile(settingText, filePath);
        }

        static void WriteSettingFile(string settingText, string filePath) { 
            var key = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).xSubstring(0, 16);
            var chiper = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).xSubstring(0, 16);
            var dbProviderObj = new JDatabaseProviderConfig();
            if(settingText.ToUpper() == "SURFACE") {
                dbProviderObj.MSSQL =
                    "Data Source=192.168.137.245;Initial Catalog=testdb;User ID=sa;Password=1q2w3e4r!Q@W#E$R;".xToEncAes256(
                        key, chiper, CipherMode.CBC, PaddingMode.PKCS7);
                dbProviderObj.MYSQL = "Server=192.168.137.245;Port=3306;Database=testdb;Uid=admin;Pwd=1q2w3e4r!Q@W#E$R;".xToEncAes256(
                    key, chiper, CipherMode.CBC, PaddingMode.PKCS7);
                dbProviderObj.POSTGRESQL = "Server=192.168.137.245;Port=5432;Database=testdb;User Id=seokwon;Password=1q2w3e4r!Q@W#E$R;".xToEncAes256(
                    key, chiper, CipherMode.CBC, PaddingMode.PKCS7);
                dbProviderObj.MONGODB = "mongodb://seokwon:1q2w3e4r!Q@W#E$R@192.168.137.245:27017/testdb".xToEncAes256(
                    key, chiper, CipherMode.CBC, PaddingMode.PKCS7);
                dbProviderObj.REDIS = "192.168.137.245:6379,allowAdmin=true,password=1q2w3e4r!Q@W#E$R".xToEncAes256(
                    key, chiper, CipherMode.CBC, PaddingMode.PKCS7);
                dbProviderObj.SQLITE = "Data Source=./testdb.db;Version=3;".xToEncAes256(
                    key, chiper, CipherMode.CBC, PaddingMode.PKCS7);
                dbProviderObj.SQLITE_IN_MEMORY = "Data Source=:memory:;Version=3;New=True;".xToEncAes256(
                    key, chiper, CipherMode.CBC, PaddingMode.PKCS7);
            } else {
                dbProviderObj.MSSQL =
                    "Data Source=192.168.137.233;Initial Catalog=testdb;User ID=sa;Password=1q2w3e4r!Q@W#E$R;".xToEncAes256(
                        key, chiper, CipherMode.CBC, PaddingMode.PKCS7);
                dbProviderObj.MYSQL = "Server=192.168.137.233;Port=3306;Database=testdb;Uid=admin;Pwd=1q2w3e4r!Q@W#E$R;".xToEncAes256(
                    key, chiper, CipherMode.CBC, PaddingMode.PKCS7);
                dbProviderObj.POSTGRESQL = "Server=192.168.137.233;Port=5432;Database=testdb;User Id=seokwon;Password=1q2w3e4r!Q@W#E$R;".xToEncAes256(
                    key, chiper, CipherMode.CBC, PaddingMode.PKCS7);
                dbProviderObj.MONGODB = "mongodb://seokwon:1q2w3e4r!Q@W#E$R@192.168.137.233:27017/testdb".xToEncAes256(
                    key, chiper, CipherMode.CBC, PaddingMode.PKCS7);
                dbProviderObj.REDIS = "192.168.137.233:6379,allowAdmin=true,password=1q2w3e4r!Q@W#E$R".xToEncAes256(
                    key, chiper, CipherMode.CBC, PaddingMode.PKCS7);
                dbProviderObj.SQLITE = "Data Source=./testdb.db;Version=3;".xToEncAes256(
                    key, chiper, CipherMode.CBC, PaddingMode.PKCS7);
                dbProviderObj.SQLITE_IN_MEMORY = "Data Source=:memory:;Version=3;New=True;".xToEncAes256(
                    key, chiper, CipherMode.CBC, PaddingMode.PKCS7);            
            }
            dbProviderObj.KEY = key;
            dbProviderObj.CHIPER = chiper;

            var config = new JConfig();
            config.DatabaseProvider = dbProviderObj;
            var json = config.xObjectToJson();

            var configPath = filePath;
            configPath.xFileCreateAll();
            Thread.Sleep(1000);

            using (var fs = File.Open(configPath, FileMode.OpenOrCreate, FileAccess.ReadWrite)) {
                using var sw = new StreamWriter(fs);
                sw.Write(json);
                sw.Close();
                fs.Close();
            }
        }
    }


}
