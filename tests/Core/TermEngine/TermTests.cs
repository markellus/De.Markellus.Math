using System;
using De.Markellus.Maths.Core.Arithmetic;
using De.Markellus.Maths.Core.TermEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace De.Markellus.Math.Tests.Core.TermEngine
{
    [TestClass]
    public class TermTests
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
            Assert.AreEqual(new Real("10.0"), new Term("5 + 5").Resolve());
            Assert.AreEqual(new Real("15.0"), new Term("5 + 5 + 5").Resolve());
            Assert.AreEqual(new Real("5.0"), new Term("5 +-5+ 5").Resolve());
            Assert.AreEqual(new Real("5.0"), new Term("-5 + 5 + 5").Resolve());
        }

        [TestMethod]
        public void TestSubtract()
        {
            Assert.AreEqual(new Real("14.0"), new Term("21 -12 --5").Resolve());
        }

        [TestMethod]
        public void TestMultiply()
        {
            Assert.AreEqual(new Real("100.0"), new Term("10 * 10").Resolve());
        }

        [TestMethod]
        public void TestDivide()
        {
            Assert.AreEqual(new Real("5"), new Term("10 / 2").Resolve());
            Assert.AreEqual(new Real("2"), new Term("10 / 5").Resolve());
            Assert.AreEqual(new Real("2.5"), new Term("5 / 2").Resolve());
            Assert.AreEqual(new Real("0.3p1"), new Term("1 / 3").Resolve());
            Assert.AreEqual(new Real("3.5p1"), new Term("32 / 9").Resolve());
            Assert.AreEqual(new Real("0.07p2"), new Term("7 / 99").Resolve());
            Assert.AreEqual(new Real("0.132p3"), new Term("44 / 333").Resolve());
            Assert.AreEqual(new Real("6.781p1"), new Term("61.03 / 9").Resolve());
            Assert.AreEqual(new Real("0.00000001p8"), new Term("1 / 99999999").Resolve());
            Assert.AreEqual(new Real("0.00000002p8"), new Term("2 / 99999999").Resolve());
        }

        [TestMethod]
        public void TestPow()
        {
            Assert.AreEqual(new Real("100.0"), new Term("10^2").Resolve());
            Assert.AreEqual(new Real("1000.0"), new Term("10^3").Resolve());
        }

        [TestMethod]
        public void TestCombinedBasicArithmeticOperations()
        {
            Assert.AreEqual(new Real("-22.0"), new Term("-5 + 25 - +42").Resolve());
            Assert.AreEqual(new Real("105.0"), new Term("10 * 10 + 5").Resolve());
            Assert.AreEqual(new Real("150.0"), new Term("10 * (10 + 5)").Resolve());
            Assert.AreEqual(new Real("2.0"), new Term("10 / (10 - 5)").Resolve());
            Assert.AreEqual(new Real("4.0"), new Term("-21 / -21 *4").Resolve());
            Assert.AreEqual(new Real("99.0"), new Term("10^2-1").Resolve());
            Assert.AreEqual(new Real("100.0"), new Term("10^(3 - 1)").Resolve());
            Assert.AreEqual(new Real("100.0"), new Term("2 * 10^(3 - 1) / 3 / 2 * 3").Resolve());
        }
    }
}
