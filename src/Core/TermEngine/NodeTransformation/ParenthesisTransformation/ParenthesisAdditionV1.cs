using De.Markellus.Maths.Core.TermEngine.Nodes;
using De.Markellus.Maths.Core.TermEngine.Nodes.Base;
using De.Markellus.Maths.Core.TermEngine.Nodes.Implementation;
using De.Markellus.Maths.Core.TermEngine.TermParsing;

namespace De.Markellus.Maths.Core.TermEngine.NodeTransformation.ParenthesisTransformation
{
    /// <summary>
    /// Klammerregeln - Addition Form 1
    /// 
    ///   Eingangsform: (x + y) + z
    ///   Ausgangsform: x + (y + z)
    ///
    ///   ODER
    ///
    ///   Eingangsform: x + (y + z)
    ///   Ausgangsform: (x + y) + z
    /// </summary>
    internal class ParenthesisAdditionV1 : INodeTransformationRule
    {
        public bool CanBeAppliedTo(TermNode node, MathExpressionTokenizer tokenizer)
        {
            return node is OperatorAddNode addNode &&
                   (addNode.LeftChild is OperatorAddNode || addNode.RightChild is OperatorAddNode);
        }

        public TermNode Transform(TermNode node, MathExpressionTokenizer tokenizer)
        {
            OperatorAddNode addNode = node as OperatorAddNode;

            if (addNode?.LeftChild is OperatorAddNode addLeft)
            {
                TermNode transformedNode = TermNodeFactory.CreateOperatorNode(
                    tokenizer.GetRegisteredToken("+"),
                    TermNodeFactory.MakeStack(addLeft.RightChild.CreateCopy(), addNode.RightChild.CreateCopy()));
                return TermNodeFactory.CreateOperatorNode(
                    tokenizer.GetRegisteredToken("+"),
                    TermNodeFactory.MakeStack(addLeft.LeftChild.CreateCopy(), transformedNode));
            }
            if(addNode?.RightChild is OperatorAddNode addRight)
            {
                TermNode transformedNode = TermNodeFactory.CreateOperatorNode(
                    tokenizer.GetRegisteredToken("+"),
                    TermNodeFactory.MakeStack(addNode.LeftChild.CreateCopy(), addRight.LeftChild.CreateCopy()));
                return TermNodeFactory.CreateOperatorNode(
                    tokenizer.GetRegisteredToken("+"),
                    TermNodeFactory.MakeStack(transformedNode, addRight.RightChild.CreateCopy()));
            }

            return null;
        }
    }
}
