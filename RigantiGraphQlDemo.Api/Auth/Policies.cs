namespace RigantiGraphQlDemo.Api.Auth
{
    public static class Policies
    {
        public static string LoggedIn { get; } = "LoggedIn";
        public static string Admin { get; } = "Admin";
    }
}