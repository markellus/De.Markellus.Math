using System;

namespace De.Markellus.Maths.Core.TermEngine.Nodes.Base
{
    public abstract class BinaryTermNode : TermNode
    {
        public TermNode LeftChild { get; private set; }

        public TermNode RightChild { get; private set; }

        protected BinaryTermNode()
        {
            LeftChild = null;
            RightChild = null;
        }

        protected BinaryTermNode(TermNode left, TermNode right)
        {
            LeftChild = left;
            RightChild = right;
        }

        public override bool IsValid()
        {
            return LeftChild != null && RightChild != null && LeftChild.IsValid() && RightChild.IsValid();
        }

        public override bool Equals(object obj)
        {
            return obj?.GetType() == GetType() && Equals(obj as BinaryTermNode);
        }

        //public override int GetHashCode()
        //{
        //    unchecked
        //    {
        //        return ((LeftChild != null ? LeftChild.GetHashCode() : 0) * 397) ^ (RightChild != null ? RightChild.GetHashCode() : 0);
        //    }
        //}

        protected bool Equals(BinaryTermNode other)
        {
            return Equals(LeftChild, other.LeftChild) && Equals(RightChild, other.RightChild);
        }

        public override ulong GetChildCount()
        {
            return LeftChild.GetChildCount() + RightChild.GetChildCount() + 2;
        }

        public override int Count()
        {
            return 2;
        }

        public override TermNode GetChild(int iIndex)
        {
            return iIndex == 0 ? LeftChild : RightChild;
        }

        public override void SetChild(int iIndex, TermNode node)
        {
            if (iIndex == 0)
            {
                LeftChild = node;
            }
            else
            {
                RightChild = node;
            }
        }

        public static bool operator ==(BinaryTermNode left, BinaryTermNode right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BinaryTermNode left, BinaryTermNode right)
        {
            return !Equals(left, right);
        }
    }
}
