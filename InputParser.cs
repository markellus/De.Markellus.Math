using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

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
