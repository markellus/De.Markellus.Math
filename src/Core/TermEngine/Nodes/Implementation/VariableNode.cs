/* Copyright (C) Marcel Bulla - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Marcel Bulla <postmaster@marcel-bulla.de>
 */

using System;
using De.Markellus.Maths.Core.Arithmetic;
using De.Markellus.Maths.Core.TermEngine.Nodes.Base;

namespace De.Markellus.Maths.Core.TermEngine.Nodes.Implementation
{
    /// <summary>
    /// Node für eine Variable.
    /// </summary>
    public class VariableNode : NullaryTermNode
    {
        /// <summary>
        /// Die String-Darstellung der Variable.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Erstellt eine neue Instanz mit dem angegebenen Wert.
        /// </summary>
        /// <param name="strVar">Die Variable, die durch den Node repräsentiert wird.</param>
        public VariableNode(string strVar)
        {
            Value = strVar;
        }

        /// <summary>
        /// Ruft ab, ob sich der Node zu einer reellen Zahl auflösen lässt.
        /// </summary>
        /// <returns>true wenn sich der Node zu einer reellen Zahl auflösen lässt, ansonsten false.</returns>
        public override bool IsResolvable()
        {
            //TODO: Variablen auflösen
            return false;
        }

        /// <summary>
        /// Löst den Node in eine reelle Zahl auf.
        /// </summary>
        /// <returns>Eine reelle Zahl, die das Ergebnis des (Teil)-Termes ist der durch diesen
        /// Node repräsentiert wird.</returns>
        public override Real Resolve()
        {
            //TODO: Variablen auflösen
            throw new NotImplementedException();
        }

        /// <summary>
        /// Überprüft, ob zwei Nodes inhaltlich identisch sind.
        /// </summary>
        /// <param name="obj">Der andere Node, mit dem verglichen werden soll</param>
        /// <returns>true, wenn beide Nodes identisch sind, ansonsten false.</returns>
        public override bool Equals(object obj)
        {
            return obj is VariableNode node && Equals(node);
        }

        /// <summary>
        /// Überprüft, ob zwei Nodes inhaltlich identisch sind.
        /// </summary>
        /// <param name="other">Der andere Node, mit dem verglichen werden soll</param>
        /// <returns>true, wenn beide Nodes identisch sind, ansonsten false.</returns>
        protected bool Equals(VariableNode other)
        {
            return Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Erstellt eine exakte Kopie des Nodes.
        /// </summary>
        /// <returns>Eine exakte Kopie des Nodes.</returns>
        public override TermNode CreateCopy()
        {
            return new VariableNode(Value);
        }

        /// <summary>
        /// Gleichheitsoperator
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(VariableNode left, VariableNode right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Ungleichheitsoperator
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(VariableNode left, VariableNode right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
