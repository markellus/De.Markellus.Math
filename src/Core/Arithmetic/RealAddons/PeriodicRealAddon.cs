using System;
using System.Collections.Generic;
using System.Text;

namespace De.Markellus.Maths.Core.Arithmetic.RealAddons
{
    internal class PeriodicRealAddon : IRealAddon
    {
        private string _strValue;
        private int _iPeriodSize;

        public void SetValue(string strInput, Real real)
        {
            _strValue = GetPeriodicValue(strInput);

            if (_strValue == null)
            {
                throw new ArithmeticException($"This number is not periodic or is not correctly formatted: {strInput}");
            }

            if (_iPeriodSize == 0)
            {
                real.ReconsiderAddon(_strValue);
            }
        }

        public string GetValue()
        {
            return _strValue + "p" + _iPeriodSize;
        }

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

        public bool IsEqual(IRealAddon other)
        {
            return SpigotClient.Subtract(this.GetPlainValue(), other.GetPlainValue()) == "0";
        }

        public bool IsMatch(string strInput)
        {
            return GetPeriodicValue(strInput) != null;
        }

        private string GetPeriodicValue(string strInput)
        {
            string[] strParts = strInput.Split('.');
            int iP = strInput.IndexOf('p');

            if (iP != -1)
            {
                string[] strPeriod = strParts[1].Split('p');

                if (!int.TryParse(strPeriod[1], out _iPeriodSize))
                {
                    return null;
                }

                strParts[1] = strPeriod[0];
            }
            else
            {
                Period period = strParts[1].DetectPeriod();

                if (strParts[1].Length >= RealFactory.ROUND_PRECISION && period.Pattern != null)
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
