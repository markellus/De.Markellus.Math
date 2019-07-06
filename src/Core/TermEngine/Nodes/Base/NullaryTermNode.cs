using System;

namespace De.Markellus.Maths.Core.TermEngine.Nodes.Base
{
    public abstract class NullaryTermNode : TermNode
    {

        public override ulong GetChildCount()
        {
            return 0;
        }

        public override int Count()
        {
            return 0;
        }

        public override TermNode GetChild(int iIndex)
        {
            throw new ArgumentOutOfRangeException();
        }

        public override void SetChild(int iIndex, TermNode node)
        {
            throw new ArgumentOutOfRangeException();
        }
    }
}