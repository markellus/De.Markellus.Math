using De.Markellus.Maths.Core.Arithmetic;

namespace De.Markellus.Maths.Core.TermEngine.Nodes.Base
{
    public abstract class TermNode
    {
        public abstract bool IsValid();

        public abstract Real Resolve();
    }
}
