using System;
using De.Markellus.Maths.Core.Arithmetic;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace De.Markellus.Math.Tests.Core.Arithmetic
{
    [TestClass]
    public class SpigotClientTests
    {
        [TestMethod]
        public void TestAdd()
        {
            SpigotClient client = new SpigotClient();
            Assert.AreEqual("10", client.ProcessData("5 + 5"));
            Assert.AreEqual("55", client.ProcessData("50 + 5"));
            Assert.AreEqual("60773179", client.ProcessData("6436743 + 54336436"));
            Assert.AreEqual(
                "436725467986784358755333029474647929699369760.0739545836638585601336459596568458703293285093258",
                client.ProcessData(
                    "643674578938645390345667.746534736554765436258356324563253 + 436725467986784358754689354895709284309024092.3274198471090931238752896350935928703293285093258"));
            Assert.AreEqual("0", client.ProcessData("-5 + 5"));
            Assert.AreEqual("0", client.ProcessData("-5.0 + 5.0"));
            client.Dispose();
        }

        [TestMethod]
        public void TestSubtract()
        {
            SpigotClient client = new SpigotClient();
            Assert.AreEqual("1", client.ProcessData("6 - 5"));
            client.Dispose();
        }

        [TestMethod]
        public void TestSqrt()
        {
            SpigotClient client = new SpigotClient();
            Assert.AreEqual("3", client.ProcessData("9^(1/2)"));
            client.Dispose();
        }
    }
}
