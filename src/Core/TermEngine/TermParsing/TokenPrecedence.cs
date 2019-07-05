/* Copyright (C) Marcel Bulla - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Marcel Bulla <postmaster@marcel-bulla.de>
 */

namespace De.Markellus.Maths.Core.TermEngine.TermParsing
{
    /// <summary>
    /// Legt die Gewichtung des Tokens bei der Auswertung fest.
    /// Es werden die allgemeinen Regeln der Infix-Notation befolgt.
    /// </summary>
    public enum TokenPrecedence
    {
        /// <summary>
        /// Die Gewichtung ist nicht definiert.
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// Die Gewichtung eines Gleichheits-Operators.
        /// </summary>
        Equality = 1,

        /// <summary>
        /// Die Gewichtung eines Additions-Operators.
        /// </summary>
        Addition = 100,

        /// <summary>
        /// Die Gewichtung eines Subtraktions-Operators.
        /// </summary>
        Subtraction = 100,

        /// <summary>
        /// Die Gewichtung eines Multiplikations-Operators.
        /// </summary>
        Multiplication = 200,

        /// <summary>
        /// Die Gewichtung eines Divisions-Operators.
        /// </summary>
        Division = 200,

        /// <summary>
        /// Die Gewichtung eines Exponential-Operators.
        /// </summary>
        Exponentiation = 300,

        /// <summary>
        /// TODO: Not implemented
        /// </summary>
        RootExtraction = 300,
    }
}
