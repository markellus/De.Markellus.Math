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
using System.Text;
using De.Markellus.Maths.Internals.TermParsing;
//using De.Markellus.Maths.KnowledgeBase.Algebra.TransformationRules;
using De.Markellus.Maths.System;

namespace De.Markellus.Maths.Internals.TermProcessing
{
    /// <summary>
    /// Ein Baumknoten eines mathematischen Termes.
    /// 
    /// Ein Node bildet einen Teil eines Terms ab, beispielsweise eine Zahl oder eine Variable.
    /// Nodes können beliebig viele Kinder haben.
    /// </summary>
    public class Node
    {
        private readonly Node[] _arrChildNodes;

        //private readonly TransformationRule[] _arrTransformationRules;

        private HashSet<Node> _setTransformations;

        /// <summary>
        /// Der Typ des Knotens
        /// </summary>
        internal TokenType Type { get; }

        /// <summary>
        /// Wert des Knotens
        /// </summary>
        internal string Value { get; }

        /// <summary>
        /// Die Umgebung, in der dieser Node gültig ist.
        /// </summary>
        internal MathEnvironment Environment { get; }

        #region Constructors

        private protected Node(TokenType type, string strValue, MathEnvironment environment, Node[] arrChildNodes) //, TransformationRule[] arrTransformationRules)
        {
            Type = type;
            Value = strValue;
            Environment = environment;
            _arrChildNodes = arrChildNodes;
            //_arrTransformationRules = arrTransformationRules;
            _setTransformations = null;
        }

        private Node(Node template)
        {
            _arrChildNodes = new Node[template._arrChildNodes.Length];

            for (int i = 0; i < _arrChildNodes.Length; i++)
            {
                _arrChildNodes[i] = new Node(template._arrChildNodes[i]);
            }

            Type = template.Type;
            Value = template.Value;
            Environment = template.Environment;
            //_arrTransformationRules = template._arrTransformationRules;
            _setTransformations = template._setTransformations;
        }

        private Node(Node template, Node[] arrChildNodes)
        {

            Type = template.Type;
            Value = template.Value;
            Environment = template.Environment;
            _arrChildNodes = arrChildNodes;
            //_arrTransformationRules = template._arrTransformationRules;
            _setTransformations = null;
        }

        #endregion

        #region Transformation

        //internal List<Node> Simplify()
        //{
        //    if (_setTransformations == null)
        //    {
        //        GenerateTransformations();
        //    }

        //    List<Node> listSimplified = new List<Node>();
        //    int iNodeCount = int.MaxValue;

        //    foreach (Node node in _setTransformations)
        //    {
        //        int iChildCount = node.RecursiveCount();
        //        if (iChildCount < iNodeCount)
        //        {
        //            iNodeCount = iChildCount;
        //            listSimplified.Clear();
        //        }

        //        if (iChildCount == iNodeCount)
        //        {
        //            listSimplified.Add(node);
        //        }
        //    }

        //    return listSimplified;
        //}

        //private void GenerateTransformations()
        //{
        //    GenerateTransformations(new HashSet<Node>());
        //}

        //private void GenerateTransformations(HashSet<Node> setAll)
        //{
        //    Func<HashSet<Node>, Node, Node> GetNode = delegate(HashSet<Node> setAll, Node nodeIn)
        //    {
        //        if (setAll.TryGetValue(nodeIn, out Node node))
        //        {
        //            return node;
        //        }
        //        setAll.Add(nodeIn);
        //        return nodeIn;
        //    };

        //    if (_setTransformations == null)
        //    {
        //        _setTransformations = new HashSet<Node>();
        //    }

        //    //Infos
        //    int iChildCount = Count();

        //    //Wir selbst sind eine Transformation
        //    _setTransformations.Add(this);

        //    HashSet<Node> listTransformationsChildNodes = new HashSet<Node>();

        //    //Alle Transformationen aller Child Nodes ermitteln und in einer Liste abspeichern.
        //    HashSet<Node>[] arrListTransformationCombinations = new HashSet<Node>[iChildCount];

        //    for (int i = 0; i < iChildCount; i++)
        //    {
        //        Node nodeChild = GetNode(setAll, this[i]);

        //        //Alle Transformationen eines Child Nodes ermitteln
        //        nodeChild.GenerateTransformations();
        //        arrListTransformationCombinations[i] = nodeChild._setTransformations;
        //        //Die Original-Transformation ist natürlich auch eine Option
        //        arrListTransformationCombinations[i].Add(new Node(nodeChild));
        //    }
        //}

        #endregion

        #region Child Handler

        /// <summary>
        /// Gibt die Anzahl der direkten Kinder zurück.
        /// </summary>
        /// <returns>Die Anzahl der direkten Kinder dieses Nodes.</returns>
        internal int Count()
        {
            return _arrChildNodes.Length;
        }

        internal int RecursiveCount()
        {
            int iCount = Count();

            foreach (Node node in _arrChildNodes)
            {
                iCount += node.RecursiveCount();
            }

            return iCount;
        }

        /// <summary>
        /// Ruft ein Kind an Index <see cref="iIndex"/> ab, oder legt das Kind an Index
        /// <see cref="iIndex"/> fest.
        /// </summary>
        /// <param name="iIndex">Der Index des Kind-Nodes</param>
        /// <returns>Kind-Node</returns>
        internal Node this[int iIndex]
        {
            get
            {
                if (iIndex >= Count())
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _arrChildNodes[iIndex];
            }
        }

        #endregion

        #region Equality Checks

        /// <summary>
        /// Überprüft, ob zwei Nodes inhaltlich identisch sind.
        /// </summary>
        /// <param name="obj">Der andere Node, mit dem verglichen werden soll</param>
        /// <returns>true, wenn beide Nodes identisch sind, ansonsten false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Node n)
            {
                return Equals(n);
            }

            return false;
        }

        /// <summary>
        /// Überprüft, ob zwei Nodes inhaltlich identisch sind.
        /// </summary>
        /// <param name="other">Der andere Node, mit dem verglichen werden soll</param>
        /// <returns>true, wenn beide Nodes identisch sind, ansonsten false.</returns>
        protected bool Equals(Node other)
        {
            if (Count() == other.Count())
            {
                int iCount = Count();
                for (int i = 0; i < iCount; i++)
                {
                    if (this[i].Equals(other[i]))
                    {
                        return false;
                    }
                }

                return Equals(Type == other.Type && string.Equals(Value, other.Value));
            }

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (_arrChildNodes != null ? _arrChildNodes.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int) Type;
                hashCode = (hashCode * 397) ^ (Value != null ? Value.GetHashCode() : 0);
                return hashCode;
            }
        }

        #endregion

        #region Operators

        /// <summary>
        /// Gleichheitsoperator
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Node left, Node right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Ungleichheitsoperator
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Node left, Node right)
        {
            return !Equals(left, right);
        }

        #endregion

        #region Representation

        private void FormatChild(StringBuilder builder, int iIndex)
        {
            if (this[iIndex].RecursiveCount() > 1 && this[iIndex].Type != TokenType.Function &&
                MathExpressionTokenizer.GetToken(Value)?.Precedence >= TokenPrecedence.Multiplication)
            {
                builder.Append("(");
                builder.Append(this[iIndex]);
                builder.Append(")");
            }
            else
            {
                builder.Append(this[iIndex]);
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            switch (Type)
            {
                case TokenType.Operator:
                    FormatChild(builder, 0);
                    builder.Append(" ");
                    builder.Append(Value);
                    builder.Append(" ");
                    FormatChild(builder, 1);
                    break;
                case TokenType.Function:
                    builder.Append(Value);
                    builder.Append("(");
                    for (int i = 0; i < Count(); i++)
                    {
                        FormatChild(builder, i);
                        if (i != Count() - 1)
                        {
                            builder.Append(", ");
                        }
                    }

                    builder.Append(")");
                    break;
                default:
                    builder.Append(Value);
                    break;
            }

            return builder.ToString();
        }

        #endregion
    }
}
