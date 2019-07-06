using System;
using System.Collections.Generic;
using System.Text;
using De.Markellus.Maths.Core.TermEngine.Nodes;
using De.Markellus.Maths.Core.TermEngine.Nodes.Base;
using De.Markellus.Maths.Core.TermEngine.Nodes.Implementation;
using De.Markellus.Maths.Core.TermEngine.TermParsing;

namespace De.Markellus.Maths.Core.SimplificationEngine.RootSimplificationRules
{
    public class PowDivision : ISimplificationRule
    {
        public bool CanBeAppliedTo(TermNode node, MathExpressionTokenizer tokenizer)
        {
            return node is OperatorDivideNode divNode &&
                   divNode.LeftChild is OperatorPowNode powLeft &&
                   divNode.RightChild is OperatorPowNode powRight &&
                   powLeft.LeftChild == powRight.LeftChild;
        }

        public TermNode Simplify(TermNode node, MathExpressionTokenizer tokenizer)
        {
            OperatorDivideNode divNode = node as OperatorDivideNode;
            OperatorPowNode powLeft = divNode?.LeftChild as OperatorPowNode;
            OperatorPowNode powRight = divNode?.RightChild as OperatorPowNode;

            TermNode subPow = NodeFactory.CreateOperatorNode(
                tokenizer.GetRegisteredToken("-"),
                NodeFactory.MakeStack(powLeft.RightChild, powRight.RightChild));

            return NodeFactory.CreateOperatorNode(
                tokenizer.GetRegisteredToken("^"),
                NodeFactory.MakeStack(powLeft.LeftChild, subPow));
        }
    }
}
