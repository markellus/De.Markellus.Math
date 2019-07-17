using De.Markellus.Maths.Core.TermEngine.Nodes.Base;
using De.Markellus.Maths.Core.TermEngine.TermParsing;

namespace De.Markellus.Maths.Core.TermEngine.NodeTransformation
{
    internal interface INodeTransformationRule
    {
        bool CanBeAppliedTo(TermNode node, MathExpressionTokenizer tokenizer);

        TermNode Transform(TermNode node, MathExpressionTokenizer tokenizer);
    }
}
