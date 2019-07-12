using De.Markellus.Maths.Core.TermEngine.Nodes;
using De.Markellus.Maths.Core.TermEngine.Nodes.Base;
using De.Markellus.Maths.Core.TermEngine.Nodes.Implementation;
using De.Markellus.Maths.Core.TermEngine.TermParsing;

namespace De.Markellus.Maths.Core.TermEngine.NodeTransformation.ParenthesisTransformation
{
    /// <summary>
    /// Klammerregeln - Subtraktion Form 2
    /// 
    ///   Eingangsform: (x - y) - z
    ///   Ausgangsform: x - (y + z)
    ///
    ///   ODER
    ///
    ///   Eingangsform: x - (y - z)
    ///   Ausgangsform: (x - y) + z
    /// </summary>
    internal class ParenthesisSubtractionV1 : INodeTransformationRule
    {
        public bool CanBeAppliedTo(TermNode node, MathExpressionTokenizer tokenizer)
        {
            return node is OperatorSubtractNode subNode &&
                   (subNode.LeftChild is OperatorSubtractNode || subNode.RightChild is OperatorSubtractNode);
        }

        public TermNode Transform(TermNode node, MathExpressionTokenizer tokenizer)
        {
            OperatorSubtractNode addNode = node as OperatorSubtractNode;

            if (addNode?.LeftChild is OperatorSubtractNode subLeft)
            {
                TermNode transformedNode = TermNodeFactory.CreateOperatorNode(
                    tokenizer.GetRegisteredToken("+"),
                    TermNodeFactory.MakeStack(subLeft.RightChild.CreateCopy(), addNode.RightChild.CreateCopy()));
                return TermNodeFactory.CreateOperatorNode(
                    tokenizer.GetRegisteredToken("-"),
                    TermNodeFactory.MakeStack(subLeft.LeftChild.CreateCopy(), transformedNode));
            }
            if (addNode?.RightChild is OperatorSubtractNode subRight)
            {
                TermNode transformedNode = TermNodeFactory.CreateOperatorNode(
                    tokenizer.GetRegisteredToken("-"),
                    TermNodeFactory.MakeStack(addNode.LeftChild.CreateCopy(), subRight.LeftChild.CreateCopy()));
                return TermNodeFactory.CreateOperatorNode(
                    tokenizer.GetRegisteredToken("+"),
                    TermNodeFactory.MakeStack(transformedNode, subRight.RightChild.CreateCopy()));
            }

            return null;
        }
    }
}
