using De.Markellus.Maths.Core.Arithmetic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace De.Markellus.Math.Tests.Core.Arithmetic
{
    [TestClass]
    public class PeriodDetectionTests
    {
        [TestMethod]
        public void Test()
        {
            Assert.AreEqual("12", "12121212".DetectPeriod().Pattern);
            Assert.AreEqual("4321", "4321432143214321".DetectPeriod().Pattern);
            Assert.AreEqual("4321", "43214321432143211".DetectPeriod().Pattern);
            Assert.AreEqual("1432", "432143214321432".DetectPeriod().Pattern);
            Assert.AreEqual("4321", "4321432143214321123".DetectPeriod().Pattern);
            Assert.AreEqual(null, "43214321432143211234".DetectPeriod().Pattern);
            Assert.AreEqual(null, "4321432112344321".DetectPeriod().Pattern);
        }
    }
}
