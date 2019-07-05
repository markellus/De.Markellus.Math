/* Copyright (C) Marcel Bulla - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Marcel Bulla <postmaster@marcel-bulla.de>
 */

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using De.Markellus.Maths.Core.Arithmetic.RealAddons;

namespace De.Markellus.Maths.Core.Arithmetic
{
    /// <summary>
    /// In dieser Fabrik werden neue Instanzen von <see cref="Real"/> erstellt.
    /// </summary>
    public static class RealFactory
    {
        /// <summary>
        /// Die maximale Genauigkeit reeller Zahlen. Aus Platzgründen können Zahlen nicht unendlich
        /// viele Nachkommastellen besitzen.
        /// </summary>
        public const int ROUND_PRECISION = 500;

        /// <summary>
        /// Regulärer Ausdruck, der den Aufbau einer reellen Zahl als String beschreibt.
        ///
        /// Eine reelle Zahl besteht aus folgenden Teilen:
        ///  1. (Optional) Ein einzelnes Zeichen   - Vorzeichen
        ///  1.1a Das Zeichen '+'                  - Positive Zahl
        ///  1.1b Das Zeichen '-''                 - Negative Zahl
        ///  2. Eine Reihe von Zeichen von 0 bis 9 - Vorkommastellen
        ///  3. Ein Punkt                          - Trennzeichen zwischen Vor- und Nachkommastellen
        ///  4. Eine Reihe von Zeichen von 0 bis 9 - Nachkommastellen
        ///  5. (Optional) Der Buchstabe 'p'       - Kennzeichnung als periodische Zahl
        ///  6.1. (Optional) UInt32                - Länge der Periode
        /// </summary>
        private static Regex _regexDecimal;

        /// <summary>
        /// Regulärer Ausdruck, der den Aufbau einer ganzen Zahl als String beschreibt.
        ///
        /// Eine ganze Zahl besteht aus folgenden Teilen:
        ///  1. (Optional) Ein einzelnes Zeichen   - Vorzeichen
        ///  1.1a Das Zeichen '+'                  - Positive Zahl
        ///  1.1b Das Zeichen '-''                 - Negative Zahl
        ///  2. Eine Reihe von Zeichen von 0 bis 9 - Vorkommastellen
        /// </summary>
        private static Regex _regexNumber;

        /// <summary>
        /// Liste mit allen zur Verfügung stehenden Implementierungen einer reellen Zahl
        /// </summary>
        private static List<IRealAddon> _listAddons;

        /// <summary>
        /// Fabrik-Initialisierung
        /// </summary>
        static RealFactory()
        {
            _regexDecimal = new Regex("^[\\+\\-]?[0-9]+\\.[0-9]+[p]{0,1}[0-9]*$", RegexOptions.Singleline);
            _regexNumber = new Regex("^[\\+\\-]?[0-9]+$", RegexOptions.Singleline);

            _listAddons = new List<IRealAddon>();

            RegisterAddon(new PeriodicRealAddon());
            RegisterAddon(new StringRealAddon());
        }

        /// <summary>
        /// Erstellt eine neue reelle Zahl anhand ihrer Stringdarstellung.
        /// </summary>
        /// <param name="strInput">Die reelle Zahl als String</param>
        /// <returns>Eine neue reelle Zahl</returns>
        public static Real GenerateReal(string strInput)
        {
            if (!ValidateInput(ref strInput))
            {
                throw new ArithmeticException($"The given string {strInput} can not be interpreted as a real number.");
            }

            IRealAddon addon = GetMatchingAddon(strInput);

            if (addon == null)
            {
                throw new ArgumentException($"Unable to create a real number from this input: {strInput}", nameof(strInput));
            }

            Real real = new Real(addon);
            addon.SetValue(strInput, real);

            return real;
        }

        /// <summary>
        /// Sucht eine passende implementierung für die übergebene reelle Zahl aus.
        /// </summary>
        /// <param name="strInput">Die reelle Zahl als String</param>
        /// <returns>Eine entsprechende Implementeirung oder null, falls keine Implementierung gefunden wurde</returns>
        internal static IRealAddon GetMatchingAddon(string strInput)
        {
            foreach (IRealAddon addon in _listAddons)
            {
                if (addon.IsMatch(strInput))
                {
                    return (IRealAddon)Activator.CreateInstance(addon.GetType());
                }
            }

            return null;
        }

        /// <summary>
        /// Fügt eine neue Implementierung hinzu.
        /// </summary>
        /// <param name="addon">Die neue Implementierung</param>
        private static void RegisterAddon(IRealAddon addon)
        {
            _listAddons.Add(addon);
        }

        /// <summary>
        /// Überprüft, ob es sich bei dem übergebenen String um die Abbildung einer reellen Zahl
        /// handelt, und passt diesen gegebenenfalls noch an.
        /// </summary>
        /// <param name="strInput">Die reelle Zahl als String</param>
        /// <returns>true, wenn es sich um eine reelle Zahl handelt, ansonsten false.</returns>
        private static bool ValidateInput(ref string strInput)
        {
            if (_regexNumber.IsMatch(strInput))
            {
                strInput += ".0";
            }
            else
            {
                strInput = strInput.Replace(',', '.');
            }

            return _regexDecimal.IsMatch(strInput);
        }
    }
}
