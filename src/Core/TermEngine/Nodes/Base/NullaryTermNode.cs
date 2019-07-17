/* Copyright (C) Marcel Bulla - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Marcel Bulla <postmaster@marcel-bulla.de>
 */

using System;

namespace De.Markellus.Maths.Core.TermEngine.Nodes.Base
{
    /// <summary>
    /// Baisklasse für Nodes die keine Kinder haben.
    /// </summary>
    public abstract class NullaryTermNode : TermNode
    {
        /// <summary>
        /// Gibt die Anzahl der direkten Kinder zurück.
        /// </summary>
        /// <returns>Die Anzahl der direkten Kinder dieses Nodes.</returns>
        public override int Count()
        {
            return 0;
        }

        /// <summary>
        /// Überprüft, ob zwei Nodes inhaltlich identisch sind.
        /// </summary>
        /// <param name="obj">Der andere Node, mit dem verglichen werden soll</param>
        /// <returns>true, wenn beide Nodes identisch sind, ansonsten false.</returns>
        public override bool Equals(object obj)
        {
            return obj is NullaryTermNode node;
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
            throw new ArgumentOutOfRangeException();
        }

        /// <summary>
        /// Legt den Kind-Node am Index <see cref="iIndex"/> fest.
        /// </summary>
        /// <param name="iIndex">Der Index des Kind-Nodes</param>
        /// <param name="node">Kind-Node</param>
        protected override void SetChild(int iIndex, TermNode node)
        {
            throw new ArgumentOutOfRangeException();
        }

        /// <summary>
        /// Gleichheitsoperator
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(NullaryTermNode left, NullaryTermNode right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Ungleichheitsoperator
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(NullaryTermNode left, NullaryTermNode right)
        {
            return !Equals(left, right);
        }
    }
}