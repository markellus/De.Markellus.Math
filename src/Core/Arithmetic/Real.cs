using System;
using System.Collections.Generic;
using System.Text;
using De.Markellus.Maths.Core.Arithmetic.RealAddons;

namespace De.Markellus.Maths.Core.Arithmetic
{
    public class Real
    {
        private IRealAddon _addon;

        public string Value => _addon.GetValue();

        public string PlainValue => _addon.GetPlainValue();

        internal Real(IRealAddon addon)
        {
            _addon = addon;
        }

        internal void ReconsiderAddon(string strCurrentInput)
        {
            IRealAddon addon = RealFactory.GetMatchingAddon(strCurrentInput);

            if (addon != null && addon.GetType() != _addon.GetType())
            {
                _addon = addon;
                addon.SetValue(strCurrentInput, this);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is Real r)
            {
                return Equals(r);
            }
            else if (obj is string s)
            {
                return Equals(RealFactory.GenerateReal(s));
            }

            return false;
        }

        protected bool Equals(Real other)
        {
            return _addon.IsEqual(other._addon);
        }

        public static implicit operator string(Real rhs)
        {
            return rhs.Value;
        }

        public static implicit operator Real(string rhs)
        {
            return RealFactory.GenerateReal(rhs);
        }

        public static Real operator +(Real left, Real right)
        {
            return RealFactory.GenerateReal(SpigotClient.Add(left.PlainValue, right.PlainValue));
        }

        public static Real operator -(Real left, Real right)
        {
            return RealFactory.GenerateReal(SpigotClient.Subtract(left.PlainValue, right.PlainValue));
        }

        public static Real operator *(Real left, Real right)
        {
            return RealFactory.GenerateReal(SpigotClient.Multiply(left.PlainValue, right.PlainValue));
        }

        public static Real operator /(Real left, Real right)
        {
            return RealFactory.GenerateReal(SpigotClient.Divide(left.PlainValue, right.PlainValue));
        }

        public static Real operator ^(Real left, Real right)
        {
            return RealFactory.GenerateReal(SpigotClient.Pow(left.PlainValue, right.PlainValue));
        }

        public static Real operator ++(Real rhs)
        {
            return RealFactory.GenerateReal(SpigotClient.Add(rhs, "1"));
        }

        public static Real operator --(Real rhs)
        {
            return RealFactory.GenerateReal(SpigotClient.Subtract(rhs, "1"));
        }

        public static bool operator >(Real left, Real right)
        {
            return !SpigotClient.Subtract(left.PlainValue, right.PlainValue).StartsWith("-");
        }

        public static bool operator <(Real left, Real right)
        {
            return SpigotClient.Subtract(left.PlainValue, right.PlainValue).StartsWith("-");
        }

        public static bool operator ==(Real left, Real right)
        {
            return Equals(left?.PlainValue, right?.PlainValue);
        }

        public static bool operator !=(Real left, Real right)
        {
            return !Equals(left?.PlainValue, right?.PlainValue);
        }
    }
}
