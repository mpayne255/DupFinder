using DupFinder.Application;
using DupFinder.Domain;
using DupFinder.Infrastructure.Hashing.Interfaces;
using DupFinder.Infrastructure.Serialization.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DupFinder.Tests
{
    [TestClass]
    public class ConsoleAppTests
    {
        [TestMethod]
        public void ConsoleApp_ShowUsage_NoException()
        {
            var mockHash = new Mock<IHashAlgorithm>();
            var mockSerializer = new Mock<IOutputSerializer<Bucket>>();
            var mockConfig = new Mock<Configuration>();

            var app = new ConsoleApp(mockHash.Object, mockSerializer.Object, mockConfig.Object);

            app.ShowUsage();
        }
    }
}
