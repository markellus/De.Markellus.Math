/* 
 * This file is part of De.Markellus.Math (https://github.com/markellus/De.Markellus.Math).
 * Copyright (c) 2019 Marcel Bulla.
 * 
 * This program is free software: you can redistribute it and/or modify  
 * it under the terms of the GNU General Public License as published by  
 * the Free Software Foundation, version 3.
 *
 * This program is distributed in the hope that it will be useful, but 
 * WITHOUT ANY WARRANTY; without even the implied warranty of 
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU 
 * General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License 
 * along with this program. If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.IO;
using De.Markellus.Maths.Internals.TermParsing;
using De.Markellus.Maths.KnowledgeBase.Algebra.Nodes;
//using De.Markellus.Maths.KnowledgeBase.Algebra.TransformationRules;
using De.Markellus.Maths.System;

namespace De.Markellus.Maths.Internals.TermProcessing
{
    /// <summary>
    /// Fabrik-Klasse zur Erstellung von mathematischen Termen als Baumdarstellung.
    /// </summary>
    internal static class NodeFactory
    {
        /// <summary>
        /// Ein Wörterbuch mit Vorlagen
        /// </summary>
        private static Dictionary<TokenType, List<NodeTemplate>> _dicTemplates;

        static NodeFactory()
        {
            _dicTemplates = new Dictionary<TokenType, List<NodeTemplate>>();
            NodeKnowledgeBase.LoadKnowledge();
        }

        /// <summary>
        /// Erstellt eine Baumstruktur, die einen mathematischen Term repräsentiert.
        /// </summary>
        /// <param name="strInfixTerm">Der mathematische Term in der Infix-Notation.</param>
        /// <param name="environment">Die Umgebung, in dessen Kontext der mathematische Term seine Gültigkeit besitzt.</param>
        /// <param name="arrVariables">Eine Liste mit Variablen, die in dem Term vorkommen.</param>
        /// <returns>Eine Baumstruktur, die einen mathematischen Term repräsentiert.</returns>
        public static Node Get(string strInfixTerm, MathEnvironment environment, params string[] arrVariables)
        {
            return Get(new StringReader(strInfixTerm), environment, arrVariables);
        }

        /// <summary>
        /// Erstellt eine Baumstruktur, die einen mathematischen Term repräsentiert.
        /// </summary>
        /// <param name="readerInfixTerm">Der mathematische Term in der Infix-Notation.</param>
        /// <param name="environment">Die Umgebung, in dessen Kontext der mathematische Term seine Gültigkeit besitzt.</param>
        /// <param name="arrVariables">Eine Liste mit Variablen, die in dem Term vorkommen.</param>
        /// <returns>Eine Baumstruktur, die einen mathematischen Term repräsentiert.</returns>
        public static Node Get(TextReader readerInfixTerm, MathEnvironment environment, params string[] arrVariables)
        {
            return Get(MathExpressionTokenizer.Tokenize(readerInfixTerm, true, arrVariables), environment, true);
        }

        /// <summary>
        /// Erstellt eine Baumstruktur, die einen mathematischen Term repräsentiert.
        /// </summary>
        /// <param name="token">Eine Liste an Token, die den mathematischen Term abbilden.</param>
        /// <param name="environment">Die Umgebung, in dessen Kontext der mathematische Term seine Gültigkeit besitzt.</param>
        /// <param name="bInfix">true, wenn der Term in der Infix-Notation formatiert ist, ansonsten false.</param>
        /// <returns>Eine Baumstruktur, die einen mathematischen Term repräsentiert.</returns>
        public static Node Get(IEnumerable<Token> token, MathEnvironment environment, bool bInfix)
        {
            if (bInfix)
            {
                token = ShuntingYardParser.InfixToRpn(token);
            }

            Stack<Node> stack = new Stack<Node>();

            foreach (Token t in token)
            {
                stack.Push(GenerateNodeFromToken(t, stack, environment));
            }

            if (stack.Count != 1)
            {
                throw new ArithmeticException("Unable to parse the given term.");
            }

            return stack.Pop();
        }

        /// <summary>
        /// Stellt der Fabrik eine neue Vorlage zur Verfügung, aus der Baumstrukturen erstellt werden.
        /// </summary>
        /// <param name="template">Die Vorlage, die henzugefügt werden soll.</param>
        internal static void AddTemplate(NodeTemplate template)
        {
            if (!_dicTemplates.ContainsKey(template.Type))
            {
                _dicTemplates.Add(template.Type, new List<NodeTemplate>());
            }

            _dicTemplates[template.Type].Add(template);
        }

        /// <summary>
        /// Sucht die passende Vorlage zu einem Token, um diesen als Baumknoten abzubilden.
        /// </summary>
        /// <param name="t">Der Token, zu dem die passende Vorlage gefunden werden soll.</param>
        /// <returns>Eine Vorlage oder null, falls keine passende Vorlage gefunden werden konnte.</returns>
        private static NodeTemplate GetTemplateFromToken(Token t)
        {
            return _dicTemplates.ContainsKey(t.Type)
                ? _dicTemplates[t.Type].Find(x => x.IsDynamicValue || x.Value == t.Value)
                : null;
        }

        /// <summary>
        /// Erstellt einen Baumknoten aus einem Token.
        /// </summary>
        /// <param name="t">Der Token</param>
        /// <param name="stack">Stack mit bereits konvertierten Baumknoten</param>
        /// <param name="environment">Die Umgebung, in dessen Kontext der mathematische Term seine Gültigkeit besitzt.</param>
        /// <returns>Ein neuer Baumknoten.</returns>
        private static Node GenerateNodeFromToken(Token t, Stack<Node> stack, MathEnvironment environment)
        {
            NodeTemplate template = GetTemplateFromToken(t);

            if (template == null)
            {
                throw new NotImplementedException(
                    $"There is no node knowledge for the token {t.Value} inside the knowledge base.");
            }

            if (template.ChildrenCount > stack.Count)
            {
                throw new ArgumentException(
                    $"The stack does not contain enough nodes to create a new node of type {t.Type} with " +
                    $"the value {t.Value}. The knowledge base may have not enough information for a function " +
                    $"to parse it properly, or the term is malformed.");
            }

            Node[] arrChildNodes = new Node[template.ChildrenCount];

            for (int i = arrChildNodes.Length - 1; i >= 0; i--)
            {
                arrChildNodes[i] = stack.Pop();
            }

            return new FactoryNode(template.Type, t.Value, environment, arrChildNodes);//, template.TransformationRules);
        }

        private class FactoryNode : Node
        {
            public FactoryNode(TokenType type, string strValue, MathEnvironment environment, Node[] arrChildNodes)//, TransformationRule[] arrTransformationRules)
                : base(type, strValue, environment, arrChildNodes)//, arrTransformationRules)
            {
            }
        }
    }
}
