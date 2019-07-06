using System;
using System.Collections.Generic;
using System.Text;
using De.Markellus.Maths.Core.TermEngine.Nodes.Base;

namespace De.Markellus.Maths.Core.TermEngine
{
    public abstract class UnaryTermNode : TermNode
    {
        public TermNode Child { get; private set; }

        public UnaryTermNode()
        {
            Child = null;
        }

        public UnaryTermNode(TermNode child)
        {
            Child = child;
        }

        public override bool IsValid()
        {
            return Child != null && Child.IsValid();
        }

        public override bool Equals(object obj)
        {
            return obj?.GetType() == GetType() && Equals(obj as UnaryTermNode);
        }

        protected bool Equals(UnaryTermNode other)
        {
            return Equals(Child, other.Child);
        }


        public override ulong GetChildCount()
        {
            return Child.GetChildCount() + 1;
        }

        public override int Count()
        {
            return 1;
        }

        public override TermNode GetChild(int iIndex)
        {
            if (iIndex == 0)
            {
                return Child;
            }
            throw new ArgumentOutOfRangeException();
        }

        public override void SetChild(int iIndex, TermNode node)
        {
            if (iIndex == 0)
            {
                Child = node;
            }
            throw new ArgumentOutOfRangeException();
        }

        public static bool operator ==(UnaryTermNode left, UnaryTermNode right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UnaryTermNode left, UnaryTermNode right)
        {
            return !Equals(left, right);
        }
    }
}
