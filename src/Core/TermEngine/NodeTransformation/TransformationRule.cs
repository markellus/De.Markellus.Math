using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using De.Markellus.Maths.Core.TermEngine.Nodes;
using De.Markellus.Maths.Core.TermEngine.Nodes.Base;
using De.Markellus.Maths.Core.TermEngine.Nodes.Implementation;
using De.Markellus.Maths.Core.TermEngine.TermParsing;

namespace De.Markellus.Maths.Core.TermEngine.NodeTransformation
{
    internal class TransformationRule
    {
        //private const string VARIABLE_FIX = "@VAR-æØ§½Ü@";

        private readonly MathEnvironment _env;
        private readonly TermNode _termIn;
        private readonly TermNode _termOut;

        private readonly List<string> _listVariablesResolve;
        private List<Term> _listRestrictions;

        public bool Valid { get; }

        public string InternalName { get; }

        public TransformationRule(XmlNode xmlSource, bool bBackwards)
        {
            _env = new MathEnvironment();
            _termIn = null;
            _termOut = null;
            _listVariablesResolve = new List<string>();
            _listRestrictions = new List<Term>();
            Valid = true;

            XmlNode xmlMeta = xmlSource.SelectSingleNode("meta");
            InternalName = xmlMeta?.SelectSingleNode("internalname")?.InnerText ?? "Undefined";

            XmlNode xmlVariables = xmlSource.SelectSingleNode("variables");
            XmlNodeList listVariables = xmlVariables?.SelectNodes("variable");

            if (listVariables != null)
            {
                foreach (XmlNode node in listVariables)
                {
                    _env.DefineVariable(node.InnerText);
                }
            }

            XmlNode xmlRestrictions = xmlSource.SelectSingleNode("restrictions");
            XmlNodeList listRestrictions = xmlRestrictions?.SelectNodes("restriction");

            if(listRestrictions != null)
            {
                foreach(XmlNode node in listRestrictions)
                {
                    _listRestrictions.Add(_env.DefineTerm(node.InnerText));
                }
            }

            XmlNode xmlResolve = xmlSource.SelectSingleNode("resolve");
            XmlNodeList listResolve = xmlResolve?.SelectNodes("variable");

            if (listResolve != null)
            {
                foreach (XmlNode node in listResolve)
                {
                    _listVariablesResolve.Add(node.InnerText);
                }
            }

            XmlNode xmlLeft = xmlSource.SelectSingleNode("left");
            XmlNode xmlRight = xmlSource.SelectSingleNode("right");

            if (xmlLeft == null || xmlRight == null)
            {
                Valid = false;
                return;
            }

            try
            {
                if (bBackwards)
                {
                    _termIn = GenerateKnowledge(xmlRight.InnerText);
                    _termOut = GenerateKnowledge(xmlLeft.InnerText);
                }
                else
                {
                    _termIn = GenerateKnowledge(xmlLeft.InnerText);
                    _termOut = GenerateKnowledge(xmlRight.InnerText);
                }
            }
            catch
            {
                Valid = false;
            }
        }

        public bool CanBeAppliedTo(TermNode node)
        {
            Dictionary<string, TermNode> dicVariables = new Dictionary<string, TermNode>();
            bool bVariablesMatch = GetVariablesFromInput(node, _termIn, ref dicVariables);
            bool bAppliable = bVariablesMatch ? CanBeAppliedTo(node, _termIn) : false;
            bool bFulfillsRestrictions = true;

            if (bAppliable && bVariablesMatch)
            {
                foreach (KeyValuePair<string, TermNode> kvp in dicVariables)
                {
                    _env.DefineVariable(kvp.Key, new Term(kvp.Value));
                }
                foreach(Term term in _listRestrictions)
                {
                    if(term.Resolve() == "0")
                    {
                        bFulfillsRestrictions = false;
                        break;
                    }
                }
                foreach (KeyValuePair<string, TermNode> kvp in dicVariables)
                {
                    _env.RemoveVariable(kvp.Key);
                }
                File.AppendAllText("D:/debug-CanBeApplioedTo.log",
                    $"CanBeAppliedTo: {node} => {bAppliable && bVariablesMatch} | {InternalName}\r\n");
            }

            return bAppliable && bVariablesMatch && bFulfillsRestrictions;
        }

        public TermNode Transform(TermNode node, MathExpressionTokenizer tokenizer)
        {
            TermNode nodeTransformed = _termOut.CreateCopy();
            
            Dictionary<string, TermNode> dicVariables = new Dictionary<string, TermNode>();
            GetVariablesFromInput(node, _termIn, ref dicVariables);
            nodeTransformed = ReplaceVariablesInOutput(nodeTransformed, _termOut, ref dicVariables);

            File.AppendAllText("D:/debug-Transform.log", $"Transformed: {node} => {nodeTransformed} | {InternalName}\r\n");
            return nodeTransformed;
        }

        private TermNode GenerateKnowledge(string strInput)
        {
            Term term = _env.DefineTerm(strInput);
            TermNode node = term.ParseTerm();
            return node;
        }

        private bool CanBeAppliedTo(TermNode nodeIn, TermNode nodeKnowledge)
        {
            if (nodeIn.GetType() == nodeKnowledge.GetType() && !(nodeKnowledge is VariableNode))
            {
                for (int i = 0; i < nodeIn.Count(); i++)
                {
                    if (!CanBeAppliedTo(nodeIn[i], nodeKnowledge[i]))
                    {
                        return false;
                    }
                }

                if (nodeKnowledge.IsResolvable())
                {
                    return nodeIn.IsResolvable() && nodeIn.Resolve() == nodeKnowledge.Resolve();
                }

                return true;
            }

            if (nodeKnowledge is VariableNode varNode)
            {
                return !_listVariablesResolve.Contains(varNode.Value) || nodeIn.IsResolvable();
            }

            return false;
        }

        private bool GetVariablesFromInput(TermNode nodeIn, TermNode nodeKnowledge, ref Dictionary<string, TermNode> dicVariables)
        {
            if (nodeKnowledge is VariableNode varNode)
            {
                if (dicVariables.ContainsKey(varNode.Value))
                {
                    return nodeIn == dicVariables[varNode.Value];
                }
                else
                {
                    dicVariables.Add(varNode.Value, nodeIn.CreateCopy());
                    return true;
                }
            }

            if (nodeKnowledge.Count() != nodeIn.Count())
            {
                return false;
            }

            for (int i = 0; i < nodeKnowledge.Count(); i++)
            {
                if (!GetVariablesFromInput(nodeIn[i], nodeKnowledge[i], ref dicVariables))
                {
                    return false;
                }
            }

            return true;
        }

        private TermNode ReplaceVariablesInOutput(TermNode termOut, TermNode nodeKnowledge,
            ref Dictionary<string, TermNode> dicVariables)
        {
            for (int i = 0; i < termOut.Count(); i++)
            {
                termOut[i] = ReplaceVariablesInOutput(termOut[i], nodeKnowledge[i], ref dicVariables);
            }

            if (nodeKnowledge is VariableNode varNode)
            {
                if (_listVariablesResolve.Contains(varNode.Value))
                {
                    return TermNodeFactory.CreateNumberNode(dicVariables[varNode.Value].Resolve());
                }
                return dicVariables[varNode.Value];
            }

            return termOut;
        }

        public override string ToString()
        {
            return InternalName;
        }
    }
}
