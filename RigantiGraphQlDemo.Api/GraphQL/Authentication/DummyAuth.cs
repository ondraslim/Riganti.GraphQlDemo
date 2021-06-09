using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace RigantiGraphQlDemo.Api.GraphQL.Authentication
{
    // auth handler -> Microsoft.AspNetCore.Authentication
    public class DummyAuth : AuthenticationHandler<DummyAuthOptions>
    {
        public DummyAuth(
            IOptionsMonitor<DummyAuthOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder, 
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // dummy auth
            if (Request.Headers.TryGetValue("Authentication", out var token) && token == "authenticated")
            {
                var claims = new List<Claim> {
                    new(ClaimTypes.NameIdentifier, "LoggedInUser")
                };
                return Task.FromResult(
                    AuthenticateResult.Success(
                        new AuthenticationTicket(
                            new ClaimsPrincipal(
                                new ClaimsIdentity(claims, Scheme.Name)),
                            Scheme.Name)));
            }

            return Task.FromResult(AuthenticateResult.Fail("Invalid login"));
        }
    }
}