using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using De.Markellus.Maths.Core.TermEngine.Nodes;
using De.Markellus.Maths.Core.TermEngine.Nodes.Base;
using De.Markellus.Maths.Core.TermEngine.Nodes.Implementation;
using De.Markellus.Maths.Core.TermEngine.TermParsing;

namespace De.Markellus.Maths.Core.SimplificationEngine.RootSimplificationRules
{
    public class RootDivision : ISimplificationRule
    {
        public bool CanBeAppliedTo(TermNode node, MathExpressionTokenizer tokenizer)
        {
            return node is OperatorDivideNode divNode &&
                   divNode.LeftChild is FunctionRootNode &&
                   divNode.RightChild is FunctionRootNode;
        }

        public TermNode Simplify(TermNode node, MathExpressionTokenizer tokenizer)
        {
            OperatorDivideNode divNode = node as OperatorDivideNode;
            FunctionRootNode numeratorNode = divNode?.LeftChild as FunctionRootNode;
            FunctionRootNode denumeratorNode = divNode?.RightChild as FunctionRootNode;

            TermNode nthNode = null;

            if (numeratorNode.RightChild != denumeratorNode.RightChild)
            {
                nthNode = NodeFactory.CreateOperatorNode(
                    tokenizer.GetRegisteredToken("*"),
                    NodeFactory.MakeStack(numeratorNode?.RightChild, denumeratorNode?.RightChild));
            }
            else
            {
                nthNode = numeratorNode?.RightChild;
            }

            TermNode divInner = NodeFactory.CreateOperatorNode(
                tokenizer.GetRegisteredToken("/"),
                NodeFactory.MakeStack(numeratorNode.LeftChild, denumeratorNode.LeftChild));

            TermNode simplified = NodeFactory.CreateFunctionNode(
                tokenizer.GetRegisteredToken("sqrt"),
                NodeFactory.MakeStack(divInner, nthNode));

            return simplified;
        }
    }
}
