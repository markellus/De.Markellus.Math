using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace De.Markellus.Maths.Core.Arithmetic
{
    public class Real
    {
        private const int ROUND_PRECISION = 500;

        private static Regex regexDecimal = new Regex("^[\\+\\-]?[0-9]+\\.[0-9]+[p]{0,1}[0-9]*$", RegexOptions.Singleline);
        private static Regex regexNumber = new Regex("^[\\+\\-]?[0-9]+$", RegexOptions.Singleline);
        private static Regex regexPrecisionDetection = new Regex("([0-9])\\1{" + (ROUND_PRECISION - 1) + "}");

        private string _strReal;

        private bool _bIsPeriodic;
        private int _iPeriodSize;

        public string Value
        {
            get => GetValue();
            set => SetValue(value);
        }

        public Real()
        {
            SetValue("0.0");
        }

        public Real(string strNumber) : this()
        {
            SetValue(strNumber);
        }

        public string GetValue()
        {
            return GetValue(ROUND_PRECISION, true);
        }

        public string GetValue(int iDecimals, bool bPlain)
        {
            string[] strReal = _strReal.Split('.');

            if (bPlain)
            {
                if (_bIsPeriodic)
                {
                    StringBuilder builder = new StringBuilder(iDecimals);
                    builder.Append(strReal[1].Substring(0, strReal[1].Length - _iPeriodSize));
                    string strPeriod = strReal[1].Substring(strReal[1].Length - _iPeriodSize);

                    while (builder.Length < iDecimals)
                    {
                        builder.Append(strPeriod);
                    }

                    strReal[1] = builder.ToString();
                }
            }
            else
            {
                if (_bIsPeriodic)
                {
                    return strReal[0] + "." + strReal[1] + "p" + _iPeriodSize;
                }
            }

            if (strReal[1].Length > iDecimals)
            {
                strReal[1] = strReal[1].Substring(0, iDecimals);
            }

            return strReal[0] + "." + strReal[1];
        }

        public void SetValue(string strValue)
        {
            _strReal = strValue;

            ValidateNumber();
        }

        public override string ToString()
        {
            return GetValue(ROUND_PRECISION, false);
        }

        public override bool Equals(object obj)
        {
            if (obj is Real r)
            {
                return Equals(r);
            }
            else if (obj is string s)
            {
                return Equals(new Real(s));
            }

            return false;
        }

        protected bool Equals(Real other)
        {
            return SpigotClient.Subtract(this, other) == "0" && this._bIsPeriodic == other._bIsPeriodic;
        }

        public static implicit operator string(Real rhs)
        {
            return rhs.GetValue();
        }

        public static implicit operator Real(string rhs)
        {
            return new Real(rhs);
        }

        public static Real operator +(Real left, Real right)
        {
            return SpigotClient.Add(left, right);
        }

        public static Real operator -(Real left, Real right)
        {
            return SpigotClient.Subtract(left, right);
        }

        public static Real operator *(Real left, Real right)
        {
            return SpigotClient.Multiply(left, right);
        }

        public static Real operator /(Real left, Real right)
        {
            return SpigotClient.Divide(left, right);
        }

        public static Real operator ^(Real left, Real right)
        {
            return SpigotClient.Pow(left, right);
        }

        public static Real operator ++(Real rhs)
        {
            rhs.SetValue(SpigotClient.Add(rhs, "1"));
            return rhs;
        }

        public static Real operator --(Real rhs)
        {
            rhs.SetValue(SpigotClient.Subtract(rhs, "1"));
            return rhs;
        }

        public static bool operator >(Real left, Real right)
        {
            return !SpigotClient.Subtract(left, right).StartsWith("-");
        }

        public static bool operator <(Real left, Real right)
        {
            return SpigotClient.Subtract(left, right).StartsWith("-");
        }

        public static bool operator ==(Real left, Real right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Real left, Real right)
        {
            return !Equals(left, right);
        }

        private void DetectPeriod()
        {
            string[] strParts = _strReal.Split('.');
            int iP = _strReal.IndexOf('p');

            if (iP != -1)
            {
                _bIsPeriodic = true;
                string[] strPeriod = strParts[1].Split('p');

                if (!int.TryParse(strPeriod[1], out _iPeriodSize))
                {
                    throw new ArithmeticException("Invalid argument for a periodic number!");
                }

                strParts[1] = strPeriod[0];
            }
            else
            {
                Period period = strParts[1].DetectPeriod();

                if (strParts[1].Length >= ROUND_PRECISION && period.Pattern != null)
                {
                    if (period.Pattern == "9")
                    {
                        strParts[0] = SpigotClient.Add(strParts[0], "1");
                        strParts[1] = "0";
                    }
                    else if (period.Pattern == "0")
                    {
                        strParts[1] = "0";
                    }
                    else
                    {
                        strParts[1] = strParts[1].Substring(0, period.Offset) + period.Pattern;
                        _bIsPeriodic = true;
                        _iPeriodSize = period.Pattern.Length;
                    }
                }
            }

            _strReal = strParts[0] + "." + strParts[1];
        }

        private void ValidateNumber()
        {
            if (regexNumber.IsMatch(_strReal))
            {
                _strReal += ".0";
            }
            else
            {
                _strReal = _strReal.Replace(',', '.');
            }

            if (!regexDecimal.IsMatch(_strReal))
            {
                throw new ArithmeticException($"The given string {_strReal} can not be interpreted as a real number.");
            }

            DetectPeriod();
        }
    }
}
