namespace Demo.API.Interface
{
    public interface ICookiesAccount
    {
        Task<string> AuthenticationSignIn(int userId, string fullName);
    }
}
