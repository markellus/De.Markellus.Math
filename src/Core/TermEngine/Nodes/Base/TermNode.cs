/* Copyright (C) Marcel Bulla - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Marcel Bulla <postmaster@marcel-bulla.de>
 */

using System;
using De.Markellus.Maths.Core.Arithmetic;

namespace De.Markellus.Maths.Core.TermEngine.Nodes.Base
{
    /// <summary>
    /// Basisklasse für alle Term-Nodes.
    /// Ein Term-Node bildet einen Teil eines Terms ab, beispielsweise eine Zahl oder eine Variable.
    /// Nodes können beliebig viele Kinder haben.
    /// </summary>
    public abstract class TermNode
    {
        /// <summary>
        /// Die Umgebung, in der dieser Node gültig ist.
        /// </summary>
        public MathEnvironment MathEnvironment { get; internal set; }

        /// <summary>
        /// Ruft ab, ob sich der Node zu einer reellen Zahl auflösen lässt.
        /// </summary>
        /// <returns>true wenn sich der Node zu einer reellen Zahl auflösen lässt, ansonsten false.</returns>
        public abstract bool IsResolvable();

        /// <summary>
        /// Löst den Node in eine reelle Zahl auf.
        /// </summary>
        /// <returns>Eine reelle Zahl, die das Ergebnis des (Teil)-Termes ist der durch diesen
        /// Node repräsentiert wird.</returns>
        public abstract Real Resolve();

        /// <summary>
        /// Gibt die Anzahl der direkten Kinder zurück.
        /// </summary>
        /// <returns>Die Anzahl der direkten Kinder dieses Nodes.</returns>
        public abstract int Count();

        /// <summary>
        /// Überprüft, ob zwei Nodes inhaltlich identisch sind.
        /// </summary>
        /// <param name="obj">Der andere Node, mit dem verglichen werden soll</param>
        /// <returns>true, wenn beide Nodes identisch sind, ansonsten false.</returns>
        public abstract override bool Equals(object obj);

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Erstellt eine exakte Kopie des Nodes.
        /// </summary>
        /// <returns>Eine exakte Kopie des Nodes.</returns>
        public abstract TermNode CreateCopy();

        /// <summary>
        /// Ruft den Kind-Node am Index <see cref="iIndex"/> ab.
        /// </summary>
        /// <param name="iIndex">Der Index des Kind-Nodes</param>
        /// <returns>Der Kind-Node am Index <see cref="iIndex"/></returns>
        protected abstract TermNode GetChild(int iIndex);

        /// <summary>
        /// Legt den Kind-Node am Index <see cref="iIndex"/> fest.
        /// </summary>
        /// <param name="iIndex">Der Index des Kind-Nodes</param>
        /// <param name="node">Kind-Node</param>
        protected abstract void SetChild(int iIndex, TermNode node);

        /// <summary>
        /// Gibt die Anzahl aller Kinder zurück.
        /// </summary>
        /// <returns></returns>
        public int GetRecursiveChildCount()
        {
            int iCount = Count();
            int iRecursiveCount = iCount;
            for (int i = 0; i < iCount; i++)
            {
                iRecursiveCount += this[i].GetRecursiveChildCount();
            }

            return iRecursiveCount;
        }

        /// <summary>
        /// Ruft ein Kind an Index <see cref="iIndex"/> ab, oder legt das Kind an Index
        /// <see cref="iIndex"/> fest.
        /// </summary>
        /// <param name="iIndex">Der Index des Kind-Nodes</param>
        /// <returns>Kind-Node</returns>
        public TermNode this[int iIndex]
        {
            get
            {
                if (iIndex >= Count())
                {
                    throw new ArgumentOutOfRangeException();
                }
                return GetChild(iIndex);
            }
            set
            {
                if (iIndex >= Count())
                {
                    throw new ArgumentOutOfRangeException();
                }
                SetChild(iIndex, value);
            }
        }

        /// <summary>
        /// Gleichheitsoperator
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(TermNode left, TermNode right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Ungleichheitsoperator
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(TermNode left, TermNode right)
        {
            return !Equals(left, right);
        }
    }
}
