using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RigantiGraphQlDemo.Client
{
    class Program
    {
        static async Task Main()
        {
            // wait for the server to start
            Thread.Sleep(5000);

            var provider = BuildServiceProvider();
            var client = provider.GetRequiredService<ISchemaClient>();

            var farms = await client.FarmListAsync();

            if (farms.HasErrors)
            {
                Console.WriteLine($"Got errors: { farms.Errors.Select(e => $"{e.Code}: {e.Message}") }" );
            }

            foreach (var farm in farms.Data.Farms.Nodes)
            {
                Console.WriteLine($"Received farm '{farm.Name}' with ID {farm.Id}");
            }

            Console.ReadKey();
        }


        private static IServiceProvider BuildServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddHttpClient(
                "schemaClient",
                c => c.BaseAddress = new Uri("https://localhost:5001"));
            serviceCollection.AddschemaClient();

            return serviceCollection.BuildServiceProvider();
        }
    }
}
