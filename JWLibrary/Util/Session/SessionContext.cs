using System;
using eXtensionSharp;
using JWLibrary.Util.Cache;

namespace JWLibrary.Util.Session {
    public interface ISessionContext : IDisposable {
        IUser GetUser();
        ICacheManager GetCacheManager();
    }

    public class SessionContext : ISessionContext {
        public SessionContext() {
            User = new User();
            CacheManager = new CacheManager();
        }

        protected IUser User { get; }
        protected ICacheManager CacheManager { get; }

        public IUser GetUser() {
            return User;
        }

        public ICacheManager GetCacheManager() {
            return CacheManager;
        }

        public void Dispose() {
            if (CacheManager.xIsNotNull())
                CacheManager.Dispose();
        }
    }
}