/* Copyright (C) Marcel Bulla - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Marcel Bulla <postmaster@marcel-bulla.de>
 */

using System;

namespace De.Markellus.Maths.Statistics
{
    /// <summary>
    /// Bildet eine Fünf-Punkte-Zusammenfassung ab.
    /// </summary>
    public class FivePointSummary
    {
        /// <summary>
        /// Das 100er-Perzentil (Maximum)
        /// </summary>
        public double Percentile100 { get; }

        /// <summary>
        /// Das 75er-Perzentil
        /// </summary>
        public double Percentile75 { get; }

        /// <summary>
        /// Das 50er-Perzentil (Median)
        /// </summary>
        public double Percentile50 { get; }

        /// <summary>
        /// Das 25er-Perzentil
        /// </summary>
        public double Percentile25 { get; }

        /// <summary>
        /// Das 0er-Perzentil (Minimum)
        /// </summary>
        public double Percentile0 { get; }

        /// <summary>
        /// Erstellt eine neue Fünf-Punkte-Zusammenfassung.
        /// </summary>
        /// <param name="d100">Das 100er-Perzentil (Maximum)</param>
        /// <param name="d75">Das 75er-Perzentil</param>
        /// <param name="d50">Das 50er-Perzentil (Median)</param>
        /// <param name="d25">Das 25er-Perzentil</param>
        /// <param name="d0">Das 0er-Perzentil (Minimum)</param>
        private FivePointSummary(double d100, double d75, double d50, double d25, double d0)
        {
            Percentile100 = d100;
            Percentile75 = d75;
            Percentile50 = d50;
            Percentile25 = d25;
            Percentile0 = d0;
        }

        /// <summary>
        /// Ruft ein bestimmtes Perzentil der übergebenen Urliste ab.
        /// </summary>
        /// <param name="arrValues">Die Liste mit Zahlen, für die ein Perzentil ermittelt werden soll.</param>
        /// <param name="dPercentile">Das zu ermittelnde Perzentil</param>
        /// <returns>Das ermittelte Perzentil</returns>
        public static double GetPercentile(double[] arrValues, double dPercentile)
        {
            if (dPercentile < 0.0f || dPercentile > 1.0f)
            {
                throw new ArgumentOutOfRangeException($"{nameof(dPercentile)}: must be between 0.0 and 1.0");
            }

            double[] arrCopy = new double[arrValues.Length];
            Array.Copy(arrValues, arrCopy, arrValues.Length);
            arrValues = arrCopy;

            Array.Sort(arrValues);

            for (int i = 0; i < arrValues.Length; i++)
            {
                if (((double)(i + 1) / arrValues.Length) >= dPercentile)
                {
                    return arrValues[i];
                }
            }

            throw new SystemException();
        }


        /// <summary>
        /// Erstellt eine Fünf-Punkte-Zusammenfassung für die übergebene Liste an Zahlen.
        /// </summary>
        /// <param name="listValues">Eine Liste mit Zahlen</param>
        /// <returns>Die erstellte Fünf-Punkte-Zusammenfassung</returns>
        public static FivePointSummary Get(double[] arrValues)
        {
            return new FivePointSummary(
                GetPercentile(arrValues, 1.00),
                GetPercentile(arrValues, 0.75),
                GetPercentile(arrValues, 0.50),
                GetPercentile(arrValues, 0.25),
                GetPercentile(arrValues, 0.00));
        }

        /// <summary>
        /// Erstellt eine Fünf-Punkte-Zusammenfassung für die übergebene Liste an Zahlen.
        /// </summary>
        /// <param name="arrValues">Eine Liste mit String-Werten, die Zahlen abbilden.</param>
        /// <returns>Die erstellte Fünf-Punkte-Zusammenfassung</returns>
        public static FivePointSummary Get(string[] arrValues)
        {
            return Get(InputParser.ParseDoubleList(arrValues));
        }

        /// <summary>
        /// Erstellt eine Fünf-Punkte-Zusammenfassung für die übergebene Liste an Zahlen.
        /// </summary>
        /// <param name="strValues">Eine Liste mit String-Werten, die Zahlen abbilden.</param>
        /// <param name="cDivider">Das Zeichen, mit dem einzelne Werte voneinander getrennt sind.</param>
        /// <returns></returns>
        public static FivePointSummary Get(string strValues, char cDivider = ';')
        {
            return Get(InputParser.ParseDoubleList(strValues, cDivider));
        }
    }
}
