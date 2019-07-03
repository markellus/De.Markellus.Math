using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using De.Markellus.Maths.Core.Arithmetic.RealAddons;

namespace De.Markellus.Maths.Core.Arithmetic
{
    public static class RealFactory
    {
        public const int ROUND_PRECISION = 500;

        private static Regex _regexDecimal;
        private static Regex _regexNumber;

        private static List<IRealAddon> _listAddons;

        static RealFactory()
        {
            _regexDecimal = new Regex("^[\\+\\-]?[0-9]+\\.[0-9]+[p]{0,1}[0-9]*$", RegexOptions.Singleline);
            _regexNumber = new Regex("^[\\+\\-]?[0-9]+$", RegexOptions.Singleline);

            _listAddons = new List<IRealAddon>();

            RegisterAddon(new PeriodicRealAddon());
            RegisterAddon(new StringRealAddon());
        }

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

        private static void RegisterAddon(IRealAddon addon)
        {
            _listAddons.Add(addon);
        }

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
