using DupFinder.Application;
using Infrastructure.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DupFinder.Tests
{
    [TestClass]
    public class ConfigurationTests
    {
        [TestMethod]
        public void Configuration_NoArguments_ReturnsNull()
        {
            var mockHash = new Mock<IHashAlgorithm>();
            var app = new ConsoleApp(mockHash.Object);

            var config = app.GetConfiguration(new string[] { });
            
            Assert.IsNull(config);
        }

        [TestMethod]
        public void Configuration_SetAllValues()
        {
            var mockHash = new Mock<IHashAlgorithm>();
            var app = new ConsoleApp(mockHash.Object);

            var config = app.GetConfiguration(new[]
            {
                "-o", "myOutputFile.xml",
                "-om", "Xml",
                "-m", "file",
                "-i",
                @"C:\MyDirectory1",
                @"C:\MyDirectory2"
            });

            Assert.IsNotNull(config);
            Assert.AreEqual("myOutputFile.xml", config.OutputTarget);
            Assert.AreEqual(Domain.Enums.OutputMode.Xml, config.OutputMode);
            Assert.AreEqual(Domain.Enums.DetectionMode.File, config.Mode);
            Assert.IsTrue(config.IncludeEmpty);
            Assert.AreEqual(@"C:\MyDirectory1", config.Directories[0]);
            Assert.AreEqual(@"C:\MyDirectory2", config.Directories[1]);
        }

        [TestMethod]
        public void Configuration_SetDirectoriesOnly_HasDefaults()
        {
            var mockHash = new Mock<IHashAlgorithm>();
            var app = new ConsoleApp(mockHash.Object);

            var config = app.GetConfiguration(new[]
            {
                @"C:\MyDirectory1",
                @"C:\MyDirectory2"
            });

            Assert.IsNotNull(config);
            Assert.IsNull(config.OutputTarget);
            Assert.AreEqual(Domain.Enums.OutputMode.Console, config.OutputMode);
            Assert.AreEqual(Domain.Enums.DetectionMode.File, config.Mode);
            Assert.IsFalse(config.IncludeEmpty);
            Assert.AreEqual(@"C:\MyDirectory1", config.Directories[0]);
            Assert.AreEqual(@"C:\MyDirectory2", config.Directories[1]);
        }
    }
}
