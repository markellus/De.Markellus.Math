using System;
using System.Collections.Generic;
using System.Text;
using De.Markellus.Maths.Core.TermEngine.Nodes.Base;
using De.Markellus.Maths.Core.TermEngine.Nodes.Implementation;
using De.Markellus.Maths.Core.TermEngine.TermParsing;

namespace De.Markellus.Maths.Core.TermEngine.NodeTransformation.PowTransformation
{
    internal class PowSimplify : INodeTransformationRule
    {
        public bool CanBeAppliedTo(TermNode node, MathExpressionTokenizer tokenizer)
        {
            return node is OperatorPowNode powNode &&
                   powNode.RightChild.IsResolvable() &&
                   powNode.RightChild.Resolve() == "1";
        }

        public TermNode Transform(TermNode node, MathExpressionTokenizer tokenizer)
        {
            return (node as OperatorPowNode)?.LeftChild;
        }
    }
}
