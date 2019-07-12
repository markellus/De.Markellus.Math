using System;
using System.Collections.Generic;
using System.Text;
using De.Markellus.Maths.Core.TermEngine.Nodes;
using De.Markellus.Maths.Core.TermEngine.Nodes.Base;
using De.Markellus.Maths.Core.TermEngine.Nodes.Implementation;
using De.Markellus.Maths.Core.TermEngine.TermParsing;

namespace De.Markellus.Maths.Core.TermEngine.NodeTransformation.ParenthesisTransformation
{
    /// <summary>
    /// Klammerregeln - Multiplikation
    /// 
    ///   Eingangsform: x * (y * z)
    ///   Ausgangsform: (x * y) * z
    ///
    ///   ODER
    ///
    ///   Eingangsform: (x * y) * z
    ///   Ausgangsform: x * (y * z)
    /// </summary>
    internal class ParenthesisMultiplication : INodeTransformationRule
    {
        public bool CanBeAppliedTo(TermNode node, MathExpressionTokenizer tokenizer)
        {
            return node is OperatorMultiplyNode mulNode &&
                   (mulNode.LeftChild is OperatorMultiplyNode || mulNode.RightChild is OperatorMultiplyNode);
        }

        public TermNode Transform(TermNode node, MathExpressionTokenizer tokenizer)
        {
            OperatorMultiplyNode mulNode = node as OperatorMultiplyNode;

            if (mulNode?.LeftChild is OperatorMultiplyNode mulLeft)
            {
                TermNode mulNew = TermNodeFactory.CreateOperatorNode(
                    tokenizer.GetRegisteredToken("*"),
                    TermNodeFactory.MakeStack(mulLeft.RightChild.CreateCopy(), mulNode.RightChild.CreateCopy()));

                return TermNodeFactory.CreateOperatorNode(
                    tokenizer.GetRegisteredToken("*"),
                    TermNodeFactory.MakeStack(mulLeft.LeftChild.CreateCopy(), mulNew));
            }
            if (mulNode?.RightChild is OperatorMultiplyNode mulRight)
            {
                TermNode mulNew = TermNodeFactory.CreateOperatorNode(
                    tokenizer.GetRegisteredToken("*"),
                    TermNodeFactory.MakeStack(mulNode.LeftChild.CreateCopy(), mulRight.LeftChild.CreateCopy()));

                return TermNodeFactory.CreateOperatorNode(
                    tokenizer.GetRegisteredToken("*"),
                    TermNodeFactory.MakeStack(mulNew, mulRight.RightChild.CreateCopy()));
            }

            return null;
        }
    }
}
