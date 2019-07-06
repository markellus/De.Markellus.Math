using System;
using De.Markellus.Maths.Core.Arithmetic;
using De.Markellus.Maths.Core.TermEngine.Nodes.Implementation;

namespace De.Markellus.Maths.Core.TermEngine.Nodes.Base
{
    public abstract class TermNode
    {
        public abstract bool IsValid();

        public abstract Real Resolve();

        public abstract ulong GetChildCount();

        public abstract int Count();

        public abstract TermNode GetChild(int iIndex);

        public abstract void SetChild(int iIndex, TermNode node);

        public abstract override bool Equals(object obj);

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public static bool operator ==(TermNode left, TermNode right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TermNode left, TermNode right)
        {
            return !Equals(left, right);
        }

        public abstract TermNode CreateCopy();

        public TermNode this[int iIndex]
        {
            get => GetChild(iIndex);
            set => SetChild(iIndex, value);
        }


    }
}
