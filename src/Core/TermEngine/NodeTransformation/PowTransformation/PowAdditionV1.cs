using De.Markellus.Maths.Core.TermEngine.Nodes;
using De.Markellus.Maths.Core.TermEngine.Nodes.Base;
using De.Markellus.Maths.Core.TermEngine.Nodes.Implementation;
using De.Markellus.Maths.Core.TermEngine.TermParsing;

namespace De.Markellus.Maths.Core.TermEngine.NodeTransformation.PowTransformation
{
    /// <summary>
    /// Rechenregeln für Potenzen - Addition Form 1
    /// 
    ///   Eingangsform: x^y + x^y
    ///   Ausgangsform: 2 * x^y
    /// </summary>
    public class PowAdditionV1 : INodeTransformationRule
    {
        public bool CanBeAppliedTo(TermNode node, MathExpressionTokenizer tokenizer)
        {
            return node is OperatorAddNode addNode &&
                   addNode.LeftChild is OperatorPowNode powLeft &&
                   addNode.RightChild is OperatorPowNode powRight &&
                   powLeft == powRight;
        }

        public TermNode Transform(TermNode node, MathExpressionTokenizer tokenizer)
        {
            return TermNodeFactory.CreateOperatorNode(
                tokenizer.GetRegisteredToken("*"),
                TermNodeFactory.MakeStack(
                    TermNodeFactory.CreateNumberNode(new Token(TokenType.Number, "2")),
                    (node as OperatorAddNode)?.LeftChild));
        }
    }
}