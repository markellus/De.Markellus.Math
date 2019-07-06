using System;
using System.Collections.Generic;
using System.Text;
using De.Markellus.Maths.Core.Arithmetic;
using De.Markellus.Maths.Core.TermEngine.Nodes.Base;

namespace De.Markellus.Maths.Core.TermEngine.Nodes.Implementation
{
    public class OperatorAddNode : BinaryTermNode
    {
        public OperatorAddNode() : base()
        {
        }

        public OperatorAddNode(TermNode left, TermNode right) : base(left, right)
        {
        }

        public override Real Resolve()
        {
            return LeftChild.Resolve() + RightChild.Resolve();
        }

        public override bool Equals(object obj)
        {
            return obj is OperatorAddNode node && Equals(node);
        }

        protected bool Equals(OperatorAddNode other)
        {
            return Equals(LeftChild, other.LeftChild) && Equals(RightChild, other.RightChild);
        }

        public static bool operator ==(OperatorAddNode left, OperatorAddNode right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(OperatorAddNode left, OperatorAddNode right)
        {
            return !Equals(left, right);
        }

        public override TermNode CreateCopy()
        {
            return new OperatorAddNode(LeftChild.CreateCopy(), RightChild.CreateCopy());
        }
    }
}
