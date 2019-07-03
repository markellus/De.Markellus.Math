using System;
using System.Collections.Generic;
using System.Text;

namespace De.Markellus.Maths.Core.Arithmetic.RealAddons
{
    internal class StringRealAddon : IRealAddon
    {
        private string _strValue;

        public void SetValue(string strInput, Real real)
        {
            _strValue = strInput;
        }

        public string GetValue()
        {
            return _strValue;
        }

        public string GetPlainValue()
        {
            return _strValue;
        }

        public bool IsEqual(IRealAddon other)
        {
            return SpigotClient.Subtract(this.GetPlainValue(), other.GetPlainValue()) == "0";
        }

        public bool IsMatch(string strInput)
        {
            return true;
        }
    }
}
