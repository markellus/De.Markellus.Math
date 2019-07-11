using De.Markellus.Maths.Core.TermEngine.Nodes;
using De.Markellus.Maths.Core.TermEngine.Nodes.Base;
using De.Markellus.Maths.Core.TermEngine.Nodes.Implementation;
using De.Markellus.Maths.Core.TermEngine.TermParsing;

namespace De.Markellus.Maths.Core.TermEngine.NodeTransformation.PowTransformation
{
    internal class PowDivision : INodeTransformationRule
    {
        public bool CanBeAppliedTo(TermNode node, MathExpressionTokenizer tokenizer)
        {
            return node is OperatorDivideNode divNode &&
                   divNode.LeftChild is OperatorPowNode powLeft &&
                   divNode.RightChild is OperatorPowNode powRight &&
                   powLeft.LeftChild == powRight.LeftChild;
        }

        public TermNode Transform(TermNode node, MathExpressionTokenizer tokenizer)
        {
            OperatorDivideNode divNode = node as OperatorDivideNode;
            OperatorPowNode powLeft = divNode?.LeftChild as OperatorPowNode;
            OperatorPowNode powRight = divNode?.RightChild as OperatorPowNode;

            TermNode subPow = TermNodeFactory.CreateOperatorNode(
                tokenizer.GetRegisteredToken("-"),
                TermNodeFactory.MakeStack(powLeft.RightChild, powRight.RightChild));

            return TermNodeFactory.CreateOperatorNode(
                tokenizer.GetRegisteredToken("^"),
                TermNodeFactory.MakeStack(powLeft.LeftChild, subPow));
        }
    }
}
