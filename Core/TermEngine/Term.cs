using System;
using System.Collections.Generic;
using De.Markellus.Maths.Core.Arithmetic;
using De.Markellus.Maths.Core.TermEngine.Nodes.Base;
using De.Markellus.Maths.Core.TermEngine.Nodes.Implementation;
using De.Markellus.Maths.Core.TermEngine.TermParsing;

namespace De.Markellus.Maths.Core.TermEngine
{
    public class Term
    {
        public TermNode RootNode { get; private set; }

        public Term(string strInfixTerm, MathExpressionTokenizer tokenizer = null)
        {
            IEnumerable<Token> rpnToken = ShuntingYardParser.InfixToRpn(strInfixTerm, tokenizer);
            CreateTermNodes(rpnToken);
        }

        public Term(IEnumerable<Token> token, bool bIsRpn = false)
        {
            IEnumerable<Token> rpnToken;
            if (bIsRpn)
            {
                rpnToken = token;
            }
            else
            {
                rpnToken = ShuntingYardParser.InfixToRpn(token);
            }
            CreateTermNodes(rpnToken);
        }

        public Real Resolve()
        {
            Real result = RootNode.Resolve();
            result.RoundPeriod();
            return result;
        }

        private void CreateTermNodes(IEnumerable<Token> rpnToken)
        {
            Stack<TermNode> stack = new Stack<TermNode>();

            foreach (Token token in rpnToken)
            {
                switch (token.Type)
                {
                    case TokenType.Number:
                        stack.Push(new NumberNode(token.Value));
                        break;
                    case TokenType.Operator:
                        stack.Push(CreateOperatorNode(token, stack));
                        break;
                }
            }

            if (stack.Count != 1)
            {
                throw new ArithmeticException("Unable to parse the given term.");
            }
            RootNode = stack.Pop();
        }

        private TermNode CreateOperatorNode(Token operatorToken, Stack<TermNode> stack)
        {
            switch (operatorToken.Value)
            {
                case "+":
                {
                    TermNode right = stack.Pop();
                    TermNode left = stack.Pop();
                    return new OperatorAddNode(left, right);
                }
                case "-":
                {
                    TermNode right = stack.Pop();
                    TermNode left = stack.Pop();
                    return new OperatorSubtractNode(left, right);
                }
                case "*":
                {
                    TermNode right = stack.Pop();
                    TermNode left = stack.Pop();
                    return new OperatorMultiplyNode(left, right);
                }
                case "/":
                {
                    TermNode right = stack.Pop();
                    TermNode left = stack.Pop();
                    return new OperatorDivideNode(left, right);
                }
                case "^":
                {
                    TermNode right = stack.Pop();
                    TermNode left = stack.Pop();
                    return new OperatorPowNode(left, right);
                }
                default:
                    throw new NotImplementedException($"This operator is not implemented: {operatorToken.Value}");
            }
        }

        public static void Test()
        {
            Term term = new Term("5 + 5");
            if (term.Resolve() != "10")
            {
                throw new SystemException("Term::Test 1");
            }

            // --------------------------------------------------------------

            term = new Term("5 + 5 + 5");
            if (term.Resolve() != "15")
            {
                throw new SystemException("Term::Test 2");
            }

            // --------------------------------------------------------------

            term = new Term("5 +-5+ 5");
            if (term.Resolve() != "5")
            {
                throw new SystemException("Term::Test 3");
            }

            // --------------------------------------------------------------

            term = new Term("-5 + 5 + 5");
            if (term.Resolve() != "5")
            {
                throw new SystemException("Term::Test 4");
            }

            // --------------------------------------------------------------

            term = new Term("-5 + 25 - +42");
            if (term.Resolve() != "-22")
            {
                throw new SystemException("Term::Test 5");
            }

            // --------------------------------------------------------------

            term = new Term("21 -12 --5");
            if (term.Resolve() != "14")
            {
                throw new SystemException("Term::Test 6");
            }

            // --------------------------------------------------------------

            term = new Term("10 * 10");
            if (term.Resolve() != "100")
            {
                throw new SystemException("Term::Test 7");
            }

            // --------------------------------------------------------------

            term = new Term("10 * 10 + 5");
            if (term.Resolve() != "105")
            {
                throw new SystemException("Term::Test 8");
            }

            // --------------------------------------------------------------

            term = new Term("10 * (10 + 5)");
            if (term.Resolve() != "150")
            {
                throw new SystemException("Term::Test 9");
            }

            // --------------------------------------------------------------

            term = new Term("10 / (10 - 5)");
            if (term.Resolve() != "2")
            {
                throw new SystemException("Term::Test 10");
            }

            // --------------------------------------------------------------

            term = new Term("-21 / -21 *4");
            if (term.Resolve() != "4")
            {
                throw new SystemException("Term::Test 11");
            }

            // --------------------------------------------------------------

            term = new Term("10^2");
            if (term.Resolve() != "100")
            {
                throw new SystemException("Term::Test 12");
            }

            // --------------------------------------------------------------

            term = new Term("10^3");
            if (term.Resolve() != "1000")
            {
                throw new SystemException("Term::Test 13");
            }

            // --------------------------------------------------------------

            term = new Term("10^2-1");
            if (term.Resolve() != "99")
            {
                throw new SystemException("Term::Test 14");
            }

            // --------------------------------------------------------------

            term = new Term("10^(3 - 1)");
            if (term.Resolve() != "100")
            {
                throw new SystemException("Term::Test 15");
            }

            // --------------------------------------------------------------

            //Periodische Zahlen führen noch zu Endlosschleife
            term = new Term("2 * 10^(3 - 1) / 3 / 2 * 3");
            if (term.Resolve() != "100")
            {
                throw new SystemException("Term::Test 16");
            }

            //// --------------------------------------------------------------
        }
    }
}
