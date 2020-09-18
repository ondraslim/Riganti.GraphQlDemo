using GraphQL.Types;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using RigantiGraphQlDemo.Api.GraphQL.Authentication;
using RigantiGraphQlDemo.Api.GraphQL.InputTypes;
using System;
using System.Security.Claims;

namespace RigantiGraphQlDemo.Api.GraphQL.Mutations
{
    public class LoginMutation : ObjectGraphType, IMutation
    {
        private const string DemoUserPassword = "user123";
        private const string DemoAdminPassword = "admin123";
        public LoginMutation(IHttpContextAccessor contextAccessor)
        {
            FieldAsync<SessionType>(
                "sessions",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "password" }),
                resolve: async context =>
                {
                    var password = context.GetArgument<string>("password");

                    // Demo purpose only!
                    if (password != DemoUserPassword && password != DemoAdminPassword)
                        return new Session { IsLoggedIn = false, IsAdmin = false };

                    var principal = new ClaimsPrincipal(new ClaimsIdentity("Cookie"));
                    await contextAccessor.HttpContext.SignInAsync(principal, new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow.AddMonths(6),
                        IsPersistent = true
                    });

                    return new Session { IsLoggedIn = true, IsAdmin = password == DemoAdminPassword };
                });
        }
    }
}