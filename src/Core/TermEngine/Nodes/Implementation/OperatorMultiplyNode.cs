using System;
using System.Collections.Generic;
using System.Text;
using De.Markellus.Maths.Core.Arithmetic;
using De.Markellus.Maths.Core.TermEngine.Nodes.Base;

namespace De.Markellus.Maths.Core.TermEngine.Nodes.Implementation
{
    public class OperatorMultiplyNode : BinaryTermNode
    {
        public OperatorMultiplyNode() : base()
        {
        }

        public OperatorMultiplyNode(TermNode left, TermNode right) : base(left, right)
        {
        }

        public override Real Resolve()
        {
            return LeftChild.Resolve() * RightChild.Resolve();
        }

        public override bool Equals(object obj)
        {
            return obj is OperatorMultiplyNode node && Equals(node);
        }

        protected bool Equals(OperatorMultiplyNode other)
        {
            return Equals(LeftChild, other.LeftChild) && Equals(RightChild, other.RightChild);
        }


        public static bool operator ==(OperatorMultiplyNode left, OperatorMultiplyNode right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(OperatorMultiplyNode left, OperatorMultiplyNode right)
        {
            return !Equals(left, right);
        }

        public override TermNode CreateCopy()
        {
            return new OperatorMultiplyNode(LeftChild.CreateCopy(), RightChild.CreateCopy());
        }
    }
}
