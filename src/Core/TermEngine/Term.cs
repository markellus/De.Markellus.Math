/* Copyright (C) Marcel Bulla - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Marcel Bulla <postmaster@marcel-bulla.de>
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using De.Markellus.Maths.Core.Arithmetic;
using De.Markellus.Maths.Core.TermEngine.Nodes;
using De.Markellus.Maths.Core.TermEngine.Nodes.Base;
using De.Markellus.Maths.Core.TermEngine.Nodes.Implementation;
using De.Markellus.Maths.Core.TermEngine.NodeTransformation;
using De.Markellus.Maths.Core.TermEngine.TermParsing;

namespace De.Markellus.Maths.Core.TermEngine
{
    public class Term
    {
        private TermDatabase _database;

        private readonly TermNode _root;

        private List<TermNode> _listTransformations;

        public MathExpressionTokenizer Tokenizer { get; }

        public Term(string strInfixTerm, MathEnvironment env = null, MathExpressionTokenizer tokenizer = null)
        {
            IEnumerable<Token> tokens = ShuntingYardParser.InfixToRpn(strInfixTerm, tokenizer);
            _database = null;
            _root = TermNodeFactory.CreateNodesFromRpnToken(tokens, env);
            _listTransformations = null;
            Tokenizer = tokenizer;
        }

        public Term(TermNode root, MathExpressionTokenizer tokenizer = null)
        {
            _database = null;
            _root = root;
            _listTransformations = null;
            Tokenizer = tokenizer;
        }

        private Term(TermNode root, MathExpressionTokenizer tokenizer, TermDatabase database)
        {
            _database = database;
            _root = root;
            _listTransformations = null;
            Tokenizer = tokenizer;
        }

        public Real Resolve()
        {
            return ParseTerm().Resolve();
        }

        public bool IsResolvable()
        {
            return ParseTerm().IsResolvable();
        }

        public TermNode ParseTerm()
        {
            return _root;
        }

        public override bool Equals(object obj)
        {
            return obj is Term other && Equals(other);
        }

        protected bool Equals(Term other)
        {
            return Equals(_root, other._root);
        }

        public override int GetHashCode()
        {
            return (_root != null ? _root.ToString().GetHashCode() : 0);
        }

        public static bool operator ==(Term left, Term right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Term left, Term right)
        {
            return !Equals(left, right);
        }

        public List<Term> SimplifyTerm()
        {
            if (_listTransformations == null)
            {
                GenerateTransformations();
            }

            List<Term> listSimplified = new List<Term>();
            int lNodeCount = int.MaxValue;

            foreach (TermNode nodeVariation in _listTransformations)
            {
                int lChildCount = nodeVariation.GetRecursiveChildCount();
                if (lChildCount < lNodeCount)
                {
                    lNodeCount = lChildCount;
                    listSimplified.Clear();
                }

                if (lChildCount == lNodeCount)
                {
                    Term term = new Term(nodeVariation, Tokenizer);
                    listSimplified.Add(term);
                }
            }

            return listSimplified;
        }

        public Term CreateCopy()
        {
            throw new NotImplementedException();
        }

        private void GenerateTransformations()
        {
            if (_database == null)
            {
                _database = new TermDatabase();
            }
            GenerateTransformations(0);
            _database = null;
        }

        private List<TermNode> GenerateTransformations(int iDepth)
        {
            if (_listTransformations == null)
            {
                TermNodeDatabase listExcluded = new TermNodeDatabase();// { _root };
                _listTransformations = GenerateTransformations2(ref listExcluded, iDepth);
                _listTransformations = listExcluded.Get();

                Log($"----------- TRANSFORMATIONS OF {_root}", 0);
                foreach(TermNode node in _listTransformations)
                {
                    Log(node.ToString(), 0);
                }
                Log("\r\n\r\n", 0);

                //Alle Terme die durch die Transformationen entstanden sind haben logischerweise dieselben Transformationen
                //foreach (TermNode node in _listTransformations)
                //{
                //    Term term = TermKnowledgeBase.GetTermByNode(node);
                //    term._listTransformations = _listTransformations;
                //}
            }
            else
            {
                Log($"----------- TRANSFORMATIONS OF {_root}: REPEATED", 0);
                foreach (TermNode node in _listTransformations)
                {
                    Log(node.ToString(), 0);
                }
                Log("\r\n\r\n", 0);
            }
            
            return _listTransformations;
        }

        private void Log(string message, int iDepth,
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            File.AppendAllText("D:/debug-GenerateTransformations.log", $"{lineNumber:0000}: ");
            for (int i = 0; i < iDepth; i++)
            {
                File.AppendAllText("D:/debug-GenerateTransformations.log", "\t");
            }
            File.AppendAllText("D:/debug-GenerateTransformations.log", $"{message}\r\n");
        }

        private List<TermNode> GenerateTransformations2(ref TermNodeDatabase listExcluded, int iDepth)
        {
            //Uns selbst als behandelt betrachten
            listExcluded.Add(_root);
            
            List<TermNode> listTransformationsChildNodes = new List<TermNode> ();

            //Alle Transformationen aller Child Nodes ermitteln und in einer Liste abspeichern.
            List<TermNode>[] arrListTransformationCombinations = new List<TermNode>[_root.Count()];

            for (int i = 0; i < _root.Count(); i++)
            {
                Term termChild = _database.GetTermByNode(_root[i]);
                //Alle Transformationen eines Child Nodes ermitteln
                arrListTransformationCombinations[i] = new List<TermNode>(termChild.GenerateTransformations(iDepth + 1));
                //Die Original-Transformation ist natürlich auch eine Option
                arrListTransformationCombinations[i].Add(_root[i].CreateCopy());
            }

            //Die Transformationen der Child Nodes miteinander kombinieren
            for (int c = 0; c < _root.Count(); c++)
            {
                TermNode rootTransformation = _root.CreateCopy();
                for (int x = 0; x < arrListTransformationCombinations.Length; x++)
                {
                    for (int y = 0; y < arrListTransformationCombinations[x].Count; y++)
                    {
                        rootTransformation[x] = arrListTransformationCombinations[x][y].CreateCopy();
                        //keine doppelten Einträge und keine Transformation die wir schon haben
                        if(!listTransformationsChildNodes.Contains(rootTransformation) &&
                           !listExcluded.Contains(rootTransformation))
                        {
                            listTransformationsChildNodes.Add(rootTransformation);
                        }
                        rootTransformation = rootTransformation.CreateCopy();
                    }
                }
            }

            //Jetzt wird für jede gefundene Transformation erneut nach weiteren Transformationen gesucht,
            //so lange bis keine neuen mehr gefunden werden.
            bool bFoundNew = true;

            List<TermNode> listTransformationsAll = new List<TermNode>();
            List<TermNode> listTransformationsNew = new List<TermNode>();

            while (bFoundNew)
            {
                bFoundNew = false;

                foreach(TermNode node in listTransformationsChildNodes)
                {
                    Term termChild = _database.GetTermByNode(node);
                    List<TermNode> listNewTransformations = termChild.GenerateTransformations2(ref listExcluded, iDepth);

                    foreach(TermNode nodeTransformation in listNewTransformations)
                    {
                        if(!listTransformationsNew.Contains(nodeTransformation))
                        {
                            listTransformationsNew.Add(nodeTransformation.CreateCopy());
                            bFoundNew = true;
                        }
                    }
                }

                listTransformationsAll.AddRange(listTransformationsChildNodes);
                listTransformationsChildNodes = listTransformationsNew;
                listTransformationsNew = new List<TermNode>();
            }

            listTransformationsAll.Add(_root);

            //Jetzt ermitteln wir unsere eigenen Transformationen
            List<TermNode> listTransformationsFinal = new List<TermNode>();

            foreach (TermNode node in listTransformationsAll)
            {
                if(!listExcluded.Contains(node))
                {
                    listTransformationsFinal.Add(node);
                }

                foreach (TransformationRule rule in TransformationKnowledgeBase.GetTransformationRules())
                {
                    if (rule.CanBeAppliedTo(node))
                    {
                        TermNode nodeTransformed = rule.Transform(node, Tokenizer);
                        if (!listTransformationsFinal.Contains(nodeTransformed) &&
                            !listExcluded.Contains(nodeTransformed))
                        {
                            listTransformationsFinal.Add(nodeTransformed);
                        }
                    }
                }
            }

            //Rekursiv alle Transformationen der Transformationen ermitteln
            List<TermNode> listTransformationsRecursive = new List<TermNode>();

            foreach(TermNode node in listTransformationsFinal)
            {
                listTransformationsRecursive.Add(node);
                Term term = _database.GetTermByNode(node);
                listTransformationsRecursive.AddRange(term.GenerateTransformations2(ref listExcluded, iDepth));
            }

            return listTransformationsRecursive;
        }

        public override string ToString()
        {
            return _root.ToString();
        }

        private class TermDatabase
        {
            private readonly HashSet<Term> _knownTerms;

            public TermDatabase()
            {
                _knownTerms = new HashSet<Term>();
            }

            public Term GetTermByNode(TermNode root)
            {
                Term term = new Term(root, null, this);

                if (_knownTerms.TryGetValue(term, out Term result))
                {
                    return result;
                }
                else
                {
                    _knownTerms.Add(term);
                    return term;
                }
            }
        }
    }
}
