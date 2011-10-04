﻿#region Copyright & License Information
/*
 * Copyright 2007-2011 The OpenRA Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation. For more information,
 * see COPYING.
 */
#endregion

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;

namespace OpenRA
{
	public static class Exts
	{
		public static string F(this string fmt, params object[] args)
		{
			return string.Format(fmt, args);
		}

		public static T WithDefault<T>(T def, Func<T> f)
		{
			try { return f(); }
			catch { return def; }
		}

		public static void Do<T>(this IEnumerable<T> e, Action<T> fn)
		{
			foreach (var ee in e)
				fn(ee);
		}

		public static IEnumerable<string> GetNamespaces(this Assembly a)
		{
			return a.GetTypes().Select(t => t.Namespace).Distinct().Where(n => n != null);
		}

		public static string ReadAllText(this Stream s)
		{
			using (s)
			using (var sr = new StreamReader(s))
				return sr.ReadToEnd();
		}

		public static byte[] ReadAllBytes(this Stream s)
		{
			using (s)
			{
				var data = new byte[s.Length - s.Position];
				s.Read(data, 0, data.Length);
				return data;
			}
		}

		public static void Write(this Stream s, byte[] data)
		{
			s.Write(data, 0, data.Length);
		}

		public static IEnumerable<string> ReadAllLines(this Stream s)
		{
			using (var sr = new StreamReader(s))
				for (; ; )
				{
					var line = sr.ReadLine();
					if (line == null)
						yield break;
					else
						yield return line;
				}
		}

		public static bool HasAttribute<T>(this MemberInfo mi)
		{
			return mi.GetCustomAttributes(typeof(T), true).Length != 0;
		}

		public static T[] GetCustomAttributes<T>(this MemberInfo mi, bool inherit)
			where T : class
		{
			return (T[])mi.GetCustomAttributes(typeof(T), inherit);
		}

		public static T[] GetCustomAttributes<T>(this ParameterInfo mi)
			where T : class
		{
			return (T[])mi.GetCustomAttributes(typeof(T), true);
		}

		public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
		{
			if (val.CompareTo(min) < 0)
				return min;
			else if (val.CompareTo(max) > 0)
				return max;
			else
				return val;
		}

		public static bool Contains(this Rectangle r, int2 p)
		{
			return r.Contains(p.ToPoint());
		}

		public static bool Contains(this RectangleF r, int2 p)
		{
			return r.Contains(p.ToPointF());
		}

        public static bool HasModifier(this Modifiers k, Modifiers mod)
        {
            return (k & mod) == mod;
        }

		public static bool IsValidInput(this KeyInput key)
		{
			return char.IsLetter(key.UnicodeChar) || char.IsDigit(key.UnicodeChar) ||
					char.IsSymbol(key.UnicodeChar) || char.IsSeparator(key.UnicodeChar) ||
					char.IsPunctuation(key.UnicodeChar);
		}

        public static V GetOrAdd<K, V>(this Dictionary<K, V> d, K k)
            where V : new()
        {
            return d.GetOrAdd(k, _ => new V());
        }

        public static V GetOrAdd<K, V>(this Dictionary<K, V> d, K k, Func<K, V> createFn)
        {
            V ret;
            if (!d.TryGetValue(k, out ret))
                d.Add(k, ret = createFn(k));
            return ret;
        }

        public static T Random<T>(this IEnumerable<T> ts, Thirdparty.Random r)
        {
            var xs = ts.ToArray();
            return xs[r.Next(xs.Length)];
        }

        public static float Product(this IEnumerable<float> xs)
        {
            return xs.Aggregate(1f, (a, x) => a * x);
        }

        public static IEnumerable<T> SymmetricDifference<T>(this IEnumerable<T> xs, IEnumerable<T> ys)
        {
            // this is probably a shockingly-slow way to do this, but it's concise.
            return xs.Except(ys).Concat(ys.Except(xs));
        }

		public static IEnumerable<T> Iterate<T>( this T t, Func<T,T> f )
		{
			for(;;) { yield return t; t = f(t); }
		}

		public static int NextPowerOf2(int v)
		{
			--v;
			v |= v >> 1;
			v |= v >> 2;
			v |= v >> 4;
			v |= v >> 8;
			++v;
			return v;
		}
	}
}
