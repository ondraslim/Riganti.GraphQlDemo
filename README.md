# GraphQL ASP.NET Core demo (7 - HotChocolate)

### HotChocolate
I highly recommend to have a look at the [HotChocolate workshop](https://github.com/ChilliCream/graphql-workshop/tree/master/docs), everything I used is nicely explained there.

### Client generation using StrawberryShake
I used **StrawberryShake** to generate the simple client. All that's is necessary is:
  1. To have the StrawberryShake nugget package installed
  2. To have downloaded a schema definition of the server, you want to generate a client for -> `schema.graphql`
  3. Define queries/mutations/subscriptions you want to generate the code for -> `Queries.graphql`
  
The generation takes place upon project build abd the result can be found in a folder called `Generated`. Detailed and advanced description can be found [here](https://chillicream.com/blog/2019/11/25/strawberry-shake_2).

###### Sources:
* [HotChocolate workshop](https://github.com/ChilliCream/graphql-workshop/tree/master/docs)
* [HotChocolate Docs](https://chillicream.com/docs/hotchocolate/v10/)
* [StrawberryShake Client](https://chillicream.com/blog/2019/11/25/strawberry-shake_2)
* [HotChocolate Auth](https://medium.com/@marcinjaniak/graphql-simple-authorization-and-authentication-with-hotchocolate-11-and-asp-net-core-3-162e0a35743d)
