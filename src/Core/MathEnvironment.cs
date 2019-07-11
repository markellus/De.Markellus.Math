using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using De.Markellus.Maths.Core.Arithmetic;
using De.Markellus.Maths.Core.TermEngine;
using De.Markellus.Maths.Core.TermEngine.Nodes.Base;
using De.Markellus.Maths.Core.TermEngine.Nodes.Implementation;
using De.Markellus.Maths.Core.TermEngine.TermParsing;

namespace De.Markellus.Maths.Core
{
    public class MathEnvironment
    {
        private Dictionary<string, Real> _dicVariables;

        private MathExpressionTokenizer _tokenizer;

        private List<Term> _listTerms;

        public MathEnvironment()
        {
            _dicVariables = new Dictionary<string, Real>();
            _tokenizer = MathExpressionTokenizerFactory.Create();
            _listTerms = new List<Term>();

        }

        public void DefineVariable(string strVar, Real realValue = null)
        {
            if (_dicVariables.ContainsKey(strVar))
            {
                _dicVariables[strVar] = realValue;
            }
            else
            {
                _dicVariables.Add(strVar, realValue);
                _tokenizer.RegisterToken(TokenType.Variable, strVar);
            }
        }

        public void RemoveVariable(string strVar)
        {
            if (_dicVariables.ContainsKey(strVar))
            {
                _dicVariables.Remove(strVar);
                _tokenizer.UnregisterToken(strVar);
            }
        }

        public Term DefineTerm(string strInfixTerm)
        {
            Term term = new Term(strInfixTerm, _tokenizer);
            _listTerms.Add(term);

            return term;
        }

        public void RemoveTerm(Term term)
        {
            _listTerms.Remove(term);
        }

        public Real ResolveVariable(string strVar)
        {
            throw new NotImplementedException();
        }

        public List<Term> SimplifyTerm(Term term)
        {
            TermNode node = term.ParseTerm();
            NodeWorker worker = new NodeWorker(node, term.Tokenizer);
            List<TermNode> listVariations = worker.GetSimplifiedTermNodes();
            
            List<Term> listResults = new List<Term>(listVariations.Count);

            foreach (TermNode n in listVariations)
            {
                listResults.Add(new Term(n, _tokenizer));
            }
            return listResults;
        }

    }
}
