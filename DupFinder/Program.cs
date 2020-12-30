using DupFinder.Application;
using DupFinder.Application.Services.Implementation;
using DupFinder.Application.Services.Interfaces;
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
            serviceCollection.AddTransient<IDuplicateService, DuplicateService>();
            serviceCollection.AddSingleton<IOutputSerializer<DuplicateResult>>(serviceProvider =>
            {
                var configuration = serviceProvider.GetService<Configuration>();

                return configuration.OutputMode switch
                {
                    Domain.Enums.OutputMode.Json => new JsonOutputSerializer<DuplicateResult>(),
                    Domain.Enums.OutputMode.Xml => new XmlOutputSerializer<DuplicateResult>(),
                    _ => new StringOutputSerializer<DuplicateResult>(),
                };
            });
            serviceCollection.AddTransient<ConsoleApp>();

            return serviceCollection;
        }
    }
}
