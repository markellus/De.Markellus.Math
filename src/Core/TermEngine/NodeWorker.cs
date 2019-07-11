using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using De.Markellus.Maths.Core.TermEngine.Nodes.Base;
using De.Markellus.Maths.Core.TermEngine.NodeTransformation;
using De.Markellus.Maths.Core.TermEngine.TermParsing;

namespace De.Markellus.Maths.Core.TermEngine
{
    public class NodeWorker
    {
        private static List<INodeTransformationRule> _listSimplificationRules;

        private TermNode _node;
        private MathExpressionTokenizer _tokenizer;

        static NodeWorker()
        {
            _listSimplificationRules = new List<INodeTransformationRule>();
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
                        if (type.GetInterfaces().Contains(typeof(INodeTransformationRule)) && !type.IsAbstract)
                        {
                            INodeTransformationRule rule = (INodeTransformationRule)Activator.CreateInstance(type);
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
        }

        public List<TermNode> GetSimplifiedTermNodes()
        {
            List<TermNode> listVariations = GenerateVariations();
            List<TermNode> listSimplified = new List<TermNode>();
            int lNodeCount = int.MaxValue;

            foreach (TermNode nodeVariation in listVariations)
            {
                int lChildCount = nodeVariation.GetRecursiveChildCount();
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

        private List<TermNode> GenerateVariations()
        {
            List<TermNode> listVariations = new List<TermNode>();
            listVariations.Add(_node);

            bool bNewVariationFound = true;

            while (bNewVariationFound)
            {
                bNewVariationFound = false;

                List<TermNode> listVariationsNew = GenerateVariationsInnerAsync(listVariations);

                foreach (TermNode nodeVariation in listVariationsNew)
                {
                    if (!listVariations.Contains(nodeVariation))
                    {
                        listVariations.Add(nodeVariation);
                        bNewVariationFound = true;
                    }
                }
            }

            return listVariations;
        }

        private List<TermNode> GenerateVariationsInnerAsync(List<TermNode> listVariations)
        {
            List<TermNode> listVariationsNew = new List<TermNode>();
            object lockList = new object();

            Parallel.ForEach(listVariations, node =>
            {
                foreach (TermNode nodeNewVariation in GenerateVariationsInner(node))
                {
                    lock (lockList)
                    {
                        if (!listVariationsNew.Contains(nodeNewVariation))
                        {
                            listVariationsNew.Add(nodeNewVariation);
                        }
                    }
                }
            });

            return listVariationsNew;
        }

        private List<TermNode> GenerateVariationsInner(TermNode nodeInner)
        {
            List<TermNode> listVariationsChildren = new List<TermNode>();
            listVariationsChildren.Add(nodeInner);

            if (nodeInner.Count() == 0)
            {
                return listVariationsChildren;
            }

            for (int i = 0; i < nodeInner.Count(); i++)
            {
                NodeWorker worker = new NodeWorker(nodeInner[i], _tokenizer);
                foreach (TermNode node in worker.GenerateVariations())
                {
                    TermNode copyC = nodeInner.CreateCopy();
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
                foreach (INodeTransformationRule rule in _listSimplificationRules)
                {
                    if (rule.CanBeAppliedTo(node, _tokenizer))
                    {
                        TermNode copy = node.CreateCopy();
                        copy = rule.Transform(copy, _tokenizer);

                        if (!listVariations.Contains(copy))
                        {
                            listVariations.Add(copy);
                        }
                    }
                }
            }

            return listVariations;
        }

    }
}
