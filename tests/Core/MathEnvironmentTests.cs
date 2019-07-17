using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using De.Markellus.Maths.Core;
using De.Markellus.Maths.Core.Arithmetic;
using De.Markellus.Maths.Core.TermEngine;
using De.Markellus.Maths.Core.TermEngine.Nodes.Implementation;
using De.Markellus.Maths.Core.TermEngine.NodeTransformation;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32.SafeHandles;

namespace De.Markellus.Math.Tests.Core
{
    [TestClass]
    public class MathEnvironmentTests
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

        private void Check(string strTerm, string strTermSimplified, params string[] arrVariables)
        {
            foreach (string strVar in arrVariables)
            {
                _env.DefineVariable(strVar);
            }

            Term term = _env.DefineTerm(strTerm);
            Term termExpected = _env.DefineTerm(strTermSimplified);

            Assert.AreEqual(term.IsResolvable(), termExpected.IsResolvable());

            if (term.IsResolvable())
            {
                Assert.AreEqual(term.Resolve(), termExpected.Resolve());
            }

            bool bSimplified = false;

            foreach (Term t in _env.SimplifyTerm(term))
            {
                Console.WriteLine($"Found: {t.ParseTerm()}");
                if (t.ParseTerm() == termExpected.ParseTerm())
                {
                    bSimplified = true;
                }

                Assert.AreEqual(termExpected.IsResolvable(), t.IsResolvable(), "Simplidief terms are not equally resolvable");
                if (t.IsResolvable())
                {
                    Assert.AreEqual(termExpected.Resolve(), t.Resolve(), "Simplified terms are not equal");
                }
            }

            Assert.IsTrue(bSimplified, "Simplification failed");
        }

        [TestMethod]
        public void TestResolve01()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Resolve");
            Check("10+10", "20");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestResolve02()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Resolve");
            Check("132 * 2 - 21", "243");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestBasics01()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Basic Rule");
            Check("132 * 2", "2 * 132");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestParenthesisAddition01()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Parenthesis Rule");
            Check("(12 + 10) + 13", "12 + (10 + 13)");
            Check("12 + (10 + 13)", "(12 + 10) + 13");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestParenthesisAddition02()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Parenthesis Rule");
            Check("(- 21 + 4) + 10", "- 21 + (4 + 10)");
            Check("- 21 + (4 + 10)", "(- 21 + 4) + 10");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestParenthesisAddition03()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Parenthesis Rule");
            Check("(21 - 4) + 10", "21 - (4 - 10)");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestParenthesisAddition04()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Parenthesis Rule");
            Check("(21 - -4) + -10", "21 - (-4 - -10)");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestParenthesisAddition05()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Parenthesis Rule");
            Check("10 + (21 - 4)", "(10 + 21) - 4");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestParenthesisAddition06()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Parenthesis Rule");
            Check("-10 + (21 - -4)", "(-10 + 21) - -4");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestParenthesisSubtraction01()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Parenthesis Rule");
            Check("(12 - 10) - 13", "12 - (10 + 13)");
            Check("12 - (10 - 13)", "(12 - 10) + 13");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestParenthesisSubtraction02()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Parenthesis Rule");
            Check("(- 21 - 4) - 10", "- 21 - (4 + 10)");
            Check("- 21 - (4 - 10)", "(- 21 - 4) + 10");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestParenthesisSubtraction03()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Parenthesis Rule");
            Check("(12 + 10) - 13", "12 + (10 - 13)");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestParenthesisSubtraction04()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Parenthesis Rule");
            Check("(- 21 + 4) - 10", "- 21 + (4 - 10)");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestParenthesisSubtraction05()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Parenthesis Rule");
            Check("((21 + 12 + 14) + 4) - 10", "(21 + 12 + 14) + (4 - 10)");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestParenthesisSubtraction06()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Parenthesis Rule");
            Check("(- 21 * 52 + 4 * 7) - 10", "- 21 * 52 + ( 4 * 7 - 10)");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestParenthesisSubtraction07()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Parenthesis Rule");
            Check("12 - (10 + 13)", "12 - 10 - 13");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestParenthesisSubtraction08()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Parenthesis Rule");
            Check("- 21 - (4 + 10)", "(- 21 - 4) - 10");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestParenthesisSubtraction09()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Parenthesis Rule");
            Check("(21 + 12 + 14) - (4 + 10)", "(21 + 12 + 14) - 4 - 10");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestParenthesisSubtraction10()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Parenthesis Rule");
            Check("- 21 * 52 - (4 * 7 + 10)", "- 21 * 52 - 4 * 7 - 10");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestParenthesisMultiplication01()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Parenthesis Rule");
            Check("(12 * 10) * 13", "12 * (10 * 13)");
            Check("12 * (10 * 13)", "(12 * 10) * 13");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestParenthesisMultiplication02()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Parenthesis Rule");
            Check("((4 * 5) * 6) * 10", "(4 * 5) * (6 * 10)");
            Check("(4 * (5 * 6)) * 10", "4 * ((5 * 6) * 10)");
            Check("4 * (5 * 6 * 10)", "(4 * (5 * 6)) * 10");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestParenthesisDivision01()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Parenthesis Rule");
            Check("(12 / 10) / 13", "12 / (10 * 13)");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestParenthesisDivision02()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Parenthesis Rule");
            Check("((12 * 31 ^3 sqrt(81)) / 10) / 34^19", "(12 * 31 ^3 sqrt(81)) / (10 * 34^19)");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestPowAddition01()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Power Rule");
            Check("31^21 + 31^21", "2 * 31^21");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestPowAddition02()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Power Rule Addition 1");
            Check("(342 * 13 + 543289 / 546^2)^2 + (342 * 13 + 543289 / 546^2)^2", "2 * (342 * 13 + 543289 / 546 ^2)^2");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestPowAddition03()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Power Rule");
            Check("31^21 + 4 * 31^21", "(1 + 4) * 31^21");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestPowAddition04()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Power Rule", "Basic Rule");
            Check("31^21 + 31^21 * 4", "(1 + 4) * 31^21");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestPowAddition05()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Power Rule", "Basic Rule");
            Check("4 * 31^21 + 31^21", "(4 + 1) * 31^21");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestPowAddition06()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Power Rule", "Basic Rule");
            Check("31^21 * 4 + 31^21", "(4 + 1) * 31^21");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestPowAddition07()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Power Rule", "Basic Rule");
            Check("3 * 31^21 + 4 * 31^21", "(3 + 4) * 31^21");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestPowAddition08()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Power Rule", "Basic Rule");
            Check("21 * 31^21 + 31^21 * 4", "(21 + 4) * 31^21");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestPowAddition09()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Power Rule", "Basic Rule");
            Check("31^21 * 4 + 31^21 * 8", "(4 + 8) * 31^21");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestPowAddition10()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Power Rule", "Basic Rule");
            Check("31^21 * 4 + 12 * 31^21", "(4 + 12) * 31^21");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestPowAddition11()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Power Rule", "Basic Rule");
            Check(" 3 * (342 * 13 + 543289 / 321 ^2)^2 + (342 * 13 + 543289 / 321 ^2)^2", "(3 + 1) * (342 * 13 + 543289 / 321 ^2)^2");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestPowDivision01()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Power Rule");
            Check("2^32 / 2^10", "2^(32 - 10)");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestPowDivision02()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Power Rule");
            Check("1^2 / 1^3", "1^(2 - 3)");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestPowSimplify01()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Power Rule");
            Check("a^1", "a", "a");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestPowSimplify02()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Power Rule", "Basic Rule");
            Check("31241^1", "31241");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestPowSimplify03()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Power Rule", "Basic Rule");
            Check("(34729 + 3249 / 3421)^1", "34729 + 3249 / 3421");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestPowToRoot01()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Power Rule");
            Check("1^(1/2)", "sqrt(1)");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestPowToRoot02()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Power Rule");
            Check("321^(3/7)", "sqrt(321^3, 7)");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestRootAddition01()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Root Rule");
            Check("sqrt(5) + sqrt(5)", "2 * sqrt(5)");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestRootAddition02()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Root Rule");
            Check("sqrt(342, 4) + sqrt(342, 4)", "2 * sqrt(342, 4)");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestRootAddition03()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Root Rule");
            Check("sqrt(342 * 12 / 313.213 -21, 4) + sqrt(342 * 12 / 313.213 -21, 4)", "2 * sqrt(342 * 12 / 313.213 -21, 4)");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestRootAddition04()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Root Rule");
            Check("sqrt(a^2, 2^10) + sqrt(a^2, 2^10)", "2 * sqrt(a^2, 2^10)", "a");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestRootAddition05()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Root Rule", "Basic Rule");
            Check("2 * sqrt(14, 2) + sqrt(14, 2)", "(2 + 1) * sqrt(14, 2)");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestRootAddition06()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Root Rule", "Basic Rule");
            Check("sqrt(14, 2) * 2 + sqrt(14, 2)", "(2 + 1) * sqrt(14, 2)");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestRootAddition07()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Root Rule", "Basic Rule");
            Check("sqrt(14, 2) + 2 * sqrt(14, 2)", "(1 + 2) * sqrt(14, 2)");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestRootAddition08()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Root Rule", "Basic Rule");
            Check("sqrt(14, 2) + sqrt(14, 2) * 2", "(1 + 2) * sqrt(14, 2)");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestRootAddition09()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Root Rule", "Basic Rule");
            Check("3 * sqrt(14, 2) + 2 * sqrt(14, 2)", "(3 + 2) * sqrt(14, 2)");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestRootAddition10()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Root Rule", "Basic Rule");
            Check("sqrt(14, 2) * 3 + 2 * sqrt(14, 2)", "(3 + 2) * sqrt(14, 2)");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestRootAddition11()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Root Rule", "Basic Rule");
            Check("3 * sqrt(14, 2) + sqrt(14, 2) * 2", "(3 + 2) * sqrt(14, 2)");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestRootAddition12()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Root Rule", "Basic Rule");
            Check("sqrt(14, 2) * 3 + sqrt(14, 2) * 2", "(3 + 2) * sqrt(14, 2)");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestRootSubtraction01()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Root Rule");
            Check("sqrt(5) - sqrt(5)", "0");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestRootSubtraction02()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Root Rule");
            Check("sqrt(342, 4) - sqrt(342, 4)", "0");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestRootSubtraction03()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Root Rule");
            Check("sqrt(342 * 12 / 313.213 -21, 4) - sqrt(342 * 12 / 313.213 -21, 4)", "0");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestRootSubtraction04()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Root Rule");
            Check("sqrt(a^2, 2^10) - sqrt(a^2, 2^10)", "0", "a");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestRootSubtraction05()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Root Rule", "Basic Rule");
            Check("2 * sqrt(14, 2) - sqrt(14, 2)", "(2 - 1) * sqrt(14, 2)");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestRootSubtraction06()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Root Rule", "Basic Rule");
            Check("sqrt(14, 2) * 2 - sqrt(14, 2)", "(2 - 1) * sqrt(14, 2)");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestRootSubtraction07()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Root Rule", "Basic Rule");
            Check("sqrt(14, 2) - 2 * sqrt(14, 2)", "(1 - 2) * sqrt(14, 2)");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestRootSubtraction08()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Root Rule", "Basic Rule");
            Check("sqrt(14, 2) - sqrt(14, 2) * 2", "(1 - 2) * sqrt(14, 2)");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestRootSubtraction09()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Root Rule", "Basic Rule");
            Check("3 * sqrt(14, 2) - 2 * sqrt(14, 2)", "(3 - 2) * sqrt(14, 2)");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestRootSubtraction10()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Root Rule", "Basic Rule");
            Check("sqrt(14, 2) * 3 - 2 * sqrt(14, 2)", "(3 - 2) * sqrt(14, 2)");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestRootSubtraction11()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Root Rule", "Basic Rule");
            Check("3 * sqrt(14, 2) - sqrt(14, 2) * 2", "(3 - 2) * sqrt(14, 2)");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestRootSubtraction12()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Root Rule", "Basic Rule");
            Check("sqrt(14, 2) * 3 - sqrt(14, 2) * 2", "(3 - 2) * sqrt(14, 2)");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestRootMultiplication01()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Root Rule", "Basic Rule");
            Check("sqrt(14, 2) * sqrt(14, 2)", "sqrt(14 * 14, 2)");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestRootMultiplication02()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Root Rule", "Basic Rule");
            Check("sqrt(144 ^ 321 - (3/2), 2) * sqrt(42, 2)", "sqrt((144 ^ 321 - (3/2)) * 42, 2)");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        //[TestMethod]
        //public void TestRootMultiplication03()
        //{
        //    TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Root Rule", "Basic Rule");
        //    Check("sqrt(81, 2) * sqrt(27, 3)", "sqrt(81^3 * 27^2, 2 * 3)");
        //    TransformationKnowledgeBase.RemoveKnowledgeFilters();
        //}

        [TestMethod]
        public void TestRootMultiplication04()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Root Rule", "Basic Rule", "Resolve");
            Check("sqrt(81, 2) * sqrt(81, 3)", "sqrt(531441 * 6561, 6)");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestRootDivision01()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Root Rule", "Basic Rule");
            Check("sqrt(5) / sqrt(12)", "sqrt(5 / 12)");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestRootDivision02()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Root Rule");
            Check("sqrt(4,3) / sqrt(3, 4)", "sqrt(4^3 / 3^4, 3 * 4)");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void TestRootDivision03()
        {
            TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Root Rule");
            Check("sqrt(4 / 342 * 40 ^3 , 3) / sqrt(43252, 3)", "sqrt((4 / 342 * 40 ^3) / 43252, 3)");
            TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void SimplifyTerm001()
        {
            //for(int i = 0; i < 100; i++)
            Check(
                "a^3 / a^2",
                "a",
                "a");
        }

        [Timeout(15000)]
        [TestMethod]
        public void SimplifyTerm002()
        {
            //TransformationKnowledgeBase.SuppressKnowledgeWithWhitelist("Power Rule Pow to Root", "Resolve", "Root Rule");
            Check(
                "sqrt(a^2, 3) / a^(1/2)",
                "sqrt(a, 6)",
                "a");
            //TransformationKnowledgeBase.RemoveKnowledgeFilters();
        }

        [TestMethod]
        public void SimplifyTerm003()
        {
            Check(
                "(sqrt(a^2, 4) + b) / sqrt(a)",
                "b / sqrt(a)  + 1",
                "a", "b");
        }

        [TestMethod]
        public void SimplifyTerm004()
        {
            Check(
                "e^x + 1 / e^-x",
                "2e^x",
                "e", "x");
        }

        [TestMethod]
        public void SimplifyTerm005()
        {
            Check(
                "a^(x-2)/a^4",
                "a^(x-6)",
                "a", "x");
        }

        [TestMethod]
        public void SimplifyTerm006()
        {
            Check(
                "x^3/x^-2",
                "x^5",
                "x");
        }

        [TestMethod]
        public void SimplifyTerm007()
        {
            Check(
                "(4a^2+12ab+9b^2)/(4a^2-9b^2)",
                "1/(2a-3b)",
                "a", "b");
        }
    }
}
