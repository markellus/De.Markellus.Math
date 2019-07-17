﻿using System.Linq;
using De.Markellus.Maths.Core.TermEngine.TermParsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace De.Markellus.Math.Tests.Core.TermEngine.TermParsing
{
    [TestClass]
    public class ShuntingYardParserTests
    {
        [TestMethod]
        public void TestShuntingYard01()
        {
            var arrExpected = new[]
            {
                new Token(TokenType.Number, "3"),
                new Token(TokenType.Number, "4"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Number, "5"),
                new Token(TokenType.Number, "6"),
                new Token(TokenType.Operator, "-"),
                new Token(TokenType.Operator, "*"),
            };
            var arrResult = ShuntingYardParser.InfixToRpn("(3 + 4)(5 - 6)").ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));

        }

        [TestMethod]
        public void TestShuntingYard02()
        {

            var arrExpected = new[]
            {
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Number, "3"),
                new Token(TokenType.Number, "4"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Operator, "-"),
                new Token(TokenType.Number, "5"),
                new Token(TokenType.Number, "6"),
                new Token(TokenType.Number, "7"),
                new Token(TokenType.Operator, "^"),
                new Token(TokenType.Operator, "^"),
                new Token(TokenType.Number, "8"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Number, "9"),
                new Token(TokenType.Operator, "-"),
            };
            var arrResult = ShuntingYardParser.InfixToRpn("1 + 2 - 3*4 + 5^6^7*8 - 9").ToArray();
            var arrResultT = ShuntingYardParser.InfixToRpn("1 + 2 - 3 4 + 5^6^7*8 - 9").ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));
            Assert.IsTrue(arrResult.SequenceEqual(arrResultT));

        }

        [TestMethod]
        public void TestShuntingYard03()
        {

            var arrExpected = new Token[]
            {
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Number, "5"),
                new Token(TokenType.Function, "ln"),
                new Token(TokenType.Number, "8"),
                new Token(TokenType.Function, "exp"),
                new Token(TokenType.Operator, "-"),
                new Token(TokenType.Function, "sin"),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Operator, "^"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Function, "cos"),
            };
            var arrResult = ShuntingYardParser.InfixToRpn("cos(1 + sin(ln(5) - exp(8))^2)").ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));

        }

        [TestMethod]
        public void TestShuntingYard04()
        {

            var arrExpected = new Token[]
            {
                new Token(TokenType.Number, "3"),
                new Token(TokenType.Number, "4"),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Number, "5"),
                new Token(TokenType.Operator, "-"),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Number, "3"),
                new Token(TokenType.Operator, "^"),
                new Token(TokenType.Operator, "^"),
                new Token(TokenType.Operator, "/"),
                new Token(TokenType.Operator, "+"),
            };
            var arrResult = ShuntingYardParser.InfixToRpn("3 + 4 * 2 / ( 1 - 5 ) ^ 2 ^ 3").ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));

        }

        [TestMethod]
        public void TestShuntingYard05()
        {

            var arrExpected = new Token[]
            {
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Number, "3"),
                new Token(TokenType.Function, "max"),
                new Token(TokenType.Number, "3"),
                new Token(TokenType.Operator, "/"),
                new Token(TokenType.Constant, "π"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Function, "sin"),
            };
            var arrResult = ShuntingYardParser.InfixToRpn("sin ( max ( 2, 3 ) / 3 * π )").ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));

        }

        [TestMethod]
        public void TestShuntingYard06()
        {

            var arrExpected = new Token[]
            {
                new Token(TokenType.Number, "5"),
                new Token(TokenType.Number, "-5"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Number, "5"),
                new Token(TokenType.Operator, "+"),
            };
            var arrResult = ShuntingYardParser.InfixToRpn("5 +-5+ 5").ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));
        }

        [TestMethod]
        public void TestShuntingYard07()
        {

            var arrExpected = new Token[]
            {
                new Token(TokenType.Number, "45"),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Operator, "^"),
                new Token(TokenType.Number, "54"),
                new Token(TokenType.Function, "sqrt"),
            };

            MathExpressionTokenizer.Default.RegisterToken(TokenType.Variable, "a");
            var arrResult = ShuntingYardParser.InfixToRpn("sqrt(45 ^ 2, 54)").ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));
            MathExpressionTokenizer.Default.UnregisterToken("a");
        }
    }
}