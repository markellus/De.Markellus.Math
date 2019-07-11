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
    /// Node für reelle Zahlen
    /// </summary>
    public class NumberNode : NullaryTermNode
    {
        /// <summary>
        /// Wert der reellen Zahl
        /// </summary>
        public Real Value { get; set; }

        /// <summary>
        /// Erstellt eine neue Instanz mit dem angegebenen Wert.
        /// </summary>
        /// <param name="value">Die reelle Zahl, die durch den Node repräsentiert wird.</param>
        public NumberNode(Real value = null)
        {
            if (value == null)
            {
                value = RealFactory.GenerateReal("0.0");
            }
            Value = value;
        }

        /// <summary>
        /// Ruft ab, ob sich der Node zu einer reellen Zahl auflösen lässt.
        /// </summary>
        /// <returns>true wenn sich der Node zu einer reellen Zahl auflösen lässt, ansonsten false.</returns>
        public override bool IsResolvable()
        {
            return Value != null;
        }

        /// <summary>
        /// Löst den Node in eine reelle Zahl auf.
        /// </summary>
        /// <returns>Eine reelle Zahl, die das Ergebnis des (Teil)-Termes ist der durch diesen
        /// Node repräsentiert wird.</returns>
        public override Real Resolve()
        {
            return Value;
        }

        /// <summary>
        /// Überprüft, ob zwei Nodes inhaltlich identisch sind.
        /// </summary>
        /// <param name="obj">Der andere Node, mit dem verglichen werden soll</param>
        /// <returns>true, wenn beide Nodes identisch sind, ansonsten false.</returns>
        public override bool Equals(object obj)
        {
            return obj is NumberNode node && Equals(node);
        }

        /// <summary>
        /// Überprüft, ob zwei Nodes inhaltlich identisch sind.
        /// </summary>
        /// <param name="other">Der andere Node, mit dem verglichen werden soll</param>
        /// <returns>true, wenn beide Nodes identisch sind, ansonsten false.</returns>
        protected bool Equals(NumberNode other)
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
            return new NumberNode(RealFactory.GenerateReal(Value));
        }

        /// <summary>
        /// Gleichheitsoperator
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(NumberNode left, NumberNode right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Ungleichheitsoperator
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(NumberNode left, NumberNode right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
