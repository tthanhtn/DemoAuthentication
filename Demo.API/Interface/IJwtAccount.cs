namespace Demo.API.Interface
{
    public interface IJwtAccount
    {
        Task<string> AuthenticationSignIn(int userId, string fullName);
    }
}
