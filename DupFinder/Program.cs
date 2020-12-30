using DupFinder.Application;
using DupFinder.Hashing.Implementation;
using DupFinder.Hashing.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DupFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = RegisterServices();
            var serviceProvider = serviceCollection.BuildServiceProvider();

            serviceProvider.GetService<ConsoleApp>().Run(args);
        }

        private static IServiceCollection RegisterServices()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddTransient<IHashAlgorithm, MD5HashAlgorithm>();
            // TODO: register multiple OutputSerializers
            serviceCollection.AddTransient<ConsoleApp>();

            return serviceCollection;
        }
    }
}
