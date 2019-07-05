/* Copyright (C) Marcel Bulla - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Marcel Bulla <postmaster@marcel-bulla.de>
 */

using System;

namespace De.Markellus.Maths.Core.Arithmetic
{
    /// <summary>
    /// Ein sich wiederholendes Muster in einer reellen Zahl
    /// </summary>
    public struct Period
    {
        /// <summary>
        /// Das Muster
        /// </summary>
        public string Pattern;

        /// <summary>
        /// Offset, ab dem das Muster erkannt wurde
        /// </summary>
        public int Offset;
    }

    /// <summary>
    /// Erkennt periodische Zahlen.
    /// </summary>
    public static class PeriodDetection
    {

        /// <summary>
        /// https://de.wikipedia.org/wiki/Knuth-Morris-Pratt-Algorithmus
        /// </summary>
        /// <param name="strInput">Eingabe</param>
        /// <param name="iPatternLength">Suchen bis zu diesem Index</param>
        /// <param name="arrLongestProperSuffix">Ziel-Array</param>
        private static void compute(string strInput, int iPatternLength, ref int[] arrLongestProperSuffix)
        {
            int iLengthPrev = 0;
            int i = 1;
            arrLongestProperSuffix[0] = 0;

            while (i < iPatternLength)
            {
                if (strInput[i] == strInput[iLengthPrev])
                {
                    iLengthPrev++;
                    arrLongestProperSuffix[i] = iLengthPrev;
                    i++;
                }
                else
                {
                    if (iLengthPrev != 0)
                    {
                        iLengthPrev = arrLongestProperSuffix[iLengthPrev - 1];
                    }
                    else
                    {
                        arrLongestProperSuffix[i] = 0;
                        i++;
                    }
                }
            }
        }

        /// <summary>
        /// Sucht nach einer Periodischen Folge im übergebenen String.
        /// </summary>
        /// <param name="str">Der zu durchsuchende String</param>
        /// <param name="iTrimStart">Start-Offset</param>
        /// <param name="iTrimEnd">End-Offset</param>
        /// <returns>Wenn eine Folge gefunden wurde wird diese als String zurückgegeben, ansonsten null.</returns>
        private static string DetectPeriod(string str, int iTrimStart, int iTrimEnd)
        {
            str = str.Substring(iTrimStart);

            int iSub = str.Length - iTrimEnd;
            int[] arrLongestProperSuffix = new int[iSub];

            compute(str, iSub, ref arrLongestProperSuffix);

            int iMax = arrLongestProperSuffix[iSub - 1];

            if (iMax > 0 && iSub % (iSub - iMax) == 0)
            {
                int i = Array.LastIndexOf(arrLongestProperSuffix, 0) + 1;
                return str.Substring(0, i);
            }

            return null;
        }

        /// <summary>
        /// Sucht nach einer Periodischen Folge im übergebenen String.
        /// </summary>
        /// <param name="str">Der zu durchsuchende String</param>
        /// <returns>Ein struct mit dem Suchergebnis</returns>
        public static Period DetectPeriod(this string str)
        {
            for (int end = 0; end < str.Length / 2; end++)
            {
                for (int start = 0; start < str.Length / 2; start++)
                {
                    string strPeriod = DetectPeriod(str, start, end);
                    if (strPeriod != null && strPeriod.Length > end)
                    {
                        return new Period {Pattern = strPeriod, Offset = start};
                    }
                }
            }

            return new Period {Pattern = null};
        }
    }
}
