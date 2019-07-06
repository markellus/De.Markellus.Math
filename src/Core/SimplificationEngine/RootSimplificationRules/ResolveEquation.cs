using System;
using System.Collections.Generic;
using System.Text;
using De.Markellus.Maths.Core.TermEngine.Nodes;
using De.Markellus.Maths.Core.TermEngine.Nodes.Base;
using De.Markellus.Maths.Core.TermEngine.TermParsing;

namespace De.Markellus.Maths.Core.SimplificationEngine.RootSimplificationRules
{
    public class ResolveEquation : ISimplificationRule
    {
        public bool CanBeAppliedTo(TermNode node, MathExpressionTokenizer tokenizer)
        {
            try
            {
                node.Resolve();
                return true;
            }
            catch
            {
                return false;
            }
            
        }

        public TermNode Simplify(TermNode node, MathExpressionTokenizer tokenizer)
        {
            return NodeFactory.CreateNumberNode(new Token(TokenType.Number, node.Resolve()));
        }
    }
}
