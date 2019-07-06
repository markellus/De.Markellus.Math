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
        
        public override bool Equals(object obj)
        {
            return obj is FunctionRootNode node && Equals(node);
        }

        protected bool Equals(FunctionRootNode other)
        {
            return Equals(LeftChild, other.LeftChild) && Equals(RightChild, other.RightChild);
        }

        public static bool operator ==(FunctionRootNode left, FunctionRootNode right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(FunctionRootNode left, FunctionRootNode right)
        {
            return !Equals(left, right);
        }

        public override TermNode CreateCopy()
        {
            return new FunctionRootNode(LeftChild.CreateCopy(), RightChild.CreateCopy());
        }
    }
}
