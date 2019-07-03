using System.Linq;
using De.Markellus.Maths.Core.TermEngine.TermParsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace De.Markellus.Math.Tests.Core.TermEngine.TermParsing
{
    [TestClass]
    public class MathExpressionTokenizerTests
    {
        [TestMethod]
        public void TestTokenizer()
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

            // --------------------------------------------------------------

            arrExpected = new Token[]
            {
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Number, "0"),
                new Token(TokenType.WhiteSpace, " "),
                new Token(TokenType.Operator, "-"),
                new Token(TokenType.WhiteSpace, " "),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Number, "0"),
            };
            arrResult = MathExpressionTokenizer.Default.Tokenize("10 - 20").ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));

            // --------------------------------------------------------------

            arrExpected = new Token[]
            {
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Number, "0"),
                new Token(TokenType.WhiteSpace, " "),
                new Token(TokenType.Operator, "-"),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.WhiteSpace, " "),
                new Token(TokenType.Number, "0"),
            };
            arrResult = MathExpressionTokenizer.Default.Tokenize("10 -2 0").ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));

            // --------------------------------------------------------------

            arrExpected = new Token[]
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
            arrResult = MathExpressionTokenizer.Default.Tokenize("sqrt(9*9) = (9 )").ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));

            // --------------------------------------------------------------

            arrExpected = new Token[]
            {
                new Token(TokenType.Function, "sqrt"),
                new Token(TokenType.Parenthesis, "("),
                new Token(TokenType.Number, "9"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Number, "9"),
                new Token(TokenType.Parenthesis, ")"),
                new Token(TokenType.Operator, "="),
                new Token(TokenType.Parenthesis, "("),
                new Token(TokenType.Number, "9"),
                new Token(TokenType.Parenthesis, ")"),
            };
            arrResult = MathExpressionTokenizer.Default.Tokenize("sqrt(9*9) = (9 )", true).ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));

            // --------------------------------------------------------------

            arrExpected = new Token[]
            {
                new Token(TokenType.Number, "10"),
                new Token(TokenType.Operator, "-"),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Number, "0"),
            };
            arrResult = MathExpressionTokenizer.Default.Tokenize("10 -2 0", true).ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));

            // --------------------------------------------------------------

            MathExpressionTokenizer.Default.RegisterToken(TokenType.Variable, "x");
            MathExpressionTokenizer.Default.RegisterToken(TokenType.Variable, "y");

            arrExpected = new Token[]
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
            arrResult = MathExpressionTokenizer.Default.Tokenize("253 * x-(tan(5 5) 3) / y", true).ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));

            MathExpressionTokenizer.Default.UnregisterToken("x");
            MathExpressionTokenizer.Default.UnregisterToken("y");

            // --------------------------------------------------------------

            arrExpected = new Token[]
            {
                new Token(TokenType.Number, "1.2042"),
                new Token(TokenType.Operator, "-"),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Number, "0"),
            };
            arrResult = MathExpressionTokenizer.Default.Tokenize("1.2042 -2 0", true).ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));

            // --------------------------------------------------------------

            arrExpected = new Token[]
            {
                new Token(TokenType.Number, "-1.2042"),
                new Token(TokenType.Operator, "-"),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Number, "0"),
            };
            arrResult = MathExpressionTokenizer.Default.Tokenize("-1.2042 -2 0", true).ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));

            // --------------------------------------------------------------

            arrExpected = new Token[]
            {
                new Token(TokenType.Number, "1.2042"),
                new Token(TokenType.Operator, "-"),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Number, "0"),
            };
            arrResult = MathExpressionTokenizer.Default.Tokenize("+1.2042 -2 0", true).ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));

            // --------------------------------------------------------------

            arrExpected = new Token[]
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
            arrResult = MathExpressionTokenizer.Default.Tokenize("+1.2042 --2 \t\t* -0 +-12\t -   + 2", true).ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));

            // --------------------------------------------------------------

            arrExpected = new Token[]
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
            arrResult = MathExpressionTokenizer.Default.Tokenize("(3 + 4)(5 - 6)", true).ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));

            // --------------------------------------------------------------

            arrExpected = new Token[]
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
            arrResult = MathExpressionTokenizer.Default.Tokenize("1 + 2 - 3*4 + 5^6^7*8 - 9", true).ToArray();
            var arrResultT = MathExpressionTokenizer.Default.Tokenize("1 + 2 - 3 4 + 5^6^7*8 - 9", true).ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));
            Assert.IsTrue(arrResultT.SequenceEqual(arrResult));

            // --------------------------------------------------------------

            arrExpected = new Token[]
            {
                new Token(TokenType.Number, "5"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Number, "-5"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Number, "5"),
            };
            arrResult = MathExpressionTokenizer.Default.Tokenize("5 +-5+ 5", true).ToArray();
            Assert.IsTrue(arrResult.SequenceEqual(arrExpected));
        }
    }
}
