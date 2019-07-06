﻿using De.Markellus.Maths.Core.Arithmetic;
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
                value = RealFactory.GenerateReal("0.0");
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

        public override bool Equals(object obj)
        {
            return obj is NumberNode node && Equals(node);
        }

        public override TermNode CreateCopy()
        {
            return new NumberNode(RealFactory.GenerateReal(Value));
        }

        protected bool Equals(NumberNode other)
        {
            return Equals(Value, other.Value);
        }

        public static bool operator ==(NumberNode left, NumberNode right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(NumberNode left, NumberNode right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
