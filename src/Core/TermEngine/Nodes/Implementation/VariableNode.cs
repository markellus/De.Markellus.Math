using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using De.Markellus.Maths.Core.Arithmetic;
using De.Markellus.Maths.Core.TermEngine.Nodes.Base;

namespace De.Markellus.Maths.Core.TermEngine.Nodes.Implementation
{
    public class VariableNode : NullaryTermNode
    {
        public string Value { get; set; }

        public VariableNode(string strVar)
        {
            Value = strVar;
        }

        public override bool IsValid()
        {
            return Value != null;
        }

        public override Real Resolve()
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            return obj is VariableNode node && Equals(node);
        }

        public override TermNode CreateCopy()
        {
            return new VariableNode(Value);
        }

        protected bool Equals(VariableNode other)
        {
            return Equals(Value, other.Value);
        }

        public static bool operator ==(VariableNode left, VariableNode right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(VariableNode left, VariableNode right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
