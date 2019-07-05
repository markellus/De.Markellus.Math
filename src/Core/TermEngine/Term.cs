/* Copyright (C) Marcel Bulla - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Marcel Bulla <postmaster@marcel-bulla.de>
 */

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
            //result.RoundPeriod();
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
    }
}
