using De.Markellus.Maths.Core.TermEngine.Nodes;
using De.Markellus.Maths.Core.TermEngine.Nodes.Base;
using De.Markellus.Maths.Core.TermEngine.TermParsing;

namespace De.Markellus.Maths.Core.TermEngine.NodeTransformation.Arithmetic
{
    public class ResolveEquation : INodeTransformationRule
    {
        public bool CanBeAppliedTo(TermNode node, MathExpressionTokenizer tokenizer)
        {
            return node.IsResolvable();
        }

        public TermNode Transform(TermNode node, MathExpressionTokenizer tokenizer)
        {
            return TermNodeFactory.CreateNumberNode(new Token(TokenType.Number, node.Resolve()));
        }
    }
}
