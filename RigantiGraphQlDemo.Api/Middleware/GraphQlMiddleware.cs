using GraphQL;
using GraphQL.Execution;
using GraphQL.Http;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RigantiGraphQlDemo.Api.GraphQL.Query.Model;
using System;
using System.IO;
using System.Threading.Tasks;

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
            if (httpContext.Request.Path.StartsWithSegments("/GraphQL"))
            {
                ExecutionOptions options = new ExecutionOptions { Schema = schema };

                if (string.Equals(httpContext.Request.Method, "POST", StringComparison.OrdinalIgnoreCase))
                {
                    using var streamReader = new StreamReader(httpContext.Request.Body);
                    var body = await streamReader.ReadToEndAsync();

                    //GraphQLRequest
                    var request = JsonConvert.DeserializeObject<GraphQlQuery>(body);
                    options.Query = request.Query;
                    options.OperationName = request.OperationName;
                    options.Inputs = request.Variables.ToInputs();
                }
                else if (string.Equals(httpContext.Request.Method, "GET", StringComparison.OrdinalIgnoreCase))
                {
                    var x = httpContext.Request.QueryString;
                }
                else
                {
                    await next(httpContext);
                }

                options.Listeners.Add(serviceProvider.GetRequiredService<IDocumentExecutionListener>());
                
                var result = await executor.ExecuteAsync(options).ConfigureAwait(false);
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
