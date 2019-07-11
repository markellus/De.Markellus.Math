/* Copyright (C) Marcel Bulla - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Marcel Bulla <postmaster@marcel-bulla.de>
 */

using System;

namespace De.Markellus.Maths.Core.TermEngine.Nodes.Base
{
    /// <summary>
    /// Basisklasse für alle Term-Nodes mit zwei Kindern.
    /// </summary>
    public abstract class BinaryTermNode : TermNode
    {
        /// <summary>
        /// Das linke Kind des Nodes
        /// </summary>
        public TermNode LeftChild { get; private set; }

        /// <summary>
        /// Das rechte Kind des Nodes
        /// </summary>
        public TermNode RightChild { get; private set; }

        /// <summary>
        /// Erstellt eine neue Instanz und initialisiert alle Kinder mit null.
        /// </summary>
        protected BinaryTermNode()
        {
            LeftChild = null;
            RightChild = null;
        }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        /// <param name="left">Das linke Kind des Nodes</param>
        /// <param name="right">Das rechte Kind des Nodes</param>
        protected BinaryTermNode(TermNode left, TermNode right)
        {
            LeftChild = left;
            RightChild = right;
        }

        /// <summary>
        /// Ruft ab, ob sich der Node zu einer reellen Zahl auflösen lässt.
        /// </summary>
        /// <returns>true wenn sich der Node zu einer reellen Zahl auflösen lässt, ansonsten false.</returns>
        public override bool IsResolvable()
        {
            return LeftChild != null && RightChild != null && LeftChild.IsResolvable() && RightChild.IsResolvable();
        }

        /// <summary>
        /// Gibt die Anzahl der direkten Kinder zurück.
        /// </summary>
        /// <returns>Die Anzahl der direkten Kinder dieses Nodes.</returns>
        public override int Count()
        {
            return 2;
        }

        /// <summary>
        /// Überprüft, ob zwei Nodes inhaltlich identisch sind.
        /// </summary>
        /// <param name="obj">Der andere Node, mit dem verglichen werden soll</param>
        /// <returns>true, wenn beide Nodes identisch sind, ansonsten false.</returns>
        public override bool Equals(object obj)
        {
            return obj is BinaryTermNode node && Equals(node);
        }

        /// <summary>
        /// Überprüft, ob zwei Nodes inhaltlich identisch sind.
        /// </summary>
        /// <param name="other">Der andere Node, mit dem verglichen werden soll</param>
        /// <returns>true, wenn beide Nodes identisch sind, ansonsten false.</returns>
        protected bool Equals(BinaryTermNode other)
        {
            return Equals(LeftChild, other.LeftChild) && Equals(RightChild, other.RightChild);
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Ruft den Kind-Node am Index <see cref="iIndex"/> ab.
        /// </summary>
        /// <param name="iIndex">Der Index des Kind-Nodes</param>
        /// <returns>Der Kind-Node am Index <see cref="iIndex"/></returns>
        protected override TermNode GetChild(int iIndex)
        {
            return iIndex == 0 ? LeftChild : RightChild;
        }

        /// <summary>
        /// Legt den Kind-Node am Index <see cref="iIndex"/> fest.
        /// </summary>
        /// <param name="iIndex">Der Index des Kind-Nodes</param>
        /// <param name="node">Kind-Node</param>
        protected override void SetChild(int iIndex, TermNode node)
        {
            if (iIndex == 0)
            {
                LeftChild = node;
            }
            else
            {
                RightChild = node;
            }
        }

        /// <summary>
        /// Gleichheitsoperator
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(BinaryTermNode left, BinaryTermNode right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Ungleichheitsoperator
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(BinaryTermNode left, BinaryTermNode right)
        {
            return !Equals(left, right);
        }
    }
}
