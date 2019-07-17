/* Copyright (C) Marcel Bulla - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Marcel Bulla <postmaster@marcel-bulla.de>
 */

using System;

namespace De.Markellus.Maths.Statistics
{
    public static class GeometricMean
    {
        /// <summary>
        /// Berechnet das geometrische Mittel der Werte in der übergebenen Liste.
        /// </summary>
        /// <param name="arrValues">Eine Liste mit Werten, dessen geometrisches Mittel berechnet werden soll.</param>
        /// <param name="bIgnoreZeros">True, wenn Zahlen mit dem Wert 0 ignoriert werden sollen.
        /// Kommen solche Zahlen vor und dieser Parameter wird nicht auf true gesetzt, ist das Gesamtergebnis ebenfalls 0.</param>
        /// <returns>Das geometrische Mittel der Werte in der übergebenen Liste</returns>
        public static double Calculate(double[] arrValues, bool bIgnoreZeros = false)
        {
            double sum = 0;

            foreach (double t in arrValues)
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (!bIgnoreZeros || t != 0)
                {
                    sum += Math.Log(t);
                }
            }

            sum /= arrValues.Length;

            return Math.Exp(sum);
        }

        /// <summary>
        /// Berechnet das geometrische Mittel der Werte in der übergebenen Liste.
        /// </summary>
        /// <param name="arrValues">Eine Liste mit Werten, dessen geometrisches Mittel berechnet werden soll.</param>
        /// <param name="bIgnoreZeros">True, wenn Zahlen mit dem Wert 0 ignoriert werden sollen.
        /// Kommen solche Zahlen vor und dieser Parameter wird nicht auf true gesetzt, ist das Gesamtergebnis ebenfalls 0.</param>
        [Obsolete("Do not use float values for this operation, they are not precise enough.", true)]
        public static float Calculate(float[] arrValues, bool bIgnoreZeros = false)
        {
            throw new Exception();
        }

        /// <summary>
        /// Berechnet das geometrische Mittel der Werte in der übergebenen Liste.
        /// </summary>
        /// <param name="arrValues">Eine Liste mit Werten, dessen geometrisches Mittel berechnet werden soll.</param>
        /// <param name="bIgnoreZeros">True, wenn Zahlen mit dem Wert 0 ignoriert werden sollen.
        /// Kommen solche Zahlen vor und dieser Parameter wird nicht auf true gesetzt, ist das Gesamtergebnis ebenfalls 0.</param>
        /// <returns>Das geometrische Mittel der Werte in der übergebenen Liste</returns>
        public static double Calculate(string[] arrValues, bool bIgnoreZeros = false)
        {
            return Calculate(InputParser.ParseDoubleList(arrValues), bIgnoreZeros);
        }

        /// <summary>
        /// Berechnet das geometrische Mittel der Werte in der übergebenen Liste.
        /// </summary>
        /// <param name="strValues">Eine Liste mit Werten, dessen geometrisches Mittel berechnet werden soll.</param>
        /// <param name="cDivider">Das Zeichen, mit dem einzelne Werte voneinander getrennt sind.</param>
        /// <param name="bIgnoreZeros">True, wenn Zahlen mit dem Wert 0 ignoriert werden sollen.
        /// Kommen solche Zahlen vor und dieser Parameter wird nicht auf true gesetzt, ist das Gesamtergebnis ebenfalls 0.</param>
        /// <returns>Das geometrische Mittel der Werte in der übergebenen Liste</param>
        /// <returns>Das geometrische Mittel der Werte in der übergebenen Liste</returns>
        public static double Calculate(string strValues, char cDivider = ';', bool bIgnoreZeros = false)
        {
            return Calculate(InputParser.ParseDoubleList(strValues, cDivider), bIgnoreZeros);
        }
    }
}
