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
    }
}
