/* Copyright (C) Marcel Bulla - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Marcel Bulla <postmaster@marcel-bulla.de>
 */

using System;
using System.Collections.Generic;
using De.Markellus.Maths.Core.Arithmetic;
using De.Markellus.Maths.Core.TermEngine.Nodes;
using De.Markellus.Maths.Core.TermEngine.Nodes.Base;
using De.Markellus.Maths.Core.TermEngine.Nodes.Implementation;
using De.Markellus.Maths.Core.TermEngine.TermParsing;

namespace De.Markellus.Maths.Core.TermEngine
{
    public class Term
    {
        private string _strInfixTerm;
        private TermNode _root;

        public MathExpressionTokenizer Tokenizer { get; }

        public Term(string strInfixTerm, MathExpressionTokenizer tokenizer = null)
        {
            _strInfixTerm = strInfixTerm;
            Tokenizer = tokenizer;
        }

        public Term(TermNode root, MathExpressionTokenizer tokenizer = null)
        {
            _root = root;
            Tokenizer = tokenizer;
        }

        public Real Resolve()
        {
            return ParseTerm().Resolve();
        }

        public TermNode ParseTerm()
        {
            if (_root == null)
            {
                IEnumerable<Token> rpnToken = ShuntingYardParser.InfixToRpn(_strInfixTerm, Tokenizer);
                _root = NodeFactory.CreateNodesFromRpnToken(rpnToken);
            }

            return _root;
        }
    }
}
