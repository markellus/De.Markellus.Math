/* Copyright (C) Marcel Bulla - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Marcel Bulla <postmaster@marcel-bulla.de>
 */

namespace De.Markellus.Maths.Core.TermEngine.TermParsing
{
    /// <summary>
    /// Beschreibt die Assoziativität eines Tokens.
    /// </summary>
    public enum TokenAssociativity
    {
        /// <summary>
        /// Der Token folgt dem Assoziativgesetz oder diese Eigenschaft
        /// ist für diesen Typ nicht definiert.
        /// </summary>
        NoneAssociative,

        /// <summary>
        /// Der Token ist links-assoziativ.
        /// </summary>
        LeftAssociative,

        /// <summary>
        /// Der Token ist rechts-assoziativ.
        /// </summary>
        RightAssociative,
    }
}
