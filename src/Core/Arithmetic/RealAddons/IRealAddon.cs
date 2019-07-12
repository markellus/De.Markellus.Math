/* Copyright (C) Marcel Bulla - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Marcel Bulla <postmaster@marcel-bulla.de>
 */

namespace De.Markellus.Maths.Core.Arithmetic.RealAddons
{
    /// <summary>
    /// Interface zur Abbildung unterschiedlicher Typen von reellen Zahlen.
    /// Bestimmte Arten von reellen Zahlen, wie zum Beispiel periodische Zahlen, können nicht in
    /// dezimaler Darstellung abgebildet werden. Daher sind hierfür spezielle Implementierungen
    /// nötig.
    /// </summary>
    internal interface IRealAddon
    {
        /// <summary>
        /// Legt den Wert der reellen Zahl fest.
        /// </summary>
        /// <param name="strInput">Die reelle Zahl als String</param>
        /// <param name="real">Verweis auf das Objekt, welches die reelle Zahl abbildet</param>
        void SetValue(string strInput, Real real);

        /// <summary>
        /// Gibt die reelle Zahl als formatierten String zurück.
        /// </summary>
        /// <returns>Die reelle Zahl als formatierter String</returns>
        string GetValue();

        /// <summary>
        /// Gibt die reelle Zahl als Dezimalzahl zurück. Gegebenenfalls müssen hierbei
        /// Ungenauigkeiten hingenommen werden.
        /// </summary>
        /// <returns>Die reelle Zahl als String</returns>
        string GetPlainValue();

        /// <summary>
        /// Gibt einen String zurück der die Zahl repräsentiert und im Spigot-Backend verwendet werden kann.
        /// </summary>
        /// <returns>Die reelle Zahl in SPigot-Kompatibler Darstellung.</returns>
        string GetSpigotCompatibleValue();

        /// <summary>
        /// Überprüft, ob der übergebene String als reelle Zahl über diese Implementierung
        /// abgebildet werden kann. Das es sich um eine reele Zahl handelt ist bereits
        /// sichergestellt.
        /// </summary>
        /// <param name="strInput">Der zu überprüfende String</param>
        /// <returns>true, wenn der String mit dieser Implementierung dargestellt werden kann, ansonsten false.</returns>
        bool IsMatch(string strInput);
    }
}
