using System;
using System.Collections.Generic;
using System.Text;
using De.Markellus.Maths.Core.TermEngine.Nodes.Base;
using De.Markellus.Maths.Core.TermEngine.TermParsing;

namespace De.Markellus.Maths.Core.TermEngine
{
    internal class TermNodeDatabase
    {
        private List<TermNode> _groupedTerms;

        public TermNodeDatabase()
        {
            _groupedTerms = new List<TermNode>();
        }

        public List<TermNode> Get()
        {
            return _groupedTerms;
        }

        public void Add(TermNode node)
        {
            //if(_groupedTerms.Contains(node))
            //{ throw new ArgumentException();}
            //_groupedTerms.Add(node);
            if (!_groupedTerms.Contains(node))
            { _groupedTerms.Add(node); }
            
        }

        public void AddRange(List<TermNode> nodes)
        {
            foreach (TermNode node in nodes)
            {
                Add(node);
            }
        }

        public bool Contains(TermNode node)
        {
            return _groupedTerms.Contains(node);
        }
    }
}
