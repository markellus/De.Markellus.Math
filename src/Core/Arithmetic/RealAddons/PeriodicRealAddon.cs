/* Copyright (C) Marcel Bulla - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Marcel Bulla <postmaster@marcel-bulla.de>
 */

using System;
using System.Text;

namespace De.Markellus.Maths.Core.Arithmetic.RealAddons
{
    /// <summary>
    /// Bildet eine periodische reelle Zahl ab.
    /// </summary>
    internal class PeriodicRealAddon : IRealAddon
    {
        /// <summary>
        /// Die interne String-Darstellung der gespeicherten reellen Zahl.
        /// </summary>
        private string _strValue;

        /// <summary>
        /// Länge der Periode
        /// </summary>
        private int _iPeriodSize;

        /// <summary>
        /// Legt den Wert der reellen Zahl fest.
        /// </summary>
        /// <param name="strInput">Die reelle Zahl als String</param>
        /// <param name="real">Verweis auf das Objekt, welches die reelle Zahl abbildet</param>
        public void SetValue(string strInput, Real real)
        {
            _strValue = GetPeriodicValue(strInput);

            if (_strValue == null)
            {
                throw new ArithmeticException($"This number is not periodic or is not correctly formatted: {strInput}");
            }


            //Wenn bei der Verarbeitung die Länge der Periode auf 0 gesunken ist, kann
            //möglicherweise eine andereImplementierung gewählt werden
            if (_iPeriodSize == 0)
            {
                real.ReconsiderAddon(_strValue);
            }
        }

        /// <summary>
        /// Gibt die reelle Zahl als formatierten String zurück.
        /// </summary>
        /// <returns>Die reelle Zahl als formatierter String</returns>
        public string GetValue()
        {
            return _strValue + "p" + _iPeriodSize;
        }

        /// <summary>
        /// Gibt die reelle Zahl als Dezimalzahl zurück. Gegebenenfalls müssen hierbei
        /// Ungenauigkeiten hingenommen werden.
        /// </summary>
        /// <returns>Die reelle Zahl als String</returns>
        public string GetPlainValue()
        {
            string[] strReal = _strValue.Split('.');

            StringBuilder builder = new StringBuilder(RealFactory.ROUND_PRECISION);
            builder.Append(strReal[1].Substring(0, strReal[1].Length - _iPeriodSize));
            string strPeriod = strReal[1].Substring(strReal[1].Length - _iPeriodSize);

            while (builder.Length < RealFactory.ROUND_PRECISION)
            {
                builder.Append(strPeriod);
            }

            if (builder.Length > RealFactory.ROUND_PRECISION)
            {
                builder.Remove(RealFactory.ROUND_PRECISION, builder.Length - RealFactory.ROUND_PRECISION);
            }

            strReal[1] = builder.ToString();

            return strReal[0] + "." + strReal[1];
        }

        /// <summary>
        /// Überprüft, ob der übergebene String als reelle Zahl über diese Implementierung
        /// abgebildet werden kann. Das es sich um eine reele Zahl handelt ist bereits
        /// sichergestellt.
        /// </summary>
        /// <param name="strInput">Der zu überprüfende String</param>
        /// <returns>true, wenn der String mit dieser Implementierung dargestellt werden kann, ansonsten false.</returns>
        public bool IsMatch(string strInput)
        {
            return GetPeriodicValue(strInput) != null;
        }

        /// <summary>
        /// Berechnet die interne Darstellung der übergebenen Zahl.
        /// </summary>
        /// <param name="strInput">Die reelle Zahl als String</param>
        /// <returns>Die angepasste Darstellung oder null, falls dieser String keine periodische Zahl abbildet</returns>
        private string GetPeriodicValue(string strInput)
        {
            string[] strParts = strInput.Split('.');

            //Zahl ist bereits in Perioden-Darstellung
            if (strInput.IndexOf('p') != -1)
            {
                string[] strPeriod = strParts[1].Split('p');

                //Periodenlänge auslesen
                if (!int.TryParse(strPeriod[1], out _iPeriodSize))
                {
                    //Fehlerhafte Angabe der Periodenlänge
                    return null;
                }

                strParts[1] = strPeriod[0];
            }
            //Zahl ist in Dezimaldarstellung
            else
            {
                //Periode ermitteln
                Period period = strParts[1].DetectPeriod();

                if (strParts[1].Length >= RealFactory.ROUND_PRECISION && period.Pattern != null)
                {
                    if (period.Pattern == "9")
                    {
                        //Zahl ist keine Periode, es handelt sich um einen Rundungsfehler
                        // -> Rundungsfehler korrigieren
                        strParts[0] = SpigotApi.Add(strParts[0], "1");
                        strParts[1] = "0";
                    }
                    else if (period.Pattern == "0")
                    {
                        //Periode besteht aus unendlich vielen Nullen
                        // -> Periode kann abgeschnitten werden
                        strParts[1] = "0";
                    }
                    else
                    {
                        //Zahl ist periodisch, string formatieren und Periodenlänge speichern
                        strParts[1] = strParts[1].Substring(0, period.Offset) + period.Pattern;
                        _iPeriodSize = period.Pattern.Length;
                    }
                }
                else
                {
                    return null;
                }
            }

            return strParts[0] + "." + strParts[1];
        }
    }
}
