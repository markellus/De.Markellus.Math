using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using De.Markellus.Maths.Core.SimplificationEngine;
using De.Markellus.Maths.Core.TermEngine.Nodes.Base;
using De.Markellus.Maths.Core.TermEngine.TermParsing;

namespace De.Markellus.Maths.Core.TermEngine
{
    public class NodeWorker
    {
        private static List<ISimplificationRule> _listSimplificationRules;

        private TermNode _node;
        private MathExpressionTokenizer _tokenizer;

        private List<TermNode> _listTermVariations;

        static NodeWorker()
        {
            _listSimplificationRules = new List<ISimplificationRule>();
            LoadSimplificationRules();
        }

        private static void LoadSimplificationRules()
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    foreach (Type type in assembly.GetTypes())
                    {
                        if (type.GetInterfaces().Contains(typeof(ISimplificationRule)) && !type.IsAbstract)
                        {
                            ISimplificationRule rule = (ISimplificationRule)Activator.CreateInstance(type);
                            _listSimplificationRules.Add(rule);
                        }
                    }
                }
                catch { }
            }
        }

        public NodeWorker(TermNode node, MathExpressionTokenizer tokenizer)
        {
            _node = node;
            _tokenizer = tokenizer;
            _listTermVariations = new List<TermNode>();
        }

        public List<TermNode> GetSimplifiedTermNodes()
        {
            List<TermNode> listVariations = GenerateVariations();
            List<TermNode> listSimplified = new List<TermNode>();
            ulong lNodeCount = ulong.MaxValue;

            foreach (TermNode nodeVariation in listVariations)
            {
                ulong lChildCount = nodeVariation.GetChildCount();
                if (lChildCount < lNodeCount)
                {
                    lNodeCount = lChildCount;
                    listSimplified.Clear();
                }

                if (lChildCount == lNodeCount)
                {
                    listSimplified.Add(nodeVariation);
                }
            }

            return listSimplified;
        }

        public List<TermNode> GenerateVariations()
        {
            TermNode nodeOriginal = _node;

            //Dictionary<TermNode, bool> dicVariations = new Dictionary<TermNode, bool>();
            List<TermNode> listVariations = new List<TermNode>();
            //dicVariations.Add(_node, false);
            listVariations.Add(_node);

            bool bNewVariationFound = true;

            while (bNewVariationFound)
            {
                bNewVariationFound = false;

                List<TermNode> listVariationsNew = new List<TermNode>();

                //foreach (KeyValuePair<TermNode, bool> nodeVariation in dicVariations.Where(kvp => kvp.Value == false))
                foreach(TermNode node in listVariations)
                {
                    //_node = nodeVariation.Key;
                    _node = node;

                    foreach (TermNode nodeNewVariation in GenerateVariationsInner())
                    {
                        if (!listVariationsNew.Contains(nodeNewVariation))
                        {
                            listVariationsNew.Add(nodeNewVariation);
                        }
                    }
                }

                foreach (TermNode nodeVariation in listVariationsNew)
                {
                    //if (!dicVariations.ContainsKey(nodeVariation))
                    if (!listVariations.Contains(nodeVariation))
                    {
                        //dicVariations.Add(nodeVariation, false);
                        listVariations.Add(nodeVariation);
                        bNewVariationFound = true;
                    }
                    else
                    {
                        //dicVariations[nodeVariation] = true;
                    }
                }
            }

            _node = nodeOriginal;

            //return dicVariations.Keys.ToList();
            return listVariations;
        }

        private List<TermNode> GenerateVariationsInner()
        {
            List<TermNode> listVariationsChildren = new List<TermNode>();
            listVariationsChildren.Add(_node);

            for (int i = 0; i < _node.Count(); i++)
            {
                NodeWorker worker = new NodeWorker(_node[i], _tokenizer);
                foreach (TermNode node in worker.GenerateVariationsInner())
                {
                    TermNode copyC = _node.CreateCopy();
                    copyC[i] = node;
                    if (!listVariationsChildren.Contains(copyC))
                    {
                        listVariationsChildren.Add(copyC);
                    }
                }
            }

            List<TermNode> listVariations = new List<TermNode>(listVariationsChildren);

            foreach (TermNode node in listVariationsChildren)
            {
                foreach (ISimplificationRule rule in _listSimplificationRules)
                {
                    if (rule.CanBeAppliedTo(node, _tokenizer))
                    {
                        TermNode copy = node.CreateCopy();
                        copy = rule.Simplify(copy, _tokenizer);

                        if (!listVariations.Contains(copy))
                        {
                            listVariations.Add(copy);
                            //AddVariation(ref listVariations, node);
                        }
                    }
                }
            }

            List<TermNode> listVariationsFinal = listVariations;//new List<TermNode>();

            //foreach (TermNode node in listVariations)
            //{
            //    NodeWorker worker = new NodeWorker(node, _tokenizer);
            //    foreach (TermNode nodeFinal in worker.GenerateVariations())
            //    {
            //        if (!listVariationsFinal.Contains(nodeFinal))
            //        {
            //            listVariations.Add(nodeFinal);
            //        }
            //    }
            //}

            return listVariationsFinal;
        }

    }
}
