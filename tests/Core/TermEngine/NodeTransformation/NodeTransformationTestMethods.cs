using System;
using De.Markellus.Maths.Core.TermEngine;
using De.Markellus.Maths.Core.TermEngine.Nodes.Base;
using De.Markellus.Maths.Core.TermEngine.NodeTransformation;
using De.Markellus.Maths.Core.TermEngine.TermParsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace De.Markellus.Math.Tests.Core.TermEngine.NodeTransformation
{
    internal static class NodeTransformationTestMethods
    {
        public static void Check(string strTerm, string strTermExpected, Type targetType)
        {
            INodeTransformationRule rule = (INodeTransformationRule)Activator.CreateInstance(targetType);

            Term term = new Term(strTerm);
            Term termExpected = new Term(strTermExpected);

            TermNode termParsed = term.ParseTerm();
            TermNode termExpectedParsed = termExpected.ParseTerm();

            Assert.IsTrue(rule.CanBeAppliedTo(termParsed, MathExpressionTokenizer.Default), "Not appliable");

            TermNode termParsedSimplified = rule.Transform(termParsed, MathExpressionTokenizer.Default);

            Assert.AreEqual(termExpectedParsed, termParsedSimplified);

            Assert.AreEqual(termParsed.IsResolvable(), termExpectedParsed.IsResolvable());
            Assert.AreEqual(termParsed.IsResolvable(), termParsedSimplified.IsResolvable());

            if (termParsed.IsResolvable())
            {
                Assert.AreEqual(termParsed.Resolve(), termExpectedParsed.Resolve());
                Assert.AreEqual(termParsed.Resolve(), termParsedSimplified.Resolve());
            }
        }
    }
}
