using System;
using System.Collections.Generic;
using System.Text;
using De.Markellus.Maths.Core.TermEngine.Nodes;
using De.Markellus.Maths.Core.TermEngine.Nodes.Base;
using De.Markellus.Maths.Core.TermEngine.Nodes.Implementation;
using De.Markellus.Maths.Core.TermEngine.TermParsing;

namespace De.Markellus.Maths.Core.SimplificationEngine.RootSimplificationRules
{
    public class PowSimplify : ISimplificationRule
    {
        public bool CanBeAppliedTo(TermNode node, MathExpressionTokenizer tokenizer)
        {
            return node is OperatorPowNode powNode && powNode.RightChild.Resolve() == "1";
        }

        public TermNode Simplify(TermNode node, MathExpressionTokenizer tokenizer)
        {
            return (node as OperatorPowNode)?.LeftChild;
        }
    }
}
