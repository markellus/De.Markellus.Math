using System;
using System.Collections.Generic;
using System.Text;
using De.Markellus.Maths.Core.Arithmetic;
using De.Markellus.Maths.Core.TermEngine.Nodes.Base;

namespace De.Markellus.Maths.Core.TermEngine.Nodes.Implementation
{
    public class OperatorDivideNode : BinaryTermNode
    {
        public OperatorDivideNode() : base()
        {
        }

        public OperatorDivideNode(TermNode left, TermNode right) : base(left, right)
        {
        }

        public override bool Equals(object obj)
        {
            return obj is OperatorDivideNode node && Equals(node);
        }

        protected bool Equals(OperatorDivideNode other)
        {
            return Equals(LeftChild, other.LeftChild) && Equals(RightChild, other.RightChild);
        }

        public static bool operator ==(OperatorDivideNode left, OperatorDivideNode right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(OperatorDivideNode left, OperatorDivideNode right)
        {
            return !Equals(left, right);
        }

        public override Real Resolve()
        {
            return LeftChild.Resolve() / RightChild.Resolve();
        }

        public override TermNode CreateCopy()
        {
            return new OperatorDivideNode(LeftChild.CreateCopy(), RightChild.CreateCopy());
        }
    }
}
