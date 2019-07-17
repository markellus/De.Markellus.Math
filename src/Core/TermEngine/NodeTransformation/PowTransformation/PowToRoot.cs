using System;
using System.Collections.Generic;
using System.Text;
using De.Markellus.Maths.Core.TermEngine.Nodes;
using De.Markellus.Maths.Core.TermEngine.Nodes.Base;
using De.Markellus.Maths.Core.TermEngine.Nodes.Implementation;
using De.Markellus.Maths.Core.TermEngine.TermParsing;

namespace De.Markellus.Maths.Core.TermEngine.NodeTransformation.PowTransformation
{
    internal class PowToRoot : INodeTransformationRule
    {
        public bool CanBeAppliedTo(TermNode node, MathExpressionTokenizer tokenizer)
        {
            return node is OperatorPowNode powNode && powNode.RightChild is OperatorDivideNode;
        }

        public TermNode Transform(TermNode node, MathExpressionTokenizer tokenizer)
        {
            OperatorPowNode powNode = node as OperatorPowNode;
            OperatorDivideNode divNode = powNode?.RightChild as OperatorDivideNode;

            TermNode simplifiedInnerNode = TermNodeFactory.CreateOperatorNode(
                tokenizer.GetRegisteredToken("^"),
                TermNodeFactory.MakeStack(powNode?.LeftChild, divNode?.LeftChild));

            TermNode simplifiedOuterNode = TermNodeFactory.CreateFunctionNode(
                tokenizer.GetRegisteredToken("sqrt"),
                TermNodeFactory.MakeStack(simplifiedInnerNode, divNode?.RightChild));

            return simplifiedOuterNode;
        }
    }
}
