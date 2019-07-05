/* Copyright (C) Marcel Bulla - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Marcel Bulla <postmaster@marcel-bulla.de>
 */

namespace De.Markellus.Maths.Core.Arithmetic.RealAddons
{
    /// <summary>
    /// Bildet eine reelle Zahl in Dezimaldarstellung mit theoretisch unendlich vielen
    /// Nachkommastellen ab.
    /// Standard-Implementierung von <see cref="IRealAddon"/>
    /// </summary>
    internal class StringRealAddon : IRealAddon
    {
        /// <summary>
        /// Die interne String-Darstellung der gespeicherten reellen Zahl.
        /// </summary>
        private string _strValue;

        /// <summary>
        /// Legt den Wert der reellen Zahl fest.
        /// </summary>
        /// <param name="strInput">Die reelle Zahl als String</param>
        /// <param name="real">Verweis auf das Objekt, welches die reelle Zahl abbildet</param>
        public void SetValue(string strInput, Real real)
        {
            _strValue = strInput;
        }

        /// <summary>
        /// Gibt die reelle Zahl als formatierten String zurück.
        /// </summary>
        /// <returns>Die reelle Zahl als formatierter String</returns>
        public string GetValue()
        {
            return _strValue;
        }

        /// <summary>
        /// Gibt die reelle Zahl als Dezimalzahl zurück. Gegebenenfalls müssen hierbei
        /// Ungenauigkeiten hingenommen werden.
        /// </summary>
        /// <returns>Die reelle Zahl als String</returns>
        public string GetPlainValue()
        {
            return _strValue;
        }

        /// <summary>
        /// Überprüft, ob der übergebene String als reelle Zahl über diese Implementierung
        /// abgebildet werden kann. Das es sich um eine reele Zahl handelt ist bereits
        /// sichergestellt.
        /// </summary>
        /// <param name="strInput">Der zu überprüfende String</param>
        /// <returns>Immer true, da mit dieser Darstellungsform alle reellen Zahlen abgebildet
        /// werden können, solange genügend Speicher vorhanden ist.</returns>
        public bool IsMatch(string strInput)
        {
            return true;
        }
    }
}
