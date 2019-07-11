using De.Markellus.Maths.Core.TermEngine.Nodes;
using De.Markellus.Maths.Core.TermEngine.Nodes.Base;
using De.Markellus.Maths.Core.TermEngine.Nodes.Implementation;
using De.Markellus.Maths.Core.TermEngine.TermParsing;

namespace De.Markellus.Maths.Core.TermEngine.NodeTransformation.PowTransformation
{
    /// <summary>
    /// Rechenregeln für Potenzen - Addition Form 2
    /// 
    ///   Eingangsform: x^y + a * x^y
    ///   Ausgangsform: (1 + a) * x^y
    ///
    ///     ODER
    /// 
    ///   Eingangsform: a * x^y + x^y
    ///   Ausgangsform: (a + 1) * x^y
    /// TODO: sehr umstaendlich programmiert, beachtet nicht das bei Transform garantiert ist das die Form korrekt ist
    /// </summary>
    public class PowAdditionV2 : INodeTransformationRule
    {
        public bool CanBeAppliedTo(TermNode node, MathExpressionTokenizer tokenizer)
        {
            if (node is OperatorAddNode addNode)
            {
                if (addNode.LeftChild is OperatorMultiplyNode mulLeft &&
                    addNode.RightChild is OperatorPowNode powRight)
                {
                    OperatorPowNode powLeft1 = (mulLeft.LeftChild is OperatorPowNode powLeft1X) ? powLeft1X : null;
                    OperatorPowNode powLeft2 = (mulLeft.RightChild is OperatorPowNode powLeft2X) ? powLeft2X : null;

                    if (powRight == powLeft1 || powRight == powLeft2)
                    {
                        return true;
                    }
                }
                else if (addNode.RightChild is OperatorMultiplyNode mulRight &&
                         addNode.LeftChild is OperatorPowNode powLeft)
                {
                    OperatorPowNode powRight1 = (mulRight.LeftChild is OperatorPowNode powRight1X) ? powRight1X : null;
                    OperatorPowNode powRight2 = (mulRight.RightChild is OperatorPowNode powRight2X) ? powRight2X : null;

                    if (powLeft == powRight1 || powLeft == powRight2)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public TermNode Transform(TermNode node, MathExpressionTokenizer tokenizer)
        {
            TermNode targetNode = null;

            OperatorAddNode addNode = node as OperatorAddNode;

            TermNode oneNode = TermNodeFactory.CreateNumberNode(tokenizer.GetRegisteredToken("1"));

            if (addNode.LeftChild is OperatorMultiplyNode mulLeft &&
                addNode.RightChild is OperatorPowNode powRight)
            {
                if (powRight == mulLeft.LeftChild)
                {
                    targetNode = mulLeft.RightChild;
                }
                else if (powRight == mulLeft.RightChild)
                {
                    targetNode = mulLeft.LeftChild;
                }

                TermNode addCombinedNode = TermNodeFactory.CreateOperatorNode(
                    tokenizer.GetRegisteredToken("+"),
                    TermNodeFactory.MakeStack(targetNode, oneNode));
                return TermNodeFactory.CreateOperatorNode(
                    tokenizer.GetRegisteredToken("*"),
                    TermNodeFactory.MakeStack(addCombinedNode, powRight));
            }

            if (addNode.RightChild is OperatorMultiplyNode mulRight &&
                addNode.LeftChild is OperatorPowNode powLeft)
            {

                if (powLeft == mulRight.LeftChild)
                {
                    targetNode = mulRight.RightChild;
                }
                else if (powLeft == mulRight.RightChild)
                {
                    targetNode = mulRight.LeftChild;
                }

                TermNode addCombinedNode = TermNodeFactory.CreateOperatorNode(
                    tokenizer.GetRegisteredToken("+"),
                    TermNodeFactory.MakeStack(oneNode, targetNode));
                return TermNodeFactory.CreateOperatorNode(
                    tokenizer.GetRegisteredToken("*"),
                    TermNodeFactory.MakeStack(addCombinedNode, powLeft));
            }

            return null;
        }
    }
}
