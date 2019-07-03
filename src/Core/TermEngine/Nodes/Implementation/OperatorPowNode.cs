using System;
using System.Collections.Generic;
using System.Text;
using De.Markellus.Maths.Core.Arithmetic;
using De.Markellus.Maths.Core.TermEngine.Nodes.Base;

namespace De.Markellus.Maths.Core.TermEngine.Nodes.Implementation
{
    public class OperatorPowNode : BinaryTermNode
    {
        public OperatorPowNode() : base()
        {
        }

        public OperatorPowNode(TermNode left, TermNode right) : base(left, right)
        {
        }

        public override Real Resolve()
        {
            return LeftChild.Resolve() ^ RightChild.Resolve();
        }
    }
}
