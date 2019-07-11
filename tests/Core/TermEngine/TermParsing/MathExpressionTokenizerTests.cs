using System.Linq;
using De.Markellus.Maths.Core.TermEngine.TermParsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace De.Markellus.Math.Tests.Core.TermEngine.TermParsing
{
    [TestClass]
    public class MathExpressionTokenizerTests
    {
        [TestMethod]
        public void TestTokenizer01()
        {
            var arrExpected = new Token[]
            {
                new Token(TokenType.Number, "1"),
                new Token(TokenType.WhiteSpace, " "),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.WhiteSpace, " "),
                new Token(TokenType.Number, "1"),
                new Token(TokenType.WhiteSpace, " "),
                new Token(TokenType.Operator, "="),
                new Token(TokenType.WhiteSpace, " "),
                new Token(TokenType.Number, "2"),
            };
            var arrResult = MathExpressionTokenizer.Default.Tokenize("1 + 1 = 2").ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));

        }

        [TestMethod]
        public void TestTokenizer02()
        {

            var arrExpected = new Token[]
            {
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Number, "0"),
                new Token(TokenType.WhiteSpace, " "),
                new Token(TokenType.Operator, "-"),
                new Token(TokenType.WhiteSpace, " "),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Number, "0"),
            };
            var arrResult = MathExpressionTokenizer.Default.Tokenize("10 - 20").ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));

        }

        [TestMethod]
        public void TestTokenizer03()
        {
            var arrExpected = new Token[]
            {
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Number, "0"),
                new Token(TokenType.WhiteSpace, " "),
                new Token(TokenType.Operator, "-"),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.WhiteSpace, " "),
                new Token(TokenType.Number, "0"),
            };
            var arrResult = MathExpressionTokenizer.Default.Tokenize("10 -2 0").ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));
        }

        [TestMethod]
        public void TestTokenizer04()
        {
            var arrExpected = new Token[]
            {
                new Token(TokenType.Function, "sqrt"),
                new Token(TokenType.Parenthesis, "("),
                new Token(TokenType.Number, "9"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Number, "9"),
                new Token(TokenType.Parenthesis, ")"),
                new Token(TokenType.WhiteSpace, " "),
                new Token(TokenType.Operator, "="),
                new Token(TokenType.WhiteSpace, " "),
                new Token(TokenType.Parenthesis, "("),
                new Token(TokenType.Number, "9"),
                new Token(TokenType.WhiteSpace, " "),
                new Token(TokenType.Parenthesis, ")"),
            };
            var arrResult = MathExpressionTokenizer.Default.Tokenize("sqrt(9*9) = (9 )").ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));
        }

        [TestMethod]
        public void TestTokenizer05()
        {
            var arrExpected = new Token[]
            {
                new Token(TokenType.Function, "sqrt"),
                new Token(TokenType.Parenthesis, "("),
                new Token(TokenType.Number, "9"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Number, "9"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Function, "sqrt"),
                new Token(TokenType.Parenthesis, "("),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.ArgumentSeparator, ","),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Parenthesis, ")"),
                new Token(TokenType.ArgumentSeparator, ","),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Parenthesis, ")"),
                new Token(TokenType.Operator, "="),
                new Token(TokenType.Parenthesis, "("),
                new Token(TokenType.Number, "9"),
                new Token(TokenType.Parenthesis, ")"),
            };
            var arrResult = MathExpressionTokenizer.Default.Tokenize("sqrt(9*9 + sqrt(2)) = (9 )", true).ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));
        }

        [TestMethod]
        public void TestTokenizer05x1()
        {
            var arrExpected = new Token[]
            {
                new Token(TokenType.Function, "sqrt"),
                new Token(TokenType.Parenthesis, "("),
                new Token(TokenType.Number, "9"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Number, "9"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Function, "sqrt"),
                new Token(TokenType.Parenthesis, "("),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.ArgumentSeparator, ","),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Parenthesis, ")"),
                new Token(TokenType.ArgumentSeparator, ","),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Parenthesis, ")"),
                new Token(TokenType.Operator, "="),
                new Token(TokenType.Parenthesis, "("),
                new Token(TokenType.Number, "9"),
                new Token(TokenType.Parenthesis, ")"),
            };
            var arrResult = MathExpressionTokenizer.Default.Tokenize("sqrt(9*9 + sqrt(2, 2), 2) = (9 )", true).ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));
        }

        [TestMethod]
        public void TestTokenizer06()
        {
            var arrExpected = new Token[]
            {
                new Token(TokenType.Function, "sqrt"),
                new Token(TokenType.Parenthesis, "("),
                new Token(TokenType.Number, "9"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Number, "9"),
                new Token(TokenType.ArgumentSeparator, ","),
                new Token(TokenType.Number, "9"),
                new Token(TokenType.Parenthesis, ")"),
                new Token(TokenType.Operator, "="),
                new Token(TokenType.Parenthesis, "("),
                new Token(TokenType.Number, "9"),
                new Token(TokenType.Parenthesis, ")"),
            };
            var arrResult = MathExpressionTokenizer.Default.Tokenize("sqrt(9*9, 9) = (9 )", true).ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));
        }

        [TestMethod]
        public void TestTokenizer07()
        {
            var arrExpected = new Token[]
            {
                new Token(TokenType.Number, "10"),
                new Token(TokenType.Operator, "-"),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Number, "0"),
            };
            var arrResult = MathExpressionTokenizer.Default.Tokenize("10 -2 0", true).ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));
        }

        [TestMethod]
        public void TestTokenizer08()
        {
            MathExpressionTokenizer.Default.RegisterToken(TokenType.Variable, "x");
            MathExpressionTokenizer.Default.RegisterToken(TokenType.Variable, "y");

            var arrExpected = new Token[]
            {
                new Token(TokenType.Number, "253"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Variable, "x"),
                new Token(TokenType.Operator, "-"),
                new Token(TokenType.Parenthesis, "("),
                new Token(TokenType.Function, "tan"),
                new Token(TokenType.Parenthesis, "("),
                new Token(TokenType.Number, "5"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Number, "5"),
                new Token(TokenType.Parenthesis, ")"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Number, "3"),
                new Token(TokenType.Parenthesis, ")"),
                new Token(TokenType.Operator, "/"),
                new Token(TokenType.Variable, "y"),
            };
            var arrResult = MathExpressionTokenizer.Default.Tokenize("253 * x-(tan(5 5) 3) / y", true).ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));

            MathExpressionTokenizer.Default.UnregisterToken("x");
            MathExpressionTokenizer.Default.UnregisterToken("y");
        }

        [TestMethod]
        public void TestTokenizer09()
        {
            var arrExpected = new Token[]
            {
                new Token(TokenType.Number, "1.2042"),
                new Token(TokenType.Operator, "-"),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Number, "0"),
            };
            var arrResult = MathExpressionTokenizer.Default.Tokenize("1.2042 -2 0", true).ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));
        }

        [TestMethod]
        public void TestTokenizer10()
        {
            var arrExpected = new Token[]
            {
                new Token(TokenType.Number, "-1.2042"),
                new Token(TokenType.Operator, "-"),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Number, "0"),
            };
            var arrResult = MathExpressionTokenizer.Default.Tokenize("-1.2042 -2 0", true).ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));
        }

        [TestMethod]
        public void TestTokenizer11()
        {
            var arrExpected = new Token[]
            {
                new Token(TokenType.Number, "1.2042"),
                new Token(TokenType.Operator, "-"),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Number, "0"),
            };
            var arrResult = MathExpressionTokenizer.Default.Tokenize("+1.2042 -2 0", true).ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));
        }

        [TestMethod]
        public void TestTokenizer12()
        {
            var arrExpected = new Token[]
            {
                new Token(TokenType.Number, "1.2042"),
                new Token(TokenType.Operator, "-"),
                new Token(TokenType.Number, "-2"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Number, "-0"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Number, "-12"),
                new Token(TokenType.Operator, "-"),
                new Token(TokenType.Number, "2"),
            };
            var arrResult = MathExpressionTokenizer.Default.Tokenize("+1.2042 --2 \t\t* -0 +-12\t -   + 2", true)
                .ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));
        }

        [TestMethod]
        public void TestTokenizer13()
        {
            var arrExpected = new Token[]
            {
                new Token(TokenType.Parenthesis, "("),
                new Token(TokenType.Number, "3"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Number, "4"),
                new Token(TokenType.Parenthesis, ")"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Parenthesis, "("),
                new Token(TokenType.Number, "5"),
                new Token(TokenType.Operator, "-"),
                new Token(TokenType.Number, "6"),
                new Token(TokenType.Parenthesis, ")"),
            };
            var arrResult = MathExpressionTokenizer.Default.Tokenize("(3 + 4)(5 - 6)", true).ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));
        }

        [TestMethod]
        public void TestTokenizer14()
        {
            var arrExpected = new Token[]
            {
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Operator, "-"),
                new Token(TokenType.Number, "3"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Number, "4"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Number, "5"),
                new Token(TokenType.Operator, "^"),
                new Token(TokenType.Number, "6"),
                new Token(TokenType.Operator, "^"),
                new Token(TokenType.Number, "7"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Number, "8"),
                new Token(TokenType.Operator, "-"),
                new Token(TokenType.Number, "9"),
            };
            var arrResult = MathExpressionTokenizer.Default.Tokenize("1 + 2 - 3*4 + 5^6^7*8 - 9", true).ToArray();
            var arrResultT = MathExpressionTokenizer.Default.Tokenize("1 + 2 - 3 4 + 5^6^7*8 - 9", true).ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));
            Assert.IsTrue(arrResultT.SequenceEqual(arrResult));
        }

        [TestMethod]
        public void TestTokenizer15()
        {
            var arrExpected = new Token[]
            {
                new Token(TokenType.Number, "5"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Number, "-5"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Number, "5"),
            };
            var arrResult = MathExpressionTokenizer.Default.Tokenize("5 +-5+ 5", true).ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));
        }

        [TestMethod]
        public void TestTokenizer16()
        {
            var arrExpected = new Token[]
            {
                new Token(TokenType.Function, "sqrt"),
                new Token(TokenType.Parenthesis, "("),
                new Token(TokenType.Variable, "a"),
                new Token(TokenType.Operator, "^"),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.ArgumentSeparator, ","),
                new Token(TokenType.Number, "3"),
                new Token(TokenType.Parenthesis, ")"),
                new Token(TokenType.Operator, "/"),
                new Token(TokenType.Variable, "a"),
                new Token(TokenType.Operator, "^"),
                new Token(TokenType.Parenthesis, "("),
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Operator, "/"),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Parenthesis, ")"),
            };
            MathExpressionTokenizer.Default.RegisterToken(TokenType.Variable, "a");
            var arrResult = MathExpressionTokenizer.Default.Tokenize("sqrt(a^2, 3) / a^(1/2)", true).ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));
            MathExpressionTokenizer.Default.UnregisterToken("a");

        }

        [TestMethod]
        public void TestTokenizer17()
        {
            var arrExpected = new Token[]
            {
                new Token(TokenType.Parenthesis, "("),
                new Token(TokenType.Number, "-21"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Number, "4"),
                new Token(TokenType.Parenthesis, ")"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Number, "10"),
            };
            var arrResult = MathExpressionTokenizer.Default.Tokenize("(- 21 + 4) + 10", true).ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));
        }
    }
}
