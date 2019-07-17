using System;
using System.Collections.Generic;
using De.Markellus.Maths.Core.Arithmetic;
using De.Markellus.Maths.Core.TermEngine.Nodes.Base;
using De.Markellus.Maths.Core.TermEngine.Nodes.Implementation;
using De.Markellus.Maths.Core.TermEngine.TermParsing;

namespace De.Markellus.Maths.Core.TermEngine.Nodes
{
    internal static class TermNodeFactory
    {
        public static TermNode CreateNodesFromRpnToken(IEnumerable<Token> rpnToken, MathEnvironment env)
        {
            Stack<TermNode> stack = new Stack<TermNode>();

            foreach (Token token in rpnToken)
            {
                switch (token.Type)
                {
                    case TokenType.Number:
                        stack.Push(CreateNumberNode(token));
                        break;
                    case TokenType.Variable:
                        stack.Push(CreateVariableNode(token));
                        break;
                    case TokenType.Operator:
                        stack.Push(CreateOperatorNode(token, stack));
                        break;
                    case TokenType.Function:
                        stack.Push(CreateFunctionNode(token, stack));
                        break;
                }
            }

            if (stack.Count != 1)
            {
                throw new ArithmeticException("Unable to parse the given term.");
            }

            TermNode nodeResult = stack.Pop();
            SetMathEnvironment(nodeResult, env);
            return nodeResult;
        }

        private static void SetMathEnvironment(TermNode node, MathEnvironment env)
        {
            node.MathEnvironment = env;
            for(int i = 0; i < node.Count(); i++)
            {
                SetMathEnvironment(node[i], env);
            }
        }

        public static TermNode CreateNumberNode(Token token)
        {
            return new NumberNode(RealFactory.GenerateReal(token.Value));
        }

        public static TermNode CreateNumberNode(Real real)
        {
            return new NumberNode(real);
        }

        public static TermNode CreateVariableNode(Token token)
        {
            return new VariableNode(token.Value);
        }

        public static TermNode CreateOperatorNode(Token token, Stack<TermNode> stack)
        {
            if (stack.Count < 2)
            {
                throw new ArgumentException("The stack does not contain enough nodes to create a new operator node.");
            }

            switch (token.Value)
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
                case "=":
                {
                    TermNode right = stack.Pop();
                    TermNode left = stack.Pop();
                    return new EqualityNode(left, right, false);
                }
                case "!=":
                {
                    TermNode right = stack.Pop();
                    TermNode left = stack.Pop();
                    return new EqualityNode(left, right, true);
                }
                default:
                    throw new NotImplementedException($"This operator is not implemented: {token.Value}");
            }
        }

        public static TermNode CreateFunctionNode(Token token, Stack<TermNode> stack)
        {
            switch (token.Value)
            {
                case "sqrt":
                {
                    TermNode right = stack.Pop();
                    TermNode left = stack.Pop();
                    return new FunctionRootNode(left, right);
                }
                default:
                    throw new NotImplementedException($"This function is not implemented: {token.Value}");
            }
        }

        public static Stack<TermNode> MakeStack(params TermNode[] nodes)
        {
            Stack<TermNode> stack = new Stack<TermNode>(nodes.Length);
            foreach (TermNode node in nodes)
            {
                stack.Push(node);
            }

            return stack;
        }
    }
}
