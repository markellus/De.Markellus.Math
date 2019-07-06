using De.Markellus.Maths.Core.TermEngine.Nodes.Base;
using De.Markellus.Maths.Core.TermEngine.TermParsing;

namespace De.Markellus.Maths.Core.SimplificationEngine
{
    public interface ISimplificationRule
    {
        bool CanBeAppliedTo(TermNode node, MathExpressionTokenizer tokenizer);

        TermNode Simplify(TermNode node, MathExpressionTokenizer tokenizer);
    }
}
