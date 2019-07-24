/* 
 * This file is part of De.Markellus.Math (https://github.com/markellus/De.Markellus.Math).
 * Copyright (c) 2019 Marcel Bulla.
 * 
 * This program is free software: you can redistribute it and/or modify  
 * it under the terms of the GNU General Public License as published by  
 * the Free Software Foundation, version 3.
 *
 * This program is distributed in the hope that it will be useful, but 
 * WITHOUT ANY WARRANTY; without even the implied warranty of 
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU 
 * General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License 
 * along with this program. If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace De.Markellus.Maths.Arithmetic
{
    /// <summary>
    /// Diese Klasse bildet eine Zahl aus der Menge der reellen Zahlen ab.
    /// Die Menge der reellen Zahlen beinhaltet natürliche Zahlen, ganze Zahlen,
    /// irrationale Zahlen sowie rationale Zahlen.
    /// </summary>
    public class Real
    {
        /// <summary>
        /// Die standardmässig eingestellte maximale Genauigkeit reeller Zahlen.
        /// Aus Platzgründen muss die Anzahl der Nachkommastellen begrenzt werden.
        /// Zahlen mit mehr Nachkommastellen als hier angegeben werden in der
        /// letzten Stelle gerundet.
        /// </summary>
        public const int ROUND_PRECISION_DEFAULT = 500;

        private static int _rp;

        /// <summary>
        /// Die maximale Genauigkeit reeller Zahlen. Aus Platzgründen muss die Anzahl der
        /// Nachkommastellen begrenzt werden. Zahlen mit mehr Nachkommastellen als hier
        /// angegeben werden in der letzten Stelle gerundet.
        /// </summary>
        public static int ROUND_PRECISION {
            get => _rp;
            set
            {
                _rp = value;
                EQUALITY_PRECISION = "0." + new string('0', _rp);
            } }

        private static string EQUALITY_PRECISION;

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
        ///  2. Eine Reihe von Zeichen von 0 bis 9 - Die ganze Zahl
        /// </summary>
        private static Regex _regexNumber;

        /// <summary>
        /// Regulärer Ausdruck, der den Aufbau einer rationalen Zahl als String beschreibt.
        ///
        /// Eine rationale Zahl besteht aus folgenden Teilen
        ///  1. (Optional) Ein einzelnes Zeichen   - Vorzeichen
        ///  1.1a Das Zeichen '+'                  - Positive Zahl
        ///  1.1b Das Zeichen '-''                 - Negative Zahl
        ///  2. Eine Reihe von Zeichen von 0 bis 9 - Der Zähler
        ///  3. Das Zeichen '/'                    - Trennzeichen zwischen Zähler und Nenner
        ///  4. Eine Reihe von Zeichen von 0 bis 9 - Der Nenner
        /// </summary>
        private static Regex _regexFraction;

        /// <summary>
        /// Statische Einstellungen
        /// </summary>
        static Real()
        {
            ROUND_PRECISION = ROUND_PRECISION_DEFAULT;

            _regexDecimal = new Regex("^[\\+\\-]?[0-9]+\\.[0-9]+[p]{0,1}[0-9]*$", RegexOptions.Singleline | RegexOptions.Compiled);
            _regexNumber = new Regex("^[\\+\\-]?[0-9]+$", RegexOptions.Singleline | RegexOptions.Compiled);
            _regexFraction = new Regex("^[\\+\\-]?[0-9]+\\/[0-9]+$", RegexOptions.Singleline | RegexOptions.Compiled);
        }

        /// <summary>
        /// Der Wert der reellen Zahl.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Erstellt eine neue Instanz einer reellen Zahl.
        /// </summary>
        /// <param name="strValue">Die reelle Zahl als String</param>
        public Real(string strValue)
        {
            //Überprüfen, ob es sich bei dem übergebenen String um die Abbildung einer reellen Zahl
            //handelt, und diesen gegebenenfalls noch anpassen

            strValue = strValue.Replace(',', '.');

            if (_regexNumber.IsMatch(strValue) || _regexFraction.IsMatch(strValue) || _regexDecimal.IsMatch(strValue))
            {
                Value = strValue;
            }
            else
            {
                throw new ArgumentException($"The given string \"{strValue}\" can not be interpreted as a real number.",
                    strValue);
            }
        }

        /// <summary>
        /// Erstellt eine neue Instanz einer reellen Zahl.
        /// </summary>
        /// <param name="strValue">Die interne Darstellungsform</param>
        /// <param name="bIntern"></param>
        private protected Real(string strValue, bool bIntern)
        {
            Value = strValue;
        }

        public override bool Equals(object obj)
        {
            if (obj is Real r)
            {
                return Equals(r);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return Value;
        }

        protected bool Equals(Real other)
        {
            string strCmp = NativeImplementation.ProcessData("(" + Value + ")-(" + other.Value + ")");

            return strCmp == "0" || (strCmp.Length >= ROUND_PRECISION &&
                                     strCmp.TrimStart('-').Substring(2, ROUND_PRECISION - 1) == EQUALITY_PRECISION);
        }

        public static implicit operator string(Real rhs)
        {
            return rhs.Value;
        }

        public static implicit operator Real(string rhs)
        {
            return new Real(rhs);
        }

        public static implicit operator Real(int rhs)
        {
            return new Real(rhs.ToString());
        }

        public static implicit operator Real(uint rhs)
        {
            return new Real(rhs.ToString());
        }

        public static implicit operator Real(float rhs)
        {
            return new Real(rhs.ToString(CultureInfo.InvariantCulture));
        }

        public static implicit operator Real(double rhs)
        {
            return new Real(rhs.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Führt eine Addition durch.
        /// </summary>
        /// <param name="left">Linker Operand</param>
        /// <param name="right">Rechter Operand</param>
        /// <returns>Ergebnis der Operation</returns>
        public static Real operator +(Real left, Real right)
        {
            return new Real(NativeImplementation.ProcessData("(" + left.Value + ")+(" + right.Value + ")"), true);
        }

        /// <summary>
        /// Führt eine Subtraktion durch.
        /// </summary>
        /// <param name="left">Linker Operand</param>
        /// <param name="right">Rechter Operand</param>
        /// <returns>Ergebnis der Operation</returns>
        public static Real operator -(Real left, Real right)
        {
            return new Real(NativeImplementation.ProcessData("(" + left.Value + ")-(" + right.Value + ")"), true);
        }

        /// <summary>
        /// Führt eine Multiplikation durch.
        /// </summary>
        /// <param name="left">Linker Operand</param>
        /// <param name="right">Rechter Operand</param>
        /// <returns>Ergebnis der Operation</returns>
        public static Real operator *(Real left, Real right)
        {
            return new Real(NativeImplementation.ProcessData("(" + left.Value + ")*(" + right.Value + ")"), true);
        }

        /// <summary>
        /// Führt eine Division durch.
        /// </summary>
        /// <param name="left">Linker Operand</param>
        /// <param name="right">Rechter Operand</param>
        /// <returns>Ergebnis der Operation</returns>
        public static Real operator /(Real left, Real right)
        {
            return new Real(NativeImplementation.ProcessData("(" + left.Value + ")/(" + right.Value + ")"), true);
        }

        /// <summary>
        /// Führt eine Exponentiation durch.
        /// </summary>
        /// <param name="left">Linker Operand</param>
        /// <param name="right">Rechter Operand</param>
        /// <returns>Ergebnis der Operation</returns>
        public static Real operator ^(Real left, Real right)
        {
            return new Real(NativeImplementation.ProcessData("(" + left.Value + ")^(" + right.Value + ")"), true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left">Linker Operand</param>
        /// <param name="right">Rechter Operand</param>
        /// <returns>Ergebnis der Operation</returns>
        public static Real operator %(Real left, Real right)
        {
            return new Real(NativeImplementation.ProcessData("(" + left.Value + ")%(" + right.Value + ")"), true);
        }

        /// <summary>
        /// ++-Operator
        /// </summary>
        /// <param name="left">Linker Operand</param>
        /// <param name="right">Rechter Operand</param>
        /// <returns>Ergebnis der Operation</returns>
        public static Real operator ++(Real rhs)
        {
            return new Real(NativeImplementation.ProcessData("(" + rhs.Value + ")+(1)"), true);
        }

        /// <summary>
        /// ---Operator
        /// </summary>
        /// <param name="left">Linker Operand</param>
        /// <param name="right">Rechter Operand</param>
        /// <returns>Ergebnis der Operation</returns>
        public static Real operator --(Real rhs)
        {
            return new Real(NativeImplementation.ProcessData("(" + rhs.Value + ")-(1)"), true);
        }

        /// <summary>
        /// </summary>
        /// <param name="left">Linker Operand</param>
        /// <param name="right">Rechter Operand</param>
        /// <returns>Ergebnis der Operation</returns>
        public static bool operator >(Real left, Real right)
        {
            return !NativeImplementation.ProcessData("(" + left.Value + ")-(" + right.Value + ")").StartsWith("-");
        }

        /// <summary>
        /// </summary>
        /// <param name="left">Linker Operand</param>
        /// <param name="right">Rechter Operand</param>
        /// <returns>Ergebnis der Operation</returns>
        public static bool operator <(Real left, Real right)
        {
            return NativeImplementation.ProcessData("(" + left.Value + ")-(" + right.Value + ")").StartsWith("-");
        }

        /// <summary>
        /// Gleichheitsoperator
        /// </summary>
        /// <param name="left">Linker Operand</param>
        /// <param name="right">Rechter Operand</param>
        /// <returns>Ergebnis der Operation</returns>
        public static bool operator ==(Real left, Real right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Ungleichheitsoperator
        /// </summary>
        /// <param name="left">Linker Operand</param>
        /// <param name="right">Rechter Operand</param>
        /// <returns>Ergebnis der Operation</returns>
        public static bool operator !=(Real left, Real right)
        {
            return !Equals(left, right);
        }

        private static class NativeImplementation
        {
            [DllImport("./spgt/spgt-wrp.dll")]
            private static extern IntPtr parse(IntPtr expression, int digitlimit);

            public static string ProcessData(string strData)
            {
                IntPtr p_data = Marshal.StringToHGlobalAnsi(strData);
                string strResult = Marshal.PtrToStringAnsi(parse(p_data, ROUND_PRECISION));
                Marshal.FreeHGlobal(p_data);
                return strResult;
            }
        }
    }
}
