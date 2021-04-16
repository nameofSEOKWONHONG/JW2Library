﻿using JLiteDBFlex;
using JWLibrary.Core;
using NUnit.Framework;
using Service.Contract;

namespace JWLibrary.NUnit.Test {
    public class LiteDbFlexerTest {
        public CompanyDto companyDto = new CompanyDto();
        
        [SetUp]
        public void Setup() {
            companyDto = new CompanyDto() {
                Name = "red worker",
                Ceo = new UserDto() {
                    Name = "kim",
                    Age = 40,
                },
                Cfo = new UserDto() {
                    Name = "hwang",
                    Age = 50,
                },
                Cto = new UserDto() {
                    Name = "hong",
                    Age = 40
                }
            };
        }

        #region [userdto test]
        [Test]
        public void insert_test() {
            var exists = LiteDbFlexerManager.Instance.Create<CompanyDto>().LiteCollection
                .FindOne(m => m.Name == "red worker");
            
            Assert.NotNull(exists);
            
            var ceoinserted= LiteDbFlexerManager.Instance.Create<UserDto>().LiteCollection.Insert(exists.Ceo);
            var cfoinserted = LiteDbFlexerManager.Instance.Create<UserDto>().LiteCollection.Insert(exists.Cfo);
            var ctoinserted = LiteDbFlexerManager.Instance.Create<UserDto>().LiteCollection.Insert(exists.Cto);
            
            Assert.Greater(ceoinserted.AsInt32, 0);
            Assert.Greater(cfoinserted.AsInt32, 0);
            Assert.Greater(ctoinserted.AsInt32, 0);
        }

        [Test]
        public void get_test() {
            var ceo = LiteDbFlexerManager.Instance.Create<UserDto>().LiteCollection.FindOne(m => m.Name == "kim");
            Assert.NotNull(ceo);
        }

        [Test]
        public void get_delete_test() {
            var exists = LiteDbFlexerManager.Instance.Create<UserDto>().LiteCollection.FindOne(m => m.Name == "kim");
            if (exists.isNotNull()) {
                LiteDbFlexerManager.Instance.Create<UserDto>().LiteCollection.Delete(exists.Name);
            }

            exists = LiteDbFlexerManager.Instance.Create<UserDto>().LiteCollection.FindOne(m => m.Name == "kim");
            
            Assert.IsNull(exists);
        }

        [Test]
        public void get_update_test() {
            var exists = LiteDbFlexerManager.Instance.Create<UserDto>().LiteCollection.FindOne(m => m.Name == "hwang");
            if (exists.isNotNull()) {
                exists.Age = 22;
                var result= LiteDbFlexerManager.Instance.Create<UserDto>().LiteCollection.Update(exists);
                Assert.IsTrue(result);
            }
            
            exists = LiteDbFlexerManager.Instance.Create<UserDto>().LiteCollection.FindOne(m => m.Name == "hwang");
            Assert.AreEqual(22, exists.Age);
        }

        [Test]
        public void get_flexermanager_test() {
            var exists = LiteDbFlexerManager.Instance.Create<UserDto>()
                .LiteCollection
                .FindOne(m => m.Name == "test");

            if (exists != null) {
                exists.Age = 40;
                var updated = LiteDbFlexerManager.Instance.Create<UserDto>()
                    .LiteCollection
                    .Update(exists);
                
                Assert.IsTrue(updated);

                exists = LiteDbFlexerManager.Instance.Create<UserDto>()
                    .LiteCollection
                    .FindOne(m => m.Name == "test");
                
                Assert.AreEqual(40, exists.Age);

                var removed = LiteDbFlexerManager.Instance.Create<UserDto>().LiteCollection
                    .Delete(exists.Name);
                
                Assert.IsTrue(removed);
            }
        }
        

        #endregion
       
        [Test]
        public void get_companyinfo_test() {
            var exists = LiteDbFlexerManager.Instance.Create<CompanyDto>()
                .LiteCollection
                .FindOne(m => m.Name == "red worker");
	
            Assert.NotNull(exists);            
        }

        [Test]
        public void insert_companyinfo_test() {
            var inserted = LiteDbFlexerManager.Instance.Create<CompanyDto>()
                .LiteCollection
                .Insert(companyDto);
            Assert.Greater(inserted.AsInt32, 0);            
        }

        [Test]
        public void update_companyinfo_test() {
            var exists = LiteDbFlexerManager.Instance.Create<CompanyDto>()
                .LiteCollection.FindOne(m => m.Name == "red worker");
            
            Assert.NotNull(exists);

            var temp = exists.Cto;
            exists.Cto = exists.Cfo;
            exists.Cfo = temp;

            var updated = LiteDbFlexerManager.Instance.Create<CompanyDto>()
                .LiteCollection.Update(exists);
            
            Assert.IsTrue(updated);
        }
        
    }

    [LiteDbTable("cominfo.db", "user", new [] {"Name"})]
    public class UserDto {
        //auto-increment
        //id가 명시적으로 없을 경우 _oid를 생성함. 해당 hash값이 고유값이 됨. Key와는 다른 의미임.
        //public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }

    [LiteDbTable("cominfo.db", "company", new[] {"Name"})]
    public class CompanyDto {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserDto Ceo { get; set; }
        public UserDto Cfo { get; set; }
        public UserDto Cto { get; set; }
    }
    
    
    
}