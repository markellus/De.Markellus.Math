/* Copyright (C) Marcel Bulla - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Marcel Bulla <postmaster@marcel-bulla.de>
 */

namespace De.Markellus.Maths.Core.TermEngine.TermParsing
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
