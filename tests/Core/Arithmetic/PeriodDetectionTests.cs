using De.Markellus.Maths.Core.Arithmetic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace De.Markellus.Math.Tests.Core.Arithmetic
{
    [TestClass]
    public class PeriodDetectionTests
    {
        [TestMethod]
        public void Test01()
        {
            Assert.AreEqual("12", "12121212".DetectPeriod().Pattern);
        }

        [TestMethod]
        public void Test02()
        {
            Assert.AreEqual("4321", "4321432143214321".DetectPeriod().Pattern);
        }

        [TestMethod]
        public void Test03()
        {
            Assert.AreEqual("4321", "43214321432143211".DetectPeriod().Pattern);
        }

        [TestMethod]
        public void Test04()
        {
            Assert.AreEqual("1432", "432143214321432".DetectPeriod().Pattern);
        }

        [TestMethod]
        public void Test05()
        {
            Assert.AreEqual("4321", "4321432143214321123".DetectPeriod().Pattern);
        }

        [TestMethod]
        public void Test06()
        {
            Assert.AreEqual(null, "43214321432143211234".DetectPeriod().Pattern);
        }

        [TestMethod]
        public void Test07()
        {
            Assert.AreEqual(null, "4321432112344321".DetectPeriod().Pattern);
        }
    }
}
