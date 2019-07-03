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
