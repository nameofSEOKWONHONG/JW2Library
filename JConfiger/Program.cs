//#define __SURFACE__

using System;
using System.IO;
using System.Security.Cryptography;
using eXtensionSharp;
using JWLibrary.Database;
using JWLibrary.Utils;

namespace JConfiger {
    class Program {
        static void Main(string[] args) {
            var key = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).xSubstring(0, 16);
            var chiper = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).xSubstring(0, 16);
            var dbProviderObj = new JDatabaseProviderConfig();
            #if __SURFACE__
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
            #else
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
            #endif
            dbProviderObj.KEY = key;
            dbProviderObj.CHIPER = chiper;

            var config = new JConfig();
            config.DatabaseProvider = dbProviderObj;
            var json = config.xObjectToJson();

            using var fs = new FileStream(@"D:\workspace\JW2Library\JConfiguration\jconfig.json", FileMode.OpenOrCreate, FileAccess.Write);
            using var sw = new StreamWriter(fs);
            sw.Write(json);
            sw.Close();
            fs.Close();
        }
    }


}