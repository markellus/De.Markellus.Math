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

namespace De.Markellus.Maths.Internals.TermParsing
{
    /// <summary>
    /// Definiert verschiedene Typen von Token, die in einem
    /// mathematischen Ausdruck vorkommen können.
    /// </summary>
    public enum TokenType
    {
        /// <summary>
        /// Der Typ ist (derzeit) unbekannt. Dieser Wert darf
        /// nach vollständiger Auswertung eines mathematischen
        /// Ausdruckes nicht mehr von einem Token genutzt werden.
        /// </summary>
        Unknown,

        /// <summary>
        /// Ein Wert aus dem Bereich der reellen Zahlen.
        /// </summary>
        Number,

        /// <summary>
        /// Eine Variable, die stellvertetend für eine
        /// beliebige reelle Zahl oder eine Funktion steht.
        /// </summary>
        Variable,

        /// <summary>
        /// Eine Konstante reelle Zahl, die durch ein
        /// gesondertes Zeichen oder Zeichenfolge dargestellt
        /// wird (z.B. PI).
        /// </summary>
        Constant,

        /// <summary>
        /// Eine mathematische Funktion mit einer variablen
        /// Anzahl an Parametern.
        /// </summary>
        Function,

        /// <summary>
        /// Eine Klammer.
        /// </summary>
        Parenthesis,

        /// <summary>
        /// Ein Zeichen oder eine Zeichenfolge, die eine
        /// Trennung von zwei anderen Token signalisiert.
        /// </summary>
        ArgumentSeparator,

        /// <summary>
        /// Ein Zeichen, welches Vor- und Nachkommastellen einer
        /// Dezimalzahl voneinander trennt.
        /// </summary>
        DecimalSeparator,

        /// <summary>
        /// Ein mathematischer Operator.
        /// </summary>
        Operator,

        /// <summary>
        /// Ein Zeichen oder eine Zeichenfolge, die keine nähere
        /// Bedeutung besitzen und lediglich der optischen Trennung
        /// zweier anderer Token dienen.
        /// </summary>
        WhiteSpace
    }
}
