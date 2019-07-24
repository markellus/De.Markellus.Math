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

using System.Collections.Generic;
using System.IO;
using System.Linq;
using De.Markellus.Maths.Internals.TermParsing;
using De.Markellus.Maths.Internals.TermProcessing;

namespace De.Markellus.Maths.System
{
    /// <summary>
    /// Die Mathematik-Umgebung ist der Ausgangspunkt für das Arbeiten mit mathematischen Ausdrücken.
    /// Hier werden neue Terme und Variablen definiert und verarbeitet.
    /// </summary>
    public class MathEnvironment
    {
        private HashSet<Node> _setTerms;

        private Dictionary<string, object> _dicVariables;

        /// <summary>
        /// Erstellt eine neue Mathematik-Umgebung.
        /// </summary>
        public MathEnvironment()
        {
            _setTerms = new HashSet<Node>();
            _dicVariables = new Dictionary<string, object>();
        }

        #region Terms

        /// <summary>
        /// Fügt der Umgebung einen neuen Term hinzu oder ruft einen bereits vorhandenen Term ab, der dem übergebenen Ausdruck entspricht.
        /// </summary>
        /// <param name="strInfixTerm">Der Term in der Infix-Notation.</param>
        /// <returns>Der oberste Knoten des neuen Terms.</returns>
        public Node AddTerm(string strInfixTerm)
        {
            Node node = NodeFactory.Get(strInfixTerm, this, _dicVariables.Keys.ToArray());
            return RegisterTerm(node);
        }

        /// <summary>
        /// Fügt der Umgebung einen neuen Term hinzu oder ruft einen bereits vorhandenen Term ab, der dem übergebenen Ausdruck entspricht.
        /// </summary>
        /// <param name="readerInfixTerm">Der Term in der Infix-Notation.</param>
        /// <returns>Der oberste Knoten des neuen Terms.</returns>
        public Node AddTerm(TextReader readerInfixTerm)
        {
            Node node = NodeFactory.Get(readerInfixTerm, this, _dicVariables.Keys.ToArray());
            return RegisterTerm(node);
        }

        /// <summary>
        /// Fügt der Umgebung einen neuen Term hinzu oder ruft einen bereits vorhandenen Term ab, der dem übergebenen Ausdruck entspricht.
        /// </summary>
        /// <param name="token">Eine Liste an Token, die den mathematischen Term abbilden.</param>
        /// <param name="bInfix">true, wenn der Term in der Infix-Notation formatiert ist, ansonsten false.</param>
        /// <returns>Der oberste Knoten des neuen Terms.</returns>
        public Node AddTerm(IEnumerable<Token> token, bool bInfix)
        {
            Node node = NodeFactory.Get(token, this, bInfix);
            return RegisterTerm(node);
        }

        //public List<Node> SimplifyTerm(Node node)
        //{
        //    return node.Simplify();
        //}

        private Node RegisterTerm(Node node)
        {
            if (_setTerms.TryGetValue(node, out Node nodeEqual))
            {
                return nodeEqual;
            }

            _setTerms.Add(node);
            return node;
        }

        #endregion

        #region Variables

        /// <summary>
        /// Definiert eine neue Variable.
        /// </summary>
        /// <param name="strVariable">Die String-Darstellung der Variable.</param>
        public void DefineVariable(string strVariable)
        {
            if (!_dicVariables.ContainsKey(strVariable))
            {
                _dicVariables.Add(strVariable, null);
            }
        }

        #endregion
    }
}
