using System;
using eXtensionSharp;

namespace JWLibrary.Util.Session
{
    public interface ISessionContext : IDisposable
    {
        IUser GetUser();
    }

    public class SessionContext : ISessionContext
    {
        public SessionContext()
        {
            User = new User();
        }

        protected IUser User { get; }

        public IUser GetUser()
        {
            return User;
        }

        public void Dispose()
        {
        }
    }
}