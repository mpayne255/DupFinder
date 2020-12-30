using DupFinder.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DupFinder.Tests
{
    [TestClass]
    public class ConfigurationTests
    {
        [TestMethod]
        public void Configuration_NoArguments_ReturnsNull()
        {
            var config = Configuration.Build(new string[] { });
            
            Assert.IsNull(config);
        }

        [TestMethod]
        public void Configuration_SetAllValues()
        {
            var config = Configuration.Build(new[]
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
            var config = Configuration.Build(new[]
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
