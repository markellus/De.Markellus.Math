using De.Markellus.Maths.Core.TermEngine.Nodes;
using De.Markellus.Maths.Core.TermEngine.Nodes.Base;
using De.Markellus.Maths.Core.TermEngine.Nodes.Implementation;
using De.Markellus.Maths.Core.TermEngine.TermParsing;

namespace De.Markellus.Maths.Core.TermEngine.NodeTransformation.RootTransformation
{
    internal class RootDivision : INodeTransformationRule
    {
        public bool CanBeAppliedTo(TermNode node, MathExpressionTokenizer tokenizer)
        {
            return node is OperatorDivideNode divNode &&
                   divNode.LeftChild is FunctionRootNode &&
                   divNode.RightChild is FunctionRootNode;
        }

        public TermNode Transform(TermNode node, MathExpressionTokenizer tokenizer)
        {
            OperatorDivideNode divNode = node as OperatorDivideNode;
            FunctionRootNode numeratorNode = divNode?.LeftChild as FunctionRootNode;
            FunctionRootNode denumeratorNode = divNode?.RightChild as FunctionRootNode;

            TermNode nthNode = null;

            if (numeratorNode.RightChild != denumeratorNode.RightChild)
            {
                nthNode = TermNodeFactory.CreateOperatorNode(
                    tokenizer.GetRegisteredToken("*"),
                    TermNodeFactory.MakeStack(numeratorNode?.RightChild, denumeratorNode?.RightChild));
            }
            else
            {
                nthNode = numeratorNode?.RightChild;

                
            }

            TermNode divInner = TermNodeFactory.CreateOperatorNode(
                tokenizer.GetRegisteredToken("/"),
                TermNodeFactory.MakeStack(numeratorNode.LeftChild, denumeratorNode.LeftChild));

            TermNode simplified = TermNodeFactory.CreateFunctionNode(
                tokenizer.GetRegisteredToken("sqrt"),
                TermNodeFactory.MakeStack(divInner, nthNode));

            return simplified;
        }
    }
}
