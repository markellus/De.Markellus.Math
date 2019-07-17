﻿/* Copyright (C) Marcel Bulla - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Marcel Bulla <postmaster@marcel-bulla.de>
 */

using De.Markellus.Maths.Core.Arithmetic.RealAddons;

namespace De.Markellus.Maths.Core.Arithmetic
{
    /// <summary>
    /// Diese Klasse bildet eine reelle Zahl ab.
    /// </summary>
    public class Real
    {
        /// <summary>
        /// Die verwendete interne Darstellungsform.
        /// </summary>
        private IRealAddon _addon;

        /// <summary>
        /// Der Wert der reellen Zahl in formatierter Form .
        /// </summary>
        public string Value => _addon.GetValue();

        /// <summary>
        /// Der Wert der reellen Zahl in Dezimalschreibweise. Je nach Zahl können Rundungsfehler
        /// auftreten.
        /// </summary>
        public string PlainValue => _addon.GetPlainValue();

        internal string SpigotCompatibleValue => _addon.GetSpigotCompatibleValue();

        /// <summary>
        /// Erstellt eine neue Instanz einer reellen Zahl.
        /// </summary>
        /// <param name="addon">Die interne Darstellungsform</param>
        internal Real(IRealAddon addon)
        {
            _addon = addon;
        }

        /// <summary>
        /// Kann aufgerufen werden, um die derzeitige Darstellungsform zu überprüfen und
        /// gegebenenfalls anzupassen.
        /// </summary>
        /// <param name="strCurrentInput"></param>
        internal void ReconsiderAddon(string strCurrentInput)
        {
            IRealAddon addon = RealFactory.GetMatchingAddon(strCurrentInput);

            if (addon != null && addon.GetType() != _addon.GetType())
            {
                _addon = addon;
                addon.SetValue(strCurrentInput, this);
            }
        }

        public Real LeastCommonMultiple(Real other)
        {
            return LeastCommonMultiple(this, other);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
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

        public override string ToString()
        {
            return this;
        }

        protected bool Equals(Real other)
        {
            string strPlainThis = PlainValue;
            string strPlainOther = other.PlainValue;

            if (strPlainThis.Length > RealFactory.ROUND_PRECISION)
            {
                strPlainThis = strPlainThis.Substring(0, RealFactory.ROUND_PRECISION);
            }
            if (strPlainOther.Length > RealFactory.ROUND_PRECISION)
            {
                strPlainOther = strPlainOther.Substring(0, RealFactory.ROUND_PRECISION);
            }

            return strPlainThis == strPlainOther;
            //string strSub = SpigotApi.Subtract(SpigotCompatibleValue, other.SpigotCompatibleValue);
            //return strSub == "0" || (strSub.Length > RealFactory.ROUND_PRECISION &&
            //                         strSub.TrimStart('-').Substring(0, RealFactory.ROUND_PRECISION + 2) ==
            //RealFactory.EQUALITY_PRECISION);
        }

        public static Real Root(Real realBase, Real realN)
        {
            return SpigotApi.Root(realBase.SpigotCompatibleValue, realN.SpigotCompatibleValue);
        }

        public static Real LeastCommonMultiple(Real realLeft, Real realRight)
        {
            Real big;
            if (realLeft == realRight)
            {
                return realRight;
            }
            else if (realLeft > realRight)
            {
                big = realLeft;
            }
            else
            {
                big = realRight;
            }

            for (Real i = big; i < realLeft * realRight; i++)
            {
                if (i % realLeft == 0 && i % realRight == 0)
                {
                    return i;
                }
            }

            return realLeft * realRight;
        }

        public static implicit operator string(Real rhs)
        {
            return rhs.Value;
        }

        public static implicit operator Real(string rhs)
        {
            return RealFactory.GenerateReal(rhs);
        }

        public static implicit operator Real(int rhs)
        {
            return RealFactory.GenerateReal(rhs.ToString());
        }

        public static Real operator +(Real left, Real right)
        {
            return RealFactory.GenerateReal(SpigotApi.Add(left.SpigotCompatibleValue, right.SpigotCompatibleValue));
        }

        public static Real operator -(Real left, Real right)
        {
            return RealFactory.GenerateReal(SpigotApi.Subtract(left.SpigotCompatibleValue, right.SpigotCompatibleValue));
        }

        public static Real operator *(Real left, Real right)
        {
            return RealFactory.GenerateReal(SpigotApi.Multiply(left.SpigotCompatibleValue, right.SpigotCompatibleValue));
        }

        public static Real operator /(Real left, Real right)
        {
            return RealFactory.GenerateReal(SpigotApi.Divide(left.SpigotCompatibleValue, right.SpigotCompatibleValue));
        }

        public static Real operator ^(Real left, Real right)
        {
            return RealFactory.GenerateReal(SpigotApi.Pow(left.SpigotCompatibleValue, right.SpigotCompatibleValue));
        }

        public static Real operator %(Real left, Real right)
        {
            return RealFactory.GenerateReal(SpigotApi.Mod(left.SpigotCompatibleValue, right.SpigotCompatibleValue));
        }

        public static Real operator ++(Real rhs)
        {
            return RealFactory.GenerateReal(SpigotApi.Add(rhs, "1"));
        }

        public static Real operator --(Real rhs)
        {
            return RealFactory.GenerateReal(SpigotApi.Subtract(rhs, "1"));
        }

        public static bool operator >(Real left, Real right)
        {
            return !SpigotApi.Subtract(left.SpigotCompatibleValue, right.SpigotCompatibleValue).StartsWith("-");
        }

        public static bool operator <(Real left, Real right)
        {
            return SpigotApi.Subtract(left.SpigotCompatibleValue, right.SpigotCompatibleValue).StartsWith("-");
        }

        public static bool operator ==(Real left, Real right)
        {
            return Equals(left?.SpigotCompatibleValue, right?.SpigotCompatibleValue);
        }

        public static bool operator !=(Real left, Real right)
        {
            return !Equals(left?.SpigotCompatibleValue, right?.SpigotCompatibleValue);
        }
    }
}