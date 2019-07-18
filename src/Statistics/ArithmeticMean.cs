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

using System.Linq;

namespace De.Markellus.Maths.Statistics
{
    public static class ArithmeticMean
    {
        /// <summary>
        /// Berechnet das arithmetische Mittel der Werte in der übergebenen Liste.
        /// </summary>
        /// <param name="listValues">Eine Liste mit Werten, dessen arithmetisches Mittel berechnet werden soll.</param>
        /// <returns>Das arithmetische Mittel der übergebenen Werte.</returns>
        public static double Calculate(double[] listValues)
        {
            return listValues.Sum() / listValues.Count();
        }

        /// <summary>
        /// Berechnet das arithmetische Mittel der Werte in der übergebenen Liste.
        /// </summary>
        /// <param name="listValues">Eine Liste mit Werten, dessen arithmetisches Mittel berechnet werden soll.</param>
        /// <returns>Das arithmetische Mittel der übergebenen Werte.</returns>
        public static float Calculate(float[] listValues)
        {
            return listValues.Sum() / listValues.Count();
        }

        /// <summary>
        /// Berechnet das arithmetische Mittel der Werte in der String-Liste. Die Strings werden als Gleitkommazahlen interpretiert.
        /// </summary>
        /// <param name="arrValues">Eine Liste mit String-Werten, dessen arithmetisches Mittel berechnet werden soll.</param>
        /// <returns>Das arithmetische Mittel der übergebenen Werte.</returns>
        public static double Calculate(string[] arrValues)
        {
            return Calculate(InputParser.ParseDoubleList(arrValues));
        }

        /// <summary>
        /// Berechnet das arithmetische Mittel der Werte in der String-Liste. Die Strings werden als Gleitkommazahlen interpretiert.
        /// </summary>
        /// <param name="strValues">Eine Liste mit String-Werten, dessen arithmetisches Mittel berechnet werden soll.</param>
        /// <param name="cDivider">Das Zeichen, mit dem einzelne Werte voneinander getrennt sind.</param>
        /// <returns>Das arithmetische Mittel der übergebenen Werte.</returns>
        public static double Calculate(string strValues, char cDivider = ';')
        {
            return Calculate(InputParser.ParseDoubleList(strValues, cDivider));
        }
    }
}
