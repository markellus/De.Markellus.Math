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
