using GraphQL.Validation.Complexity;
using RigantiGraphQlDemo.Api.Configuration.Auth;
using System.Collections.Generic;
using System.Security.Claims;

namespace RigantiGraphQlDemo.Api.Configuration
{
    public class GraphQlConfig
    {
        public static ComplexityConfiguration ComplexityConfiguration =>
            new ComplexityConfiguration
            {
                MaxDepth = 3,                   // Nested types in query
                FieldImpact = 3,                // average number of records in db
                MaxComplexity = 1_000           // max amount of returned entities in a query
            };

        public static ClaimsPrincipal FakedUser => new ClaimsPrincipal(
            new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name, "Jon Doe"),
                new Claim(ClaimTypes.Role, Roles.UserRole)
            }));
    }
}