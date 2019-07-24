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

using De.Markellus.Maths.Internals.TermParsing;
//using De.Markellus.Maths.KnowledgeBase.Algebra.TransformationRules;

namespace De.Markellus.Maths.Internals.TermProcessing
{
    /// <summary>
    /// Eine Vorlage zur Erstellung eines Baumknotens, der einen Teil eines mathematischen Terms repräsentiert.
    /// </summary>
    internal class NodeTemplate
    {
        /// <summary>
        /// Der Typ des Knotens
        /// </summary>
        public TokenType Type { get; }

        /// <summary>
        /// Erwarteter Wert des Knotens
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Legt fest, ob der erwartete Wert des Knotens festgelegt ist oder ein beliebiger Wert gewählt werden kann
        /// </summary>
        public bool IsDynamicValue { get; }

        /// <summary>
        /// Die Anzahl der Kinder, die ein Knoten mit diesen Werten besitzen muss.
        /// </summary>
        public int ChildrenCount { get; }

        //public TransformationRule[] TransformationRules { get; }

        /// <summary>
        /// Erstellt eine Vorlage zur Erstellung eines Baumknotens, der einen Teil eines mathematischen Terms repräsentiert.
        /// </summary>
        /// <param name="type">Der Typ des Knotens</param>
        /// <param name="strValue">Erwarteter Wert des Knotens</param>
        /// <param name="bIsDynamicValue">Legt fest, ob der erwartete Wert des Knotens festgelegt ist oder ein beliebiger Wert gewählt werden kann</param>
        /// <param name="iChildrenCount">Die Anzahl der Kinder, die ein Knoten mit diesen Werten besitzen muss.</param>
        public NodeTemplate(TokenType type, string strValue, bool bIsDynamicValue, int iChildrenCount)
        {
            Type = type;
            Value = strValue;
            IsDynamicValue = bIsDynamicValue;
            ChildrenCount = iChildrenCount;
            //TransformationRules = TransformationKnowledgeBase.GetTransformationRules(Type, Value).ToArray();
        }
    }
}
