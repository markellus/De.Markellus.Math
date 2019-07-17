/* Copyright (C) Marcel Bulla - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Marcel Bulla <postmaster@marcel-bulla.de>
 */

using System.Linq;

namespace De.Markellus.Maths.Statistics
{
    public static class ModalMean
    {
        /// <summary>
        /// Berechnet den Modalwert der Werte in der übergebenen Liste.
        /// </summary>
        /// <param name="arrValues">Eine Liste mit Werten, dessen Modal berechnet werden soll.</param>
        /// <returns>Der Modalwert der Werte in der übergebenen Liste</returns>
        public static double[] Calculate(double[] arrValues)
        {
            ILookup<double, double> lk = arrValues.ToLookup(x => x);
            if (lk.Count == 0)
                return new double[0];
            int maxCount = lk.Max(x => x.Count());
            return lk.Where(x => x.Count() == maxCount).Select(x => x.Key).ToArray();
        }

        /// <summary>
        /// Berechnet den Modalwert der Werte in der übergebenen Liste.
        /// </summary>
        /// <param name="arrValues">Eine Liste mit Werten, dessen Modal berechnet werden soll.</param>
        /// <returns>Der Modalwert der Werte in der übergebenen Liste</returns>
        public static float[] Calculate(float[] arrValues)
        {
            ILookup<float, float> lk = arrValues.ToLookup(x => x);
            if (lk.Count == 0)
                return new float[0];
            int maxCount = lk.Max(x => x.Count());
            return lk.Where(x => x.Count() == maxCount).Select(x => x.Key).ToArray();
        }

        /// <summary>
        /// Berechnet den Modalwert der Werte in der übergebenen Liste.
        /// </summary>
        /// <param name="arrValues">Eine Liste mit Werten, dessen Modal berechnet werden soll.</param>
        /// <returns>Der Modalwert der Werte in der übergebenen Liste</returns>
        public static double[] Calculate(string[] arrValues)
        {
            return Calculate(InputParser.ParseDoubleList(arrValues));
        }

        /// <summary>
        /// Berechnet den Modalwert der Werte in der übergebenen Liste.
        /// </summary>
        /// <param name="strValues">Eine Liste mit Werten, dessen Modal berechnet werden soll.</param>
        /// <param name="cDivider">Das Zeichen, mit dem einzelne Werte voneinander getrennt sind.</param>
        /// <returns>Der Modalwert der Werte in der übergebenen Liste</returns>
        public static double[] Calculate(string strValues, char cDivider = ';')
        {
            return Calculate(InputParser.ParseDoubleList(strValues, cDivider));
        }
    }
}
