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
        public void TestAdd01()
        {
            Assert.AreEqual(RealFactory.GenerateReal("10.0"), new Term("5 + 5").Resolve());
        }

        [TestMethod]
        public void TestAdd02()
        {
            Assert.AreEqual(RealFactory.GenerateReal("15.0"), new Term("5 + 5 + 5").Resolve());
        }

        [TestMethod]
        public void TestAdd03()
        {
            Assert.AreEqual(RealFactory.GenerateReal("5.0"), new Term("5 +-5+ 5").Resolve());
        }

        [TestMethod]
        public void TestAdd04()
        {
            Assert.AreEqual(RealFactory.GenerateReal("5.0"), new Term("-5 + 5 + 5").Resolve());
        }

        [TestMethod]
        public void TestSubtract()
        {
            Assert.AreEqual(RealFactory.GenerateReal("14.0"), new Term("21 -12 --5").Resolve());
        }

        [TestMethod]
        public void TestMultiply()
        {
            Assert.AreEqual(RealFactory.GenerateReal("100.0"), new Term("10 * 10").Resolve());
        }

        [TestMethod]
        public void TestDivide01()
        {
            Assert.AreEqual(RealFactory.GenerateReal("5"), new Term("10 / 2").Resolve());
        }

        [TestMethod]
        public void TestDivide02()
        {
            Assert.AreEqual(RealFactory.GenerateReal("2"), new Term("10 / 5").Resolve());
        }

        [TestMethod]
        public void TestDivide03()
        {
            Assert.AreEqual(RealFactory.GenerateReal("2.5"), new Term("5 / 2").Resolve());
        }

        [TestMethod]
        public void TestDivide04()
        {
            Assert.AreEqual(RealFactory.GenerateReal("0.3p1"), new Term("1 / 3").Resolve());
        }

        [TestMethod]
        public void TestDivide05()
        {
            Assert.AreEqual(RealFactory.GenerateReal("3.5p1"), new Term("32 / 9").Resolve());
        }

        [TestMethod]
        public void TestDivide06()
        {
            Assert.AreEqual(RealFactory.GenerateReal("0.07p2"), new Term("7 / 99").Resolve());
        }

        [TestMethod]
        public void TestDivide07()
        {
            Assert.AreEqual(RealFactory.GenerateReal("0.132p3"), new Term("44 / 333").Resolve());
        }

        [TestMethod]
        public void TestDivide08()
        {
            Assert.AreEqual(RealFactory.GenerateReal("6.781p1"), new Term("61.03 / 9").Resolve());
        }

        [TestMethod]
        public void TestDivide09()
        {
            Assert.AreEqual(RealFactory.GenerateReal("0.00000001p8"), new Term("1 / 99999999").Resolve());
        }

        [TestMethod]
        public void TestDivide10()
        {
            Assert.AreEqual(RealFactory.GenerateReal("0.00000002p8"), new Term("2 / 99999999").Resolve());
        }

        [TestMethod]
        public void TestPow01()
        {
            Assert.AreEqual(RealFactory.GenerateReal("100.0"), new Term("10^2").Resolve());
        }

        [TestMethod]
        public void TestPow02()
        {
            Assert.AreEqual(RealFactory.GenerateReal("1000.0"), new Term("10^3").Resolve());
        }

        [TestMethod]
        public void TestCombinedBasicArithmeticOperations01()
        {
            Assert.AreEqual(RealFactory.GenerateReal("-22.0"), new Term("-5 + 25 - +42").Resolve());


        }

        [TestMethod]
        public void TestCombinedBasicArithmeticOperations02()
        {
            Assert.AreEqual(RealFactory.GenerateReal("105.0"), new Term("10 * 10 + 5").Resolve());
        }

        [TestMethod]
        public void TestCombinedBasicArithmeticOperations03()
        {
            Assert.AreEqual(RealFactory.GenerateReal("150.0"), new Term("10 * (10 + 5)").Resolve());
        }

        [TestMethod]
        public void TestCombinedBasicArithmeticOperations04()
        {
            Assert.AreEqual(RealFactory.GenerateReal("2.0"), new Term("10 / (10 - 5)").Resolve());
        }

        [TestMethod]
        public void TestCombinedBasicArithmeticOperations05()
        {
            Assert.AreEqual(RealFactory.GenerateReal("4.0"), new Term("-21 / -21 *4").Resolve());
        }

        [TestMethod]
        public void TestCombinedBasicArithmeticOperations06()
        {
            Assert.AreEqual(RealFactory.GenerateReal("99.0"), new Term("10^2-1").Resolve());
        }

        [TestMethod]
        public void TestCombinedBasicArithmeticOperations07()
        {
            Assert.AreEqual(RealFactory.GenerateReal("100.0"), new Term("10^(3 - 1)").Resolve());
        }

        [TestMethod]
        public void TestCombinedBasicArithmeticOperations08()
        {
            Assert.AreEqual(RealFactory.GenerateReal("100.0"), new Term("2 * 10^(3 - 1) / 3 / 2 * 3").Resolve());
        }

        [TestMethod]
        public void TestRoot01()
        {
            Assert.AreEqual(RealFactory.GenerateReal("3"), new Term("sqrt(9, 2)").Resolve());
        }

        [TestMethod]
        public void TestRoot02()
        {
            Assert.AreEqual(RealFactory.GenerateReal("3"), new Term("sqrt(9)").Resolve());
        }

        [TestMethod]
        public void TestRoot03()
        {
            Assert.AreEqual(RealFactory.GenerateReal("3"), new Term("sqrt(27, 3)").Resolve());
        }

        [TestMethod]
        public void TestRoot04()
        {
            Assert.AreEqual(RealFactory.GenerateReal("3"), new Term("sqrt(sqrt(81))").Resolve());
        }

        [TestMethod]
        public void TestRoot05()
        {
            Assert.AreEqual(RealFactory.GenerateReal("3"), new Term("sqrt(3 * 3)").Resolve());
        }

        [TestMethod]
        public void TestRoot06()
        {
            Assert.AreEqual(RealFactory.GenerateReal("3"), new Term("sqrt(3 ^ 2, 1 + 1)").Resolve());
        }
    }
}
