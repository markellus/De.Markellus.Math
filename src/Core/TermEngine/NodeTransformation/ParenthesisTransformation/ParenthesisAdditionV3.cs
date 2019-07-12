using De.Markellus.Maths.Core.TermEngine.Nodes;
using De.Markellus.Maths.Core.TermEngine.Nodes.Base;
using De.Markellus.Maths.Core.TermEngine.Nodes.Implementation;
using De.Markellus.Maths.Core.TermEngine.TermParsing;

namespace De.Markellus.Maths.Core.TermEngine.NodeTransformation.ParenthesisTransformation
{
    /// <summary>
    /// Klammerregeln - Addition Form 3
    /// 
    ///   Eingangsform: x + (y - z)
    ///   Ausgangsform: (x + y) - z
    /// </summary>
    internal class ParenthesisAdditionV3 : INodeTransformationRule
    {
        public bool CanBeAppliedTo(TermNode node, MathExpressionTokenizer tokenizer)
        {
            return node is OperatorAddNode addNode && addNode.RightChild is OperatorSubtractNode;
        }

        public TermNode Transform(TermNode node, MathExpressionTokenizer tokenizer)
        {
            OperatorAddNode addNode = node as OperatorAddNode;

            if(addNode?.RightChild is OperatorSubtractNode subRight)
            {
                TermNode transformedNode = TermNodeFactory.CreateOperatorNode(
                    tokenizer.GetRegisteredToken("+"),
                    TermNodeFactory.MakeStack(addNode.LeftChild.CreateCopy(), subRight.LeftChild.CreateCopy()));
                return TermNodeFactory.CreateOperatorNode(
                    tokenizer.GetRegisteredToken("-"),
                    TermNodeFactory.MakeStack(transformedNode, subRight.RightChild.CreateCopy()));
            }

            return null;
        }
    }
}
