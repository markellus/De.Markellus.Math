using De.Markellus.Maths.Core.Arithmetic;
using De.Markellus.Maths.Core.SimplificationEngine.RootSimplificationRules;
using De.Markellus.Maths.Core.TermEngine;
using De.Markellus.Maths.Core.TermEngine.Nodes.Base;
using De.Markellus.Maths.Core.TermEngine.TermParsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace De.Markellus.Math.Tests.Core.SimplificationEngine
{
    [TestClass]
    public class PowToRootTests
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

        private void Check(string strTerm, string strTermExpected)
        {
            PowToRoot ptr = new PowToRoot();

            Term term = new Term(strTerm);
            Term termExpected = new Term(strTermExpected);

            TermNode termParsed = term.ParseTerm();
            TermNode termExpectedParsed = termExpected.ParseTerm();

            Assert.IsTrue(ptr.CanBeAppliedTo(termParsed, MathExpressionTokenizer.Default), "Not appliable");
            Assert.AreEqual(termExpectedParsed, ptr.Simplify(termParsed, MathExpressionTokenizer.Default));
        }

        [TestMethod]
        public void Test01()
        {
            Check("1^(1/2)", "sqrt(1^1)");
        }

        [TestMethod]
        public void Test02()
        {
            Check("321^(3/7)", "sqrt(321^3, 7)");
        }
    }
}
