using System.Linq;
using De.Markellus.Maths.Internals.TermParsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace De.Markellus.Math.Tests.Internals.TermParsing
{
    [TestClass]
    public class ShuntingYardParserTests
    {
        private Token GenerateTestToken(TokenType tt, string strValue)
        {
            if (tt == TokenType.Variable)
            {
                return MathExpressionTokenizer.GetVariableToken(strValue);
            }
            return MathExpressionTokenizer.GetModifiedToken(MathExpressionTokenizer.GetToken(tt), strValue);
        }

        [TestMethod]
        public void TestShuntingYard01()
        {
            var arrExpected = new[]
            {
                GenerateTestToken(TokenType.Number, "3"),
                GenerateTestToken(TokenType.Number, "4"),
                GenerateTestToken(TokenType.Operator, "+"),
                GenerateTestToken(TokenType.Number, "5"),
                GenerateTestToken(TokenType.Number, "6"),
                GenerateTestToken(TokenType.Operator, "-"),
                GenerateTestToken(TokenType.Operator, "*"),
            };
            var arrResult = ShuntingYardParser.InfixToRpn("(3 + 4)(5 - 6)").ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));

        }

        [TestMethod]
        public void TestShuntingYard02()
        {

            var arrExpected = new[]
            {
                GenerateTestToken(TokenType.Number, "1"),
                GenerateTestToken(TokenType.Number, "2"),
                GenerateTestToken(TokenType.Operator, "+"),
                GenerateTestToken(TokenType.Number, "3"),
                GenerateTestToken(TokenType.Number, "4"),
                GenerateTestToken(TokenType.Operator, "*"),
                GenerateTestToken(TokenType.Operator, "-"),
                GenerateTestToken(TokenType.Number, "5"),
                GenerateTestToken(TokenType.Number, "6"),
                GenerateTestToken(TokenType.Number, "7"),
                GenerateTestToken(TokenType.Operator, "^"),
                GenerateTestToken(TokenType.Operator, "^"),
                GenerateTestToken(TokenType.Number, "8"),
                GenerateTestToken(TokenType.Operator, "*"),
                GenerateTestToken(TokenType.Operator, "+"),
                GenerateTestToken(TokenType.Number, "9"),
                GenerateTestToken(TokenType.Operator, "-"),
            };
            var arrResult = ShuntingYardParser.InfixToRpn("1 + 2 - 3*4 + 5^6^7*8 - 9").ToArray();
            var arrResultT = ShuntingYardParser.InfixToRpn("1 + 2 - 3 4 + 5^6^7*8 - 9").ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));
            Assert.IsTrue(arrResult.SequenceEqual(arrResultT));

        }

        [TestMethod]
        public void TestShuntingYard03()
        {

            var arrExpected = new[]
            {
                GenerateTestToken(TokenType.Number, "1"),
                GenerateTestToken(TokenType.Number, "5"),
                GenerateTestToken(TokenType.Function, "ln"),
                GenerateTestToken(TokenType.Number, "8"),
                GenerateTestToken(TokenType.Function, "exp"),
                GenerateTestToken(TokenType.Operator, "-"),
                GenerateTestToken(TokenType.Function, "sin"),
                GenerateTestToken(TokenType.Number, "2"),
                GenerateTestToken(TokenType.Operator, "^"),
                GenerateTestToken(TokenType.Operator, "+"),
                GenerateTestToken(TokenType.Function, "cos"),
            };
            var arrResult = ShuntingYardParser.InfixToRpn("cos(1 + sin(ln(5) - exp(8))^2)").ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));

        }

        [TestMethod]
        public void TestShuntingYard04()
        {

            var arrExpected = new[]
            {
                GenerateTestToken(TokenType.Number, "3"),
                GenerateTestToken(TokenType.Number, "4"),
                GenerateTestToken(TokenType.Number, "2"),
                GenerateTestToken(TokenType.Operator, "*"),
                GenerateTestToken(TokenType.Number, "1"),
                GenerateTestToken(TokenType.Number, "5"),
                GenerateTestToken(TokenType.Operator, "-"),
                GenerateTestToken(TokenType.Number, "2"),
                GenerateTestToken(TokenType.Number, "3"),
                GenerateTestToken(TokenType.Operator, "^"),
                GenerateTestToken(TokenType.Operator, "^"),
                GenerateTestToken(TokenType.Operator, "/"),
                GenerateTestToken(TokenType.Operator, "+"),
            };
            var arrResult = ShuntingYardParser.InfixToRpn("3 + 4 * 2 / ( 1 - 5 ) ^ 2 ^ 3").ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));

        }

        [TestMethod]
        public void TestShuntingYard05()
        {

            var arrExpected = new[]
            {
                GenerateTestToken(TokenType.Number, "2"),
                GenerateTestToken(TokenType.Number, "3"),
                GenerateTestToken(TokenType.Function, "max"),
                GenerateTestToken(TokenType.Number, "3"),
                GenerateTestToken(TokenType.Operator, "/"),
                GenerateTestToken(TokenType.Constant, "π"),
                GenerateTestToken(TokenType.Operator, "*"),
                GenerateTestToken(TokenType.Function, "sin"),
            };
            var arrResult = ShuntingYardParser.InfixToRpn("sin ( max ( 2, 3 ) / 3 * π )").ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));

        }

        [TestMethod]
        public void TestShuntingYard06()
        {

            var arrExpected = new[]
            {
                GenerateTestToken(TokenType.Number, "5"),
                GenerateTestToken(TokenType.Number, "-5"),
                GenerateTestToken(TokenType.Operator, "+"),
                GenerateTestToken(TokenType.Number, "5"),
                GenerateTestToken(TokenType.Operator, "+"),
            };
            var arrResult = ShuntingYardParser.InfixToRpn("5 +-5+ 5").ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));
        }

        [TestMethod]
        public void TestShuntingYard07()
        {

            var arrExpected = new[]
            {
                GenerateTestToken(TokenType.Number, "45"),
                GenerateTestToken(TokenType.Number, "2"),
                GenerateTestToken(TokenType.Operator, "^"),
                GenerateTestToken(TokenType.Number, "54"),
                GenerateTestToken(TokenType.Function, "sqrt"),
            };

            var arrResult = ShuntingYardParser.InfixToRpn("sqrt(45 ^ 2, 54)", "a").ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));
        }
    }
}
