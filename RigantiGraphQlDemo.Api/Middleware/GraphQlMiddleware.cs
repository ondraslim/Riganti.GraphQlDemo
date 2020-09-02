using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Http;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RigantiGraphQlDemo.Api.GraphQL.Query.Model;
using System;
using System.IO;
using System.Threading.Tasks;
using GraphQL.Server.Transports.AspNetCore.Common;
using Microsoft.EntityFrameworkCore.Design;
using Newtonsoft.Json.Linq;

namespace RigantiGraphQlDemo.Api.Middleware
{
    public class GraphQlMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IDocumentWriter writer;
        private readonly IDocumentExecuter executor;

        public GraphQlMiddleware(RequestDelegate next, IDocumentWriter writer, IDocumentExecuter executor)
        {
            this.next = next;
            this.writer = writer;
            this.executor = executor;
        }

        public async Task InvokeAsync(HttpContext httpContext, ISchema schema, IServiceProvider serviceProvider)
        {
            if (httpContext.Request.Path.StartsWithSegments("/graphql") && 
                string.Equals(httpContext.Request.Method, "POST", StringComparison.OrdinalIgnoreCase))
            {
                using var streamReader = new StreamReader(httpContext.Request.Body);
                var body = await streamReader.ReadToEndAsync();

                //GraphQLRequest
                var request = JsonConvert.DeserializeObject<GraphQlQuery>(body);

                var result = await executor.ExecuteAsync(doc =>
                {
                    doc.Schema = schema;
                    doc.Query = request.Query;
                    doc.Inputs = request.Variables.ToInputs();
                    doc.Listeners.Add(serviceProvider.GetRequiredService<DataLoaderDocumentListener>());
                }).ConfigureAwait(false);

                var json = writer.Write(result);
                await httpContext.Response.WriteAsync(json);
            }
            else
            {
                await next(httpContext);
            }
        }
    }
}
