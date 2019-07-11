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
    /// Node für den Subtraktions-Operator
    /// </summary>
    public class OperatorSubtractNode : BinaryTermNode
    {
        /// <summary>
        /// Erstellt eine neue Instanz und initialisiert alle Kinder mit null.
        /// </summary>
        public OperatorSubtractNode() : base()
        {
        }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        /// <param name="left">Das linke Kind des Nodes</param>
        /// <param name="right">Das rechte Kind des Nodes</param>
        public OperatorSubtractNode(TermNode left, TermNode right) : base(left, right)
        {
        }

        /// <summary>
        /// Löst den Node in eine reelle Zahl auf.
        /// </summary>
        /// <returns>Eine reelle Zahl, die das Ergebnis des (Teil)-Termes ist der durch diesen
        /// Node repräsentiert wird.</returns>
        public override Real Resolve()
        {
            return LeftChild.Resolve() - RightChild.Resolve();
        }

        /// <summary>
        /// Überprüft, ob zwei Nodes inhaltlich identisch sind.
        /// </summary>
        /// <param name="obj">Der andere Node, mit dem verglichen werden soll</param>
        /// <returns>true, wenn beide Nodes identisch sind, ansonsten false.</returns>
        public override bool Equals(object obj)
        {
            return obj is OperatorSubtractNode node && Equals(node);
        }

        /// <summary>
        /// Überprüft, ob zwei Nodes inhaltlich identisch sind.
        /// </summary>
        /// <param name="other">Der andere Node, mit dem verglichen werden soll</param>
        /// <returns>true, wenn beide Nodes identisch sind, ansonsten false.</returns>
        protected bool Equals(OperatorSubtractNode other)
        {
            return Equals(LeftChild, other.LeftChild) && Equals(RightChild, other.RightChild);
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
            return new OperatorSubtractNode(LeftChild.CreateCopy(), RightChild.CreateCopy());
        }

        /// <summary>
        /// Gleichheitsoperator
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(OperatorSubtractNode left, OperatorSubtractNode right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Ungleichheitsoperator
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(OperatorSubtractNode left, OperatorSubtractNode right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return "(" + LeftChild + ")-(" + RightChild + ")";
        }
    }
}
