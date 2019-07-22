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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using De.Markellus.Maths.Internals.TermParsing;

namespace De.Markellus.Maths
{
    public static class InputParser
    {
        /// <summary>
        /// Parst eine Liste an Zahlen, dargestellt durch Zeichen, in eine Liste mit Gleitkommawerten.
        /// </summary>
        /// <param name="arrValues">Die zu parsende Liste mit Zahlen</param>
        /// <returns>Eine Liste mit Gleitkommawerten.</returns>
        public static double[] ParseDoubleList(string[] arrValues)
        {
            double[] arrParsed = new double[arrValues.Length];

            for (int i = 0; i < arrValues.Length; i++)
            {
                if (double.TryParse(arrValues[i].Replace(',', '.'), NumberStyles.Float,
                    CultureInfo.CreateSpecificCulture("en-US"), out double dInput))
                {
                    arrParsed[i] = dInput;
                }
                else
                {
                    throw new FormatException($"Invalid input value: {arrParsed}, array index: {i}");
                }
            }

            return arrParsed;
        }

        /// <summary>
        /// Parst eine Liste an Zahlen, dargestellt durch Zeichen, in eine Liste mit Gleitkommawerten.
        /// </summary>
        /// <param name="strValues">Die zu parsende Liste mit Zahlen</param>
        /// <param name="cDivider">Das Zeichen, mit dem einzelne Werte voneinander getrennt sind.</param>
        /// <returns>Eine Liste mit Gleitkommawerten.</returns>
        public static double[] ParseDoubleList(string strValues, char cDivider)
        {
            return ParseDoubleList(strValues.Split(new char[] { cDivider }, StringSplitOptions.RemoveEmptyEntries));
        }

        /// <summary>
        /// Parst einen mathematischen Term und gibt diesen aufgeteilt in einzelne Bauteile zurück.
        /// </summary>
        /// <param name="strTerm">Der mathematische Term als String, z.B. "5 + 5 - sqrt(9)"</param>
        /// <param name="arrVariables">Eine Liste mit Variablen, die in dem Term vortkommen, beispielsweise "x", "y"</param>
        /// <returns></returns>
        public static Token[] TokenizeTerm(string strTerm, params string[] arrVariables)
        {
            return MathExpressionTokenizer.Tokenize(strTerm, true, arrVariables).ToArray();
        }

        /// <summary>
        /// Parst einen mathematischen Term und gibt diesen aufgeteilt in einzelne Bauteile zurück.
        /// </summary>
        /// <param name="reader">Ein StringReader-Objekt, von dem der zu parsende Term ausgelesen wird.</param>
        /// <param name="arrVariables">Eine Liste mit Variablen, die in dem Term vortkommen, beispielsweise "x", "y"</param>
        /// <returns></returns>
        public static Token[] TokenizeTerm(StringReader reader, params string[] arrVariables)
        {
            return MathExpressionTokenizer.Tokenize(reader, true, arrVariables).ToArray();
        }

        /// <summary>
        /// Parst einen mathematischen Term und gibt diesen aufgeteilt in einzelne Bauteile und in umgedrehter polnischer Notation zurück.
        /// </summary>
        /// <param name="strTerm">Der mathematische Term als String, z.B. "5 + 5 - sqrt(9)"</param>
        /// <param name="arrVariables">Eine Liste mit Variablen, die in dem Term vortkommen, beispielsweise "x", "y"</param>
        /// <returns></returns>
        public static Token[] TokenizeTermToRpn(string strTerm, params string[] arrVariables)
        {
            return ShuntingYardParser.InfixToRpn(strTerm, arrVariables).ToArray();
        }

        /// <summary>
        /// Parst einen mathematischen Term und gibt diesen aufgeteilt in einzelne Bauteile und in umgedrehter polnischer Notation zurück.
        /// </summary>
        /// <param name="reader">Ein StringReader-Objekt, von dem der zu parsende Term ausgelesen wird.</param>
        /// <param name="arrVariables">Eine Liste mit Variablen, die in dem Term vortkommen, beispielsweise "x", "y"</param>
        /// <returns></returns>
        public static Token[] TokenizeTermToRpn(StringReader reader, params string[] arrVariables)
        {
            return ShuntingYardParser.InfixToRpn(reader, arrVariables).ToArray();
        }
    }
}
