using System;
using De.Markellus.Maths.Core.TermEngine.Nodes;
using De.Markellus.Maths.Core.TermEngine.Nodes.Base;
using De.Markellus.Maths.Core.TermEngine.Nodes.Implementation;
using De.Markellus.Maths.Core.TermEngine.TermParsing;

namespace De.Markellus.Maths.Core.TermEngine.NodeTransformation.PowTransformation
{
    /// <summary>
    /// Rechenregeln für Potenzen - Addition Form 3
    /// 
    ///   Eingangsform: (a * x^y) + (b * x^y)
    ///   Ausgangsform: (a + b) * x^y
    /// </summary>
    public class PowAdditionV3 : INodeTransformationRule
    {
        public bool CanBeAppliedTo(TermNode node, MathExpressionTokenizer tokenizer)
        {
            if (node is OperatorAddNode addNode &&
                addNode.LeftChild is OperatorMultiplyNode mulLeft &&
                addNode.RightChild is OperatorMultiplyNode mulRight)
            {
                return ((mulLeft.LeftChild is OperatorPowNode pow1 && pow1 == mulRight.LeftChild) ||
                        (mulLeft.LeftChild is OperatorPowNode pow2 && pow2 == mulRight.RightChild) ||
                        (mulLeft.RightChild is OperatorPowNode pow3 && pow3 == mulRight.LeftChild) ||
                        (mulLeft.RightChild is OperatorPowNode pow4 && pow4 == mulRight.RightChild));
            }

            return false;
        }

        public TermNode Transform(TermNode node, MathExpressionTokenizer tokenizer)
        {
            OperatorAddNode addNode = node as OperatorAddNode;
            OperatorMultiplyNode mulLeft = addNode?.LeftChild as OperatorMultiplyNode;
            OperatorMultiplyNode mulRight = addNode?.RightChild as OperatorMultiplyNode;

            if (mulLeft?.LeftChild is OperatorPowNode pow1)
            {
                TermNode addCombinedNode = TermNodeFactory.CreateOperatorNode(tokenizer.GetRegisteredToken("+"),
                    pow1 == mulRight?.LeftChild
                        ? TermNodeFactory.MakeStack(mulLeft.RightChild, mulRight?.RightChild)
                        : TermNodeFactory.MakeStack(mulLeft.RightChild, mulRight?.LeftChild));

                return TermNodeFactory.CreateOperatorNode(
                    tokenizer.GetRegisteredToken("*"),
                    TermNodeFactory.MakeStack(addCombinedNode, pow1));
            }
            if (mulLeft?.RightChild is OperatorPowNode pow2)
            {
                TermNode addCombinedNode = TermNodeFactory.CreateOperatorNode(tokenizer.GetRegisteredToken("+"),
                    pow2 == mulRight?.LeftChild
                        ? TermNodeFactory.MakeStack(mulLeft.LeftChild, mulRight?.RightChild)
                        : TermNodeFactory.MakeStack(mulLeft.LeftChild, mulRight?.LeftChild));

                return TermNodeFactory.CreateOperatorNode(
                    tokenizer.GetRegisteredToken("*"),
                    TermNodeFactory.MakeStack(addCombinedNode, pow2));
            }

            return null;
        }
    }
}
