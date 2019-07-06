using System;
using System.Collections.Generic;
using System.Text;
using De.Markellus.Maths.Core;
using De.Markellus.Maths.Core.Arithmetic;
using De.Markellus.Maths.Core.TermEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace De.Markellus.Math.Tests.Core
{
    [TestClass]
    public class MathEnvironmentTests
    {
        private MathEnvironment _env;

        [TestInitialize]
        public void Initialize()
        {
            SpigotClient.Start();
            _env = new MathEnvironment();
        }

        [TestCleanup]
        public void Cleanup()
        {
            SpigotClient.Stop();
            _env = null;
        }

        private bool Check(string strTerm, string strTermSimplified, params string[] arrVariables)
        {
            foreach (string strVar in arrVariables)
            {
                _env.DefineVariable(strVar);
            }

            Term term = _env.DefineTerm(strTerm);
            Term termExpected = _env.DefineTerm(strTermSimplified);

            foreach (Term t in _env.SimplifyTerm(term))
            {
                if (t.ParseTerm() == termExpected.ParseTerm())
                {
                    return true;
                }
            }

            return false;
        }

        [TestMethod]
        public void SimplifyTerm001()
        {
            Assert.IsTrue(Check(
                "a^3 / a^2",
                "a",
                "a"));
        }

        [TestMethod]
        public void SimplifyTerm002()
        {
            Assert.IsTrue(Check(
                "sqrt(a^2, 3) / a^(1/2)",
                "sqrt(a, 6)",
                "a"));
        }

        [TestMethod]
        public void SimplifyTerm003()
        {
            Assert.IsTrue(Check(
                "(sqrt(a^2, 4) + b) / sqrt(a)",
                "b / sqrt(a)  + 1",
                "a", "b"));
        }

        [TestMethod]
        public void SimplifyTerm004()
        {
            Assert.IsTrue(Check(
                "e^x + 1 / e^-x",
                "2e^x",
                "e", "x"));
        }

        [TestMethod]
        public void SimplifyTerm005()
        {
            Assert.IsTrue(Check(
                "a^(x-2)/a^4",
                "a^x-6",
                "a", "x"));
        }

        [TestMethod]
        public void SimplifyTerm006()
        {
            Assert.IsTrue(Check(
                "x^3/x^-2",
                "x^5",
                "x"));
        }

        [TestMethod]
        public void SimplifyTerm007()
        {
            Assert.IsTrue(Check(
                "(4a^2+12ab+9b^2)/(4a^2-9b^2)",
                "1/(2a-3b)",
                "a", "b"));
        }
    }
}
