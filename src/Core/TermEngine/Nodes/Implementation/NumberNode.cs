using De.Markellus.Maths.Core.Arithmetic;
using De.Markellus.Maths.Core.TermEngine.Nodes.Base;

namespace De.Markellus.Maths.Core.TermEngine.Nodes.Implementation
{
    public class NumberNode : NullaryTermNode
    {
        public Real Value { get; set; }

        public NumberNode(Real value = null)
        {
            if (value == null)
            {
                value = new Real();
            }
            Value = value;
        }

        public override bool IsValid()
        {
            return Value != null;
        }

        public override Real Resolve()
        {
            return Value;
        }
    }
}
