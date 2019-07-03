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
    /// Bildet einen Teil eines mathematischen Ausdrucks oder Terms ab.
    /// </summary>
    public class Token
    {
        /// <summary>
        /// Der Typ des Tokens
        /// </summary>
        public TokenType Type { get; }

        /// <summary>
        /// Der Token an sich, in String-Darstellung.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Die Assoziativät des Tokens
        /// </summary>
        public TokenAssociativity Associativity { get; }

        /// <summary>
        /// Die Gewichtung des Tokens
        /// </summary>
        public TokenPrecedence Precedence { get; }

        /// <summary>
        /// Erstellt einen leeren Token.
        /// </summary>
        /// <returns></returns>
        public static Token CreateDefault()
        {
            return new Token(TokenType.Unknown, "", TokenAssociativity.NoneAssociative, TokenPrecedence.Undefined);
        }

        /// <summary>
        /// Erstellt einen neuen Token mit den angegebenen Eigenschaften
        /// </summary>
        /// <param name="type">Der Typ des Tokens</param>
        /// <param name="value">Der Token an sich, in String-Darstellung</param>
        /// <param name="associativity">Die Assoziativät des Tokens</param>
        /// <param name="precedence">Die Gewichtung des Tokens</param>
        public Token(TokenType type,
            string value,
            TokenAssociativity associativity = TokenAssociativity.NoneAssociative,
            TokenPrecedence precedence = TokenPrecedence.Undefined)
        {
            Type = type;
            Associativity = associativity;
            Precedence = precedence;
            Value = value;
        }

        /// <summary>
        /// Erstellt eine Kopie des Tokens mit denselben Eigenschaften.
        /// </summary>
        /// <returns></returns>
        public Token CreateCopy()
        {
            return new Token(Type, Value, Associativity, Precedence);
        }

        /// <summary>
        /// Vergleichsoperator
        /// </summary>
        /// <param name="obj">Das Objekt, mit dem verglichen wird.</param>
        /// <returns>true, wenn beide Objekte gleich sind, ansonsten false.</returns>
        public override bool Equals(object obj)
        {
            return obj is Token other && Type == other.Type && string.Equals(Value, other.Value);
        }

        /// <summary>
        /// Vergleichsoperator
        /// </summary>
        /// <param name="other">Das Objekt, mit dem verglichen wird.</param>
        /// <returns>true, wenn beide Objekte gleich sind, ansonsten false.</returns>
        protected bool Equals(Token other)
        {
            return Type == other.Type && Associativity == other.Associativity && Precedence == other.Precedence &&
                   string.Equals(Value, other.Value);
        }

        /// <summary>
        /// Gibt eine String-Darstellung des Objektes zurück.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Type}: {Value}";
        }

        /// <summary>
        /// Erstellt einen Hashwert des Objektes.
        /// </summary>
        /// <returns>Der Hashwert des Objektes.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (int) Type;
                hashCode = (hashCode * 397) ^ (int) Associativity;
                hashCode = (hashCode * 397) ^ (int) Precedence;
                hashCode = (hashCode * 397) ^ (Value != null ? Value.GetHashCode() : 0);
                return hashCode;
            }
        }

        /// <summary>
        /// Vergleichsoperator
        /// </summary>
        /// <param name="left">Linkes Vergleichsobjekt</param>
        /// <param name="right">Rechtes Vergleichsobjekt</param>
        /// <returns>true, wenn beide Objekte gleich sind, ansonsten false.</returns>
        public static bool operator ==(Token left, Token right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Vergleichsoperator
        /// </summary>
        /// <param name="left">Linkes Vergleichsobjekt</param>
        /// <param name="right">Rechtes Vergleichsobjekt</param>
        /// <returns>true, wenn die Objekte nicht gleich sind, ansonsten false.</returns>
        public static bool operator !=(Token left, Token right)
        {
            return !Equals(left, right);
        }
    }
}