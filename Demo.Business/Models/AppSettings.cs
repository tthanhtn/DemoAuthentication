namespace Demo.Business.Entity
{
    public class AppSettings
    {
        public string ConnectionString { get; set; }
        public string IdentityServerAuthority { get; set; }
        public Jwt Jwt { get; set; }

    }
}
