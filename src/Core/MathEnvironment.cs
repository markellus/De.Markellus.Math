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
        private Dictionary<string, Term> _dicVariables;

        private MathExpressionTokenizer _tokenizer;

        private List<Term> _listTerms;

        public MathEnvironment()
        {
            _dicVariables = new Dictionary<string, Term>();
            _tokenizer = MathExpressionTokenizerFactory.Create();
            _listTerms = new List<Term>();
        }

        public void DefineVariable(string strVar, Term termValue = null)
        {
            if (_dicVariables.ContainsKey(strVar))
            {
                _dicVariables[strVar] = termValue;
            }
            else
            {
                _dicVariables.Add(strVar, termValue);
                _tokenizer.RegisterToken(TokenType.Variable, strVar);
            }
        }

        public Term GetVariable(string strVar)
        {
            if (_dicVariables.ContainsKey(strVar))
            {
                return _dicVariables[strVar];
            }
            return null;
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
            Term term = new Term(strInfixTerm, this, _tokenizer);
            _listTerms.Add(term);

            return term;
        }

        public Term DefineTerm(TermNode termNode)
        {
            Term term = new Term(termNode);
            _listTerms.Add(term);

            return term;
        }

        public void RemoveTerm(Term term)
        {
            _listTerms.Remove(term);
        }

        public Real ResolveVariable(string strVar)
        {
            if (_dicVariables.ContainsKey(strVar))
            {
                Term term = _dicVariables[strVar];
                if (term.IsResolvable())
                {
                    return term.Resolve();
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                throw new ArgumentException($"The variable {strVar} is not defined.");
            }
        }

        public List<Term> SimplifyTerm(Term term)
        {
            return term.SimplifyTerm();
            //TermNode node = term.ParseTerm();
            //NodeWorker_Old workerOld = new NodeWorker_Old(node, term.Tokenizer);
            //List<TermNode> listVariations = workerOld.GetSimplifiedTermNodes();
            
            //List<Term> listResults = new List<Term>(listVariations.Count);

            //foreach (TermNode n in listVariations)
            //{
            //    listResults.Add(new Term(n, _tokenizer));
            //}
            //return listResults;
        }

    }
}
