using DupFinder.Application;
using DupFinder.Application.Services.Interfaces;
using DupFinder.Domain;
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
            var mockDupService = new Mock<IDuplicateService>();
            var mockSerializer = new Mock<IOutputSerializer<DuplicateResult>>();
            var mockConfig = new Mock<Configuration>();

            var app = new ConsoleApp(mockSerializer.Object, mockConfig.Object, mockDupService.Object);

            app.ShowUsage();
        }
    }
}
