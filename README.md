# GraphQL ASP.NET Core demo (6 - GraphQL 3.0 migration, code cleanup)

Updated the code to the newest dotnet *GraphQL* packages *(guide in the sources)*.

I also figured out that method `.AddDataLoaders()` of `IGraphQlBuilder` is sufficient to provide efficient requests.
Thus the middleware `GraphQlMiddleware.cs` is not necessary and was deleted.
The same is true for the controller `GraphQlController.cs`, the GraphQl can handle the requests by the pipeline.

###### Sources:

* [GraphQL 3.0 migration guide](https://graphql-dotnet.github.io/docs/guides/migration3)
