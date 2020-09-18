namespace RigantiGraphQlDemo.Api.Configuration
{
    public static class Policies
    {
        public static string LoggedIn { get; } = "LoggedIn";
        public static string Admin { get; } = "Admin";
    }
}