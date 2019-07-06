using System;
using System.Collections.Generic;
using System.Text;
using De.Markellus.Maths.Core.Arithmetic;
using De.Markellus.Maths.Core.SimplificationEngine.RootSimplificationRules;
using De.Markellus.Maths.Core.TermEngine;
using De.Markellus.Maths.Core.TermEngine.Nodes.Base;
using De.Markellus.Maths.Core.TermEngine.TermParsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace De.Markellus.Math.Tests.Core.SimplificationEngine
{
    [TestClass]
    public class RootDivisionTests
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
            RootDivision ptr = new RootDivision();

            Term term = new Term(strTerm);
            Term termExpected = new Term(strTermExpected);

            TermNode termParsed = term.ParseTerm();
            TermNode termExpectedParsed = termExpected.ParseTerm();

            Assert.IsTrue(ptr.CanBeAppliedTo(termParsed, MathExpressionTokenizer.Default), "Not appliable");

            TermNode termParsedSimplified = ptr.Simplify(termParsed, MathExpressionTokenizer.Default);

            Assert.AreEqual(termExpectedParsed, termParsedSimplified);
        }

        [TestMethod]
        public void Test01()
        {
            Check("sqrt(5) / sqrt(12)", "sqrt(5 / 12)");
        }

        [TestMethod]
        public void Test02()
        {
            Check("sqrt(4,3) / sqrt(3, 4)", "sqrt(4 / 3, 3 * 4)");
        }

        [TestMethod]
        public void Test03()
        {
            Check("sqrt(4 / 342 * 40 ^3 , 3) / sqrt(43252, 3)", "sqrt((4 / 342 * 40 ^3) / 43252, 3)");
        }
    }
}
