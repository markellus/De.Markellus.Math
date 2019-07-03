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
    }
}
