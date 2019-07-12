using De.Markellus.Maths.Core.TermEngine.Nodes;
using De.Markellus.Maths.Core.TermEngine.Nodes.Base;
using De.Markellus.Maths.Core.TermEngine.Nodes.Implementation;
using De.Markellus.Maths.Core.TermEngine.TermParsing;

namespace De.Markellus.Maths.Core.TermEngine.NodeTransformation.ParenthesisTransformation
{
    /// <summary>
    /// Klammerregeln - Addition Form 2
    /// 
    ///   Eingangsform: (x - y) + z
    ///   Ausgangsform: x - (y - z)
    /// </summary>
    internal class ParenthesisAdditionV2 : INodeTransformationRule
    {
        public bool CanBeAppliedTo(TermNode node, MathExpressionTokenizer tokenizer)
        {
            return node is OperatorAddNode addNode &&
                   (addNode.LeftChild is OperatorSubtractNode);
        }

        public TermNode Transform(TermNode node, MathExpressionTokenizer tokenizer)
        {
            OperatorAddNode addNode = node as OperatorAddNode;

            if (addNode?.LeftChild is OperatorSubtractNode subLeft)
            {
                TermNode transformedNode = TermNodeFactory.CreateOperatorNode(
                    tokenizer.GetRegisteredToken("-"),
                    TermNodeFactory.MakeStack(subLeft.RightChild.CreateCopy(), addNode.RightChild.CreateCopy()));
                return TermNodeFactory.CreateOperatorNode(
                    tokenizer.GetRegisteredToken("-"),
                    TermNodeFactory.MakeStack(subLeft.LeftChild.CreateCopy(), transformedNode));
            }

            return null;
        }
    }
}
