using System;
using De.Markellus.Maths.Arithmetic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace De.Markellus.Math.Tests.Internals.Arithmetic
{
    [TestClass]
    public class RealTests
    {
        [TestMethod]
        public void TestAdd01()
        {
            Assert.AreEqual(new Real("10"), new Real("5") + new Real("5"));
        }

        [TestMethod]
        public void TestAdd02()
        {
            Assert.AreEqual(new Real("55"), new Real("50") + new Real("5"));
        }

        [TestMethod]
        public void TestAdd03()
        {
            Assert.AreEqual(new Real("60773179"), new Real("6436743") + new Real("54336436"));
        }

        [TestMethod]
        public void TestAdd04()
        {
            Assert.AreEqual(
                new Real("436725467986784358755333029474647929699369760.0739545836638585601336459596568458703293285093258"),
                new Real(
                    "643674578938645390345667.746534736554765436258356324563253") + new Real("436725467986784358754689354895709284309024092.3274198471090931238752896350935928703293285093258"));
        }

        [TestMethod]
        public void TestAdd05()
        {
            Assert.AreEqual(new Real("0"), new Real("-5") + new Real("5"));
        }

        [TestMethod]
        public void TestAdd06()
        {
            Assert.AreEqual(new Real("0"), new Real("-5.0") + new Real("5.0"));
        }

        [TestMethod]
        public void TestSubtract01()
        {
            Assert.AreEqual(new Real("1"), new Real("6") - new Real("5"));
        }

        [TestMethod]
        public void TestSubtract02()
        {
            Assert.AreEqual(new Real("3999.5"), new Real("4321") - new Real("321.5"));
        }

        [TestMethod]
        public void TestSubtract03()
        {
            Assert.AreEqual(new Real("0.25"), new Real("1/2") - new Real("1/4"));
        }

        [TestMethod]
        public void TestSubtract04()
        {
            Assert.AreEqual(new Real("1/4"), new Real("1/2") - new Real("1/4"));
        }

        [TestMethod]
        public void TestSubtract05()
        {
            Assert.AreEqual(new Real("4/3"), new Real("2") - new Real("2/3"));
        }

        //[TestMethod]
        //public void TestSqrt()
        //{
        //    Assert.AreEqual("3", new Real(("9^(1/2)"));
        //}
    }
}
