namespace JWLibrary.Util.Session
{
    public interface IUser
    {
        string Id { get; set; }
        string Token { get; set; }
    }

    public class User : IUser
    {
        public string Id { get; set; }
        public string Token { get; set; }
    }
}