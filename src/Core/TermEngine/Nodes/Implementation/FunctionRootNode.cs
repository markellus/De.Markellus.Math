using System;
using System.Collections.Generic;
using System.Text;
using De.Markellus.Maths.Core.Arithmetic;
using De.Markellus.Maths.Core.TermEngine.Nodes.Base;

namespace De.Markellus.Maths.Core.TermEngine.Nodes.Implementation
{
    public class FunctionRootNode : BinaryTermNode
    {
        public FunctionRootNode() : base()
        {
        }

        public FunctionRootNode(TermNode left, TermNode right) : base(left, right)
        {
        }

        public override Real Resolve()
        {
            return Real.Root(LeftChild.Resolve(), RightChild.Resolve());
        }
    }
}
