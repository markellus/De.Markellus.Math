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
using System.Globalization;

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
    }
}
