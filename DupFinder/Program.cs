using DupFinder.Application;
using DupFinder.Domain;
using DupFinder.Infrastructure.Hashing.Implementation;
using DupFinder.Infrastructure.Hashing.Interfaces;
using DupFinder.Infrastructure.Serialization.Implementation;
using DupFinder.Infrastructure.Serialization.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DupFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = RegisterServices(args);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            serviceProvider.GetService<ConsoleApp>().Run(args);
        }

        private static IServiceCollection RegisterServices(string[] args)
        {
            var serviceCollection = new ServiceCollection();

            var config = Configuration.Build(args);

            serviceCollection.AddSingleton(config);
            serviceCollection.AddTransient<IHashAlgorithm, MD5HashAlgorithm>();
            serviceCollection.AddSingleton<IOutputSerializer<Bucket>>(serviceProvider =>
            {
                var configuration = serviceProvider.GetService<Configuration>();

                return configuration.OutputMode switch
                {
                    Domain.Enums.OutputMode.Json => new JsonOutputSerializer<Bucket>(),
                    Domain.Enums.OutputMode.Xml => new XmlOutputSerializer<Bucket>(),
                    _ => new ConsoleOutputSerializer<Bucket>(),
                };
            });
            serviceCollection.AddTransient<ConsoleApp>();

            return serviceCollection;
        }
    }
}
