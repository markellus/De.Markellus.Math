using System;
using System.Collections.Generic;
using System.Text;
using De.Markellus.Maths.Core.TermEngine.Nodes;
using De.Markellus.Maths.Core.TermEngine.Nodes.Base;
using De.Markellus.Maths.Core.TermEngine.Nodes.Implementation;
using De.Markellus.Maths.Core.TermEngine.TermParsing;

namespace De.Markellus.Maths.Core.SimplificationEngine.RootSimplificationRules
{
    public class PowToRoot : ISimplificationRule
    {
        public bool CanBeAppliedTo(TermNode node, MathExpressionTokenizer tokenizer)
        {
            return node is OperatorPowNode powNode && powNode.RightChild is OperatorDivideNode;
        }

        public TermNode Simplify(TermNode node, MathExpressionTokenizer tokenizer)
        {
            OperatorPowNode powNode = node as OperatorPowNode;
            OperatorDivideNode divNode = powNode?.RightChild as OperatorDivideNode;

            TermNode simplifiedInnerNode = NodeFactory.CreateOperatorNode(
                tokenizer.GetRegisteredToken("^"),
                NodeFactory.MakeStack(powNode?.LeftChild, divNode?.LeftChild));

            TermNode simplifiedOuterNode = NodeFactory.CreateFunctionNode(
                tokenizer.GetRegisteredToken("sqrt"),
                NodeFactory.MakeStack(simplifiedInnerNode, divNode?.RightChild));

            return simplifiedOuterNode;
        }
    }
}
