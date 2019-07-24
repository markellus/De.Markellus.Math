using System;
using System.Collections.Generic;
using System.Text;
using De.Markellus.Maths.Internals.TermProcessing;
using De.Markellus.Maths.System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace De.Markellus.Math.Tests.Internals.TermProcessing
{
    [TestClass]
    public class NodeTests
    {
        private MathEnvironment _env;

        [TestInitialize]
        public void Initialize()
        {
            _env = new MathEnvironment();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _env = null;
        }

        [TestMethod]
        public void TestNodeCreation01()
        {
            Assert.AreEqual("10 + 10", _env.AddTerm("10+10").ToString());
        }

        [TestMethod]
        public void TestNodeCreation02()
        {
            Assert.AreEqual("10 * 10", _env.AddTerm("10 10").ToString());
        }

        [TestMethod]
        public void TestNodeCreation03()
        {
            _env.DefineVariable("x");
            Assert.AreEqual("sqrt(9, 2) * 25 - 12 * x", _env.AddTerm("sqrt(9) * 25 - 12x").ToString());
        }

        [TestMethod]
        public void TestNodeCreation04()
        {
            _env.DefineVariable("x");
            Assert.AreEqual("(sqrt(9, 2) * (25 - 12)) * x", _env.AddTerm("sqrt(9) * (25 - 12)x").ToString());
        }
    }
}
