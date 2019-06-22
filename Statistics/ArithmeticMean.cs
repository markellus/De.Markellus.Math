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
