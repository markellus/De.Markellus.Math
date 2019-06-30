using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace De.Markellus.Maths.Core.Arithmetic
{
    public class Real
    {
        private const int ROUND_PRECISION = 500;

        private static Regex regexDecimal = new Regex("^[\\+\\-]?[0-9]+\\.[0-9]+$", RegexOptions.Singleline);
        private static Regex regexNumber = new Regex("^[\\+\\-]?[0-9]+$", RegexOptions.Singleline);
        private static Regex regexPrecisionDetection = new Regex("([0-9])\\1{" + (ROUND_PRECISION - 1) + "}");

        private string _strReal;

        public Real()
        {
            _strReal = "0.0";
        }

        public Real(string strNumber)
        {
            _strReal = strNumber;
            ValidateNumber();
        }

        public override string ToString()
        {
            return _strReal;
        }

        public override bool Equals(object obj)
        {
            if (obj is Real r)
            {
                return Equals(r);
            }

            return false;
        }

        protected bool Equals(Real other)
        {
            return SpigotClient.Subtract(_strReal, other._strReal) == "0";
        }

        public static implicit operator string(Real rhs)
        {
            return rhs._strReal;
        }

        public static implicit operator Real(string rhs)
        {
            return new Real(rhs);
        }

        public static Real operator +(Real left, Real right)
        {
            return SpigotClient.Add(left._strReal, right._strReal);
        }

        public static Real operator -(Real left, Real right)
        {
            return SpigotClient.Subtract(left._strReal, right._strReal);
        }

        public static Real operator *(Real left, Real right)
        {
            return SpigotClient.Multiply(left._strReal, right._strReal);
        }

        public static Real operator /(Real left, Real right)
        {
            return SpigotClient.Divide(left._strReal, right._strReal);
        }

        public static Real operator ^(Real left, Real right)
        {
            return SpigotClient.Pow(left._strReal, right._strReal);
        }

        public static Real operator ++(Real rhs)
        {
            rhs._strReal = SpigotClient.Add(rhs, "1");
            return rhs;
        }

        public static Real operator --(Real rhs)
        {
            rhs._strReal = SpigotClient.Subtract(rhs, "1");
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

        public void RoundPeriod()
        {
            string[] strParts = _strReal.Split(new char[] { '.' });

            Match matchPrecision = regexPrecisionDetection.Match(strParts[1]);

            if (matchPrecision.Success && matchPrecision.Index == 0 && strParts[1][0] == strParts[1][strParts.Length - 1])
            {
                Real round = new Real(strParts[1][0].ToString());
                if (round > "5")
                {
                    // ReSharper disable once RedundantAssignment
                    round++;
                }
                else
                {
                    // ReSharper disable once RedundantAssignment
                    round--;
                }

                if (round == "10")
                {
                    strParts[0] = SpigotClient.Add(strParts[0], "1");
                    round = "0";
                }

                strParts[1] = round._strReal.Split(new char[]{'.'})[1];

                _strReal = strParts[0] + "." + strParts[1];
                ValidateNumber();
            }
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
        }
    }
}
