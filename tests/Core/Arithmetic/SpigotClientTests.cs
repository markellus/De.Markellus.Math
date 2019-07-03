using System;
using De.Markellus.Maths.Core.Arithmetic;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace De.Markellus.Math.Tests.Core.Arithmetic
{
    [TestClass]
    public class SpigotClientTests
    {
        [TestInitialize]
        public void Initialize()
        {
            SpigotClient.Start();
        }

        [TestCleanup]
        public void Cleanup()
        {
            SpigotClient.Stop();
        }

        [TestMethod]
        public void TestAdd()
        {
            Assert.AreEqual("10", SpigotClient.Add("5", "5"));
            Assert.AreEqual("55", SpigotClient.Add("50", "5"));
            Assert.AreEqual("60773179", SpigotClient.Add("6436743", "54336436"));
            Assert.AreEqual(
                "436725467986784358755333029474647929699369760.0739545836638585601336459596568458703293285093258",
                SpigotClient.Add("643674578938645390345667.746534736554765436258356324563253",
                    "436725467986784358754689354895709284309024092.3274198471090931238752896350935928703293285093258"));
            Assert.AreEqual("0", SpigotClient.Add("-5", "5"));
            Assert.AreEqual("0", SpigotClient.Add("-5.0", "5.0"));
        }

        [TestMethod]
        public void TestSubtract()
        {
            Assert.AreEqual("1", SpigotClient.Subtract("6", "5"));
        }
    }
}
