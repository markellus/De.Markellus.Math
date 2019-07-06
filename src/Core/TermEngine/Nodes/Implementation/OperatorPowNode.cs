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

        public override bool Equals(object obj)
        {
            return obj is OperatorPowNode node && Equals(node);
        }

        protected bool Equals(OperatorPowNode other)
        {
            return Equals(LeftChild, other.LeftChild) && Equals(RightChild, other.RightChild);
        }

        public static bool operator ==(OperatorPowNode left, OperatorPowNode right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(OperatorPowNode left, OperatorPowNode right)
        {
            return !Equals(left, right);
        }

        public override TermNode CreateCopy()
        {
            return new OperatorPowNode(LeftChild.CreateCopy(), RightChild.CreateCopy());
        }
    }
}
