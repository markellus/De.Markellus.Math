using System.Linq;
using De.Markellus.Maths.Internals.TermParsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace De.Markellus.Math.Tests.Internals.TermParsing
{
    [TestClass]
    public class MathExpressionTokenizerTests
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
        public void TestTokenizer01()
        {
            var arrExpected = new[]
            {
                GenerateTestToken(TokenType.Number, "1"),
                GenerateTestToken(TokenType.WhiteSpace, " "),
                GenerateTestToken(TokenType.Operator, "+"),
                GenerateTestToken(TokenType.WhiteSpace, " "),
                GenerateTestToken(TokenType.Number, "1"),
                GenerateTestToken(TokenType.WhiteSpace, " "),
                GenerateTestToken(TokenType.Operator, "="),
                GenerateTestToken(TokenType.WhiteSpace, " "),
                GenerateTestToken(TokenType.Number, "2"),
            };
            var arrResult = MathExpressionTokenizer.Tokenize("1 + 1 = 2").ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));

        }

        [TestMethod]
        public void TestTokenizer02()
        {
            var arrExpected = new[]
            {
                    GenerateTestToken(TokenType.Number, "1"),
                    GenerateTestToken(TokenType.Number, "0"),
                    GenerateTestToken(TokenType.WhiteSpace, " "),
                    GenerateTestToken(TokenType.Operator, "-"),
                    GenerateTestToken(TokenType.WhiteSpace, " "),
                    GenerateTestToken(TokenType.Number, "2"),
                    GenerateTestToken(TokenType.Number, "0"),
            };
            var arrResult = MathExpressionTokenizer.Tokenize("10 - 20").ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));
        }

        [TestMethod]
        public void TestTokenizer03()
        {
            var arrExpected = new[]
            {
                    GenerateTestToken(TokenType.Number, "1"),
                    GenerateTestToken(TokenType.Number, "0"),
                    GenerateTestToken(TokenType.WhiteSpace, " "),
                    GenerateTestToken(TokenType.Operator, "-"),
                    GenerateTestToken(TokenType.Number, "2"),
                    GenerateTestToken(TokenType.WhiteSpace, " "),
                    GenerateTestToken(TokenType.Number, "0"),
            };
            var arrResult = MathExpressionTokenizer.Tokenize("10 -2 0").ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));
        }

        [TestMethod]
        public void TestTokenizer04()
        {
            var arrExpected = new[]
            {
                    GenerateTestToken(TokenType.Function, "sqrt"),
                    GenerateTestToken(TokenType.Parenthesis, "("),
                    GenerateTestToken(TokenType.Number, "9"),
                    GenerateTestToken(TokenType.Operator, "*"),
                    GenerateTestToken(TokenType.Number, "9"),
                    GenerateTestToken(TokenType.Parenthesis, ")"),
                    GenerateTestToken(TokenType.WhiteSpace, " "),
                    GenerateTestToken(TokenType.Operator, "="),
                    GenerateTestToken(TokenType.WhiteSpace, " "),
                    GenerateTestToken(TokenType.Parenthesis, "("),
                    GenerateTestToken(TokenType.Number, "9"),
                    GenerateTestToken(TokenType.WhiteSpace, " "),
                    GenerateTestToken(TokenType.Parenthesis, ")"),
            };
            var arrResult = MathExpressionTokenizer.Tokenize("sqrt(9*9) = (9 )").ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));
        }

        [TestMethod]
        public void TestTokenizer05()
        {
            var arrExpected = new[]
            {
                    GenerateTestToken(TokenType.Function, "sqrt"),
                    GenerateTestToken(TokenType.Parenthesis, "("),
                    GenerateTestToken(TokenType.Number, "9"),
                    GenerateTestToken(TokenType.Operator, "*"),
                    GenerateTestToken(TokenType.Number, "9"),
                    GenerateTestToken(TokenType.Operator, "+"),
                    GenerateTestToken(TokenType.Function, "sqrt"),
                    GenerateTestToken(TokenType.Parenthesis, "("),
                    GenerateTestToken(TokenType.Number, "2"),
                    GenerateTestToken(TokenType.ArgumentSeparator, ","),
                    GenerateTestToken(TokenType.Number, "2"),
                    GenerateTestToken(TokenType.Parenthesis, ")"),
                    GenerateTestToken(TokenType.ArgumentSeparator, ","),
                    GenerateTestToken(TokenType.Number, "2"),
                    GenerateTestToken(TokenType.Parenthesis, ")"),
                    GenerateTestToken(TokenType.Operator, "="),
                    GenerateTestToken(TokenType.Parenthesis, "("),
                    GenerateTestToken(TokenType.Number, "9"),
                    GenerateTestToken(TokenType.Parenthesis, ")"),
            };
            var arrResult = MathExpressionTokenizer.Tokenize("sqrt(9*9 + sqrt(2)) = (9 )", true).ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));
        }

        [TestMethod]
        public void TestTokenizer06()
        {
            var arrExpected = new[]
            {
                    GenerateTestToken(TokenType.Function, "sqrt"),
                    GenerateTestToken(TokenType.Parenthesis, "("),
                    GenerateTestToken(TokenType.Number, "9"),
                    GenerateTestToken(TokenType.Operator, "*"),
                    GenerateTestToken(TokenType.Number, "9"),
                    GenerateTestToken(TokenType.ArgumentSeparator, ","),
                    GenerateTestToken(TokenType.Number, "9"),
                    GenerateTestToken(TokenType.Parenthesis, ")"),
                    GenerateTestToken(TokenType.Operator, "="),
                    GenerateTestToken(TokenType.Parenthesis, "("),
                    GenerateTestToken(TokenType.Number, "9"),
                    GenerateTestToken(TokenType.Parenthesis, ")"),
            };
            var arrResult = MathExpressionTokenizer.Tokenize("sqrt(9*9, 9) = (9 )", true).ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));
        }

        [TestMethod]
        public void TestTokenizer07()
        {
            var arrExpected = new[]
            {
                    GenerateTestToken(TokenType.Number, "10"),
                    GenerateTestToken(TokenType.Operator, "-"),
                    GenerateTestToken(TokenType.Number, "2"),
                    GenerateTestToken(TokenType.Operator, "*"),
                    GenerateTestToken(TokenType.Number, "0"),
            };
            var arrResult = MathExpressionTokenizer.Tokenize("10 -2 0", true).ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));
        }

        [TestMethod]
        public void TestTokenizer08()
        {
            var arrExpected = new[]
            {
                    GenerateTestToken(TokenType.Number, "253"),
                    GenerateTestToken(TokenType.Operator, "*"),
                    GenerateTestToken(TokenType.Variable, "x"),
                    GenerateTestToken(TokenType.Operator, "-"),
                    GenerateTestToken(TokenType.Parenthesis, "("),
                    GenerateTestToken(TokenType.Function, "tan"),
                    GenerateTestToken(TokenType.Parenthesis, "("),
                    GenerateTestToken(TokenType.Number, "5"),
                    GenerateTestToken(TokenType.Operator, "*"),
                    GenerateTestToken(TokenType.Number, "5"),
                    GenerateTestToken(TokenType.Parenthesis, ")"),
                    GenerateTestToken(TokenType.Operator, "*"),
                    GenerateTestToken(TokenType.Number, "3"),
                    GenerateTestToken(TokenType.Parenthesis, ")"),
                    GenerateTestToken(TokenType.Operator, "/"),
                    GenerateTestToken(TokenType.Variable, "y"),
            };
            var arrResult = MathExpressionTokenizer.Tokenize("253 * x-(tan(5 5) 3) / y", true, "x", "y").ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));
        }

        [TestMethod]
        public void TestTokenizer09()
        {
            var arrExpected = new[]
            {
                    GenerateTestToken(TokenType.Number, "1.2042"),
                    GenerateTestToken(TokenType.Operator, "-"),
                    GenerateTestToken(TokenType.Number, "2"),
                    GenerateTestToken(TokenType.Operator, "*"),
                    GenerateTestToken(TokenType.Number, "0"),
            };
            var arrResult = MathExpressionTokenizer.Tokenize("1.2042 -2 0", true).ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));
        }

        [TestMethod]
        public void TestTokenizer10()
        {
            var arrExpected = new[]
            {
                    GenerateTestToken(TokenType.Number, "-1.2042"),
                    GenerateTestToken(TokenType.Operator, "-"),
                    GenerateTestToken(TokenType.Number, "2"),
                    GenerateTestToken(TokenType.Operator, "*"),
                    GenerateTestToken(TokenType.Number, "0"),
            };
            var arrResult = MathExpressionTokenizer.Tokenize("-1.2042 -2 0", true).ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));
        }

        [TestMethod]
        public void TestTokenizer11()
        {
            var arrExpected = new[]
            {
                    GenerateTestToken(TokenType.Number, "1.2042"),
                    GenerateTestToken(TokenType.Operator, "-"),
                    GenerateTestToken(TokenType.Number, "2"),
                    GenerateTestToken(TokenType.Operator, "*"),
                    GenerateTestToken(TokenType.Number, "0"),
            };
            var arrResult = MathExpressionTokenizer.Tokenize("+1.2042 -2 0", true).ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));
        }

        [TestMethod]
        public void TestTokenizer12()
        {
            var arrExpected = new[]
            {
                    GenerateTestToken(TokenType.Number, "1.2042"),
                    GenerateTestToken(TokenType.Operator, "-"),
                    GenerateTestToken(TokenType.Number, "-2"),
                    GenerateTestToken(TokenType.Operator, "*"),
                    GenerateTestToken(TokenType.Number, "-0"),
                    GenerateTestToken(TokenType.Operator, "+"),
                    GenerateTestToken(TokenType.Number, "-12"),
                    GenerateTestToken(TokenType.Operator, "-"),
                    GenerateTestToken(TokenType.Number, "2"),
            };
            var arrResult = MathExpressionTokenizer.Tokenize("+1.2042 --2 \t\t* -0 +-12\t -   + 2", true)
                .ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));
        }

        [TestMethod]
        public void TestTokenizer13()
        {
            var arrExpected = new[]
            {
                    GenerateTestToken(TokenType.Parenthesis, "("),
                    GenerateTestToken(TokenType.Number, "3"),
                    GenerateTestToken(TokenType.Operator, "+"),
                    GenerateTestToken(TokenType.Number, "4"),
                    GenerateTestToken(TokenType.Parenthesis, ")"),
                    GenerateTestToken(TokenType.Operator, "*"),
                    GenerateTestToken(TokenType.Parenthesis, "("),
                    GenerateTestToken(TokenType.Number, "5"),
                    GenerateTestToken(TokenType.Operator, "-"),
                    GenerateTestToken(TokenType.Number, "6"),
                    GenerateTestToken(TokenType.Parenthesis, ")"),
            };
            var arrResult = MathExpressionTokenizer.Tokenize("(3 + 4)(5 - 6)", true).ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));
        }

        [TestMethod]
        public void TestTokenizer14()
        {
            var arrExpected = new[]
            {
                    GenerateTestToken(TokenType.Number, "1"),
                    GenerateTestToken(TokenType.Operator, "+"),
                    GenerateTestToken(TokenType.Number, "2"),
                    GenerateTestToken(TokenType.Operator, "-"),
                    GenerateTestToken(TokenType.Number, "3"),
                    GenerateTestToken(TokenType.Operator, "*"),
                    GenerateTestToken(TokenType.Number, "4"),
                    GenerateTestToken(TokenType.Operator, "+"),
                    GenerateTestToken(TokenType.Number, "5"),
                    GenerateTestToken(TokenType.Operator, "^"),
                    GenerateTestToken(TokenType.Number, "6"),
                    GenerateTestToken(TokenType.Operator, "^"),
                    GenerateTestToken(TokenType.Number, "7"),
                    GenerateTestToken(TokenType.Operator, "*"),
                    GenerateTestToken(TokenType.Number, "8"),
                    GenerateTestToken(TokenType.Operator, "-"),
                    GenerateTestToken(TokenType.Number, "9"),
            };
            var arrResult = MathExpressionTokenizer.Tokenize("1 + 2 - 3*4 + 5^6^7*8 - 9", true).ToArray();
            var arrResultT = MathExpressionTokenizer.Tokenize("1 + 2 - 3 4 + 5^6^7*8 - 9", true).ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));
            Assert.IsTrue(arrResultT.SequenceEqual(arrResult));
        }

        [TestMethod]
        public void TestTokenizer15()
        {
            var arrExpected = new[]
            {
                    GenerateTestToken(TokenType.Number, "5"),
                    GenerateTestToken(TokenType.Operator, "+"),
                    GenerateTestToken(TokenType.Number, "-5"),
                    GenerateTestToken(TokenType.Operator, "+"),
                    GenerateTestToken(TokenType.Number, "5"),
            };
            var arrResult = MathExpressionTokenizer.Tokenize("5 +-5+ 5", true).ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));
        }

        [TestMethod]
        public void TestTokenizer16()
        {
            var arrExpected = new[]
            {
                    GenerateTestToken(TokenType.Function, "sqrt"),
                    GenerateTestToken(TokenType.Parenthesis, "("),
                    GenerateTestToken(TokenType.Variable, "a"),
                    GenerateTestToken(TokenType.Operator, "^"),
                    GenerateTestToken(TokenType.Number, "2"),
                    GenerateTestToken(TokenType.ArgumentSeparator, ","),
                    GenerateTestToken(TokenType.Number, "3"),
                    GenerateTestToken(TokenType.Parenthesis, ")"),
                    GenerateTestToken(TokenType.Operator, "/"),
                    GenerateTestToken(TokenType.Variable, "a"),
                    GenerateTestToken(TokenType.Operator, "^"),
                    GenerateTestToken(TokenType.Parenthesis, "("),
                    GenerateTestToken(TokenType.Number, "1"),
                    GenerateTestToken(TokenType.Operator, "/"),
                    GenerateTestToken(TokenType.Number, "2"),
                    GenerateTestToken(TokenType.Parenthesis, ")"),
            };
            var arrResult = MathExpressionTokenizer.Tokenize("sqrt(a^2, 3) / a^(1/2)", true, "a").ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));

        }

        [TestMethod]
        public void TestTokenizer17()
        {
            var arrExpected = new[]
            {
                    GenerateTestToken(TokenType.Parenthesis, "("),
                    GenerateTestToken(TokenType.Number, "-21"),
                    GenerateTestToken(TokenType.Operator, "+"),
                    GenerateTestToken(TokenType.Number, "4"),
                    GenerateTestToken(TokenType.Parenthesis, ")"),
                    GenerateTestToken(TokenType.Operator, "+"),
                    GenerateTestToken(TokenType.Number, "10"),
            };
            var arrResult = MathExpressionTokenizer.Tokenize("(- 21 + 4) + 10", true).ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));
        }

        [TestMethod]
        public void TestTokenizer18()
        {
            var arrExpected = new[]
            {
                GenerateTestToken(TokenType.Function, "sqrt"),
                GenerateTestToken(TokenType.Parenthesis, "("),
                GenerateTestToken(TokenType.Number, "9"),
                GenerateTestToken(TokenType.Operator, "*"),
                GenerateTestToken(TokenType.Number, "9"),
                GenerateTestToken(TokenType.Operator, "+"),
                GenerateTestToken(TokenType.Function, "sqrt"),
                GenerateTestToken(TokenType.Parenthesis, "("),
                GenerateTestToken(TokenType.Number, "2"),
                GenerateTestToken(TokenType.ArgumentSeparator, ","),
                GenerateTestToken(TokenType.Number, "2"),
                GenerateTestToken(TokenType.Parenthesis, ")"),
                GenerateTestToken(TokenType.ArgumentSeparator, ","),
                GenerateTestToken(TokenType.Number, "2"),
                GenerateTestToken(TokenType.Parenthesis, ")"),
                GenerateTestToken(TokenType.Operator, "="),
                GenerateTestToken(TokenType.Parenthesis, "("),
                GenerateTestToken(TokenType.Number, "9"),
                GenerateTestToken(TokenType.Parenthesis, ")"),
            };
            var arrResult = MathExpressionTokenizer.Tokenize("sqrt(9*9 + sqrt(2, 2), 2) = (9 )", true).ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));
        }
    }
}
