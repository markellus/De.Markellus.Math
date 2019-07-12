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
    /// Klammerregeln - Subtraktion Form 3
    /// 
    ///   Eingangsform: x - (y + z)
    ///   Ausgangsform: (x - y) - z
    /// </summary>
    internal class ParenthesisSubtractionV3 : INodeTransformationRule
    {
        public bool CanBeAppliedTo(TermNode node, MathExpressionTokenizer tokenizer)
        {
            return node is OperatorSubtractNode subNode && subNode.RightChild is OperatorAddNode;
        }

        public TermNode Transform(TermNode node, MathExpressionTokenizer tokenizer)
        {
            OperatorSubtractNode subNode = node as OperatorSubtractNode;

            if (subNode?.RightChild is OperatorAddNode addRight)
            {
                TermNode transformedNode = TermNodeFactory.CreateOperatorNode(
                    tokenizer.GetRegisteredToken("-"),
                    TermNodeFactory.MakeStack(subNode.LeftChild.CreateCopy(), addRight.LeftChild.CreateCopy()));
                return TermNodeFactory.CreateOperatorNode(
                    tokenizer.GetRegisteredToken("-"),
                    TermNodeFactory.MakeStack(transformedNode, addRight.RightChild.CreateCopy()));
            }

            return null;
        }
    }
}
