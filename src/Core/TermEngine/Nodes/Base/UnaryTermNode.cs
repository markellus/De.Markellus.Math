/* Copyright (C) Marcel Bulla - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Marcel Bulla <postmaster@marcel-bulla.de>
 */

using System;

namespace De.Markellus.Maths.Core.TermEngine.Nodes.Base
{
    /// <summary>
    /// Basisklasse für Ndoes mit einem Kind.
    /// </summary>
    public abstract class UnaryTermNode : TermNode
    {
        /// <summary>
        /// Das Kind dieses Nodes.
        /// </summary>
        public TermNode Child { get; private set; }

        /// <summary>
        /// Erstellt eine neue Instanz und initialisiert alle Kinder mit null.
        /// </summary>
        protected UnaryTermNode()
        {
            Child = null;
        }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        /// <param name="child">Das Kind dieses Nodes.</param>
        protected UnaryTermNode(TermNode child)
        {
            Child = child;
        }

        /// <summary>
        /// Ruft ab, ob sich der Node zu einer reellen Zahl auflösen lässt.
        /// </summary>
        /// <returns>true wenn sich der Node zu einer reellen Zahl auflösen lässt, ansonsten false.</returns>
        public override bool IsResolvable()
        {
            return Child != null && Child.IsResolvable();
        }

        /// <summary>
        /// Gibt die Anzahl der direkten Kinder zurück.
        /// </summary>
        /// <returns>Die Anzahl der direkten Kinder dieses Nodes.</returns>
        public override int Count()
        {
            return 1;
        }

        /// <summary>
        /// Überprüft, ob zwei Nodes inhaltlich identisch sind.
        /// </summary>
        /// <param name="obj">Der andere Node, mit dem verglichen werden soll</param>
        /// <returns>true, wenn beide Nodes identisch sind, ansonsten false.</returns>
        public override bool Equals(object obj)
        {
            return obj is UnaryTermNode node && Equals(node);
        }

        /// <summary>
        /// Überprüft, ob zwei Nodes inhaltlich identisch sind.
        /// </summary>
        /// <param name="other">Der andere Node, mit dem verglichen werden soll</param>
        /// <returns>true, wenn beide Nodes identisch sind, ansonsten false.</returns>
        protected bool Equals(UnaryTermNode other)
        {
            return Equals(Child, other.Child);
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
            if (iIndex == 0)
            {
                return Child;
            }
            throw new ArgumentOutOfRangeException();
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
                Child = node;
            }
            throw new ArgumentOutOfRangeException();
        }

        /// <summary>
        /// Gleichheitsoperator
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(UnaryTermNode left, UnaryTermNode right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Ungleichheitsoperator
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(UnaryTermNode left, UnaryTermNode right)
        {
            return !Equals(left, right);
        }
    }
}
