using System;
using JWLibrary.Core;
using JWLibrary.Util.Cache;

namespace JWLibrary.Util.Session {
    public interface ISessionContext : IDisposable {
        IUser GetUser();
        ICacheManager GetCacheManager();
    }

    public class SessionContext : ISessionContext {
        protected IUser User { get; private set; }
        protected ICacheManager CacheManager { get; private set; }

        public SessionContext() {
            User = new User();
            CacheManager = new CacheManager();
        }

        public IUser GetUser() {
            return User;
        }

        public ICacheManager GetCacheManager() {
            return CacheManager;
        }

        public void Dispose() {
            if (CacheManager.jIsNotNull())
                CacheManager.Dispose();
        }
    }
}