using System;
using System.Diagnostics;

namespace ClEngine.CoreLibrary.Particle
{
	internal static class Guard
	{
		[Conditional("DEBUG")]
		public static void ArgumentNullOrEmpty(string parameter, string argument)
		{
			if (string.IsNullOrEmpty(argument))
				throw new ArgumentNullException(parameter);
		}

		[Conditional("DEBUG")]
		public static void IsTrue(bool expression, string message)
		{
			if (expression)
				throw new InvalidOperationException(message);
		}

		[Conditional("DEBUG")]
		public static void ArgumentLessThan<T>(string parameter, T argument, T threshold) where T : IComparable<T>
		{
			if (argument.CompareTo(threshold) < 0)
				throw new ArgumentOutOfRangeException(parameter);
		}

		[Conditional("DEBUG")]
		public static void ArgumentNotFinite(string parameter, float argument)
		{
			if (float.IsNaN(argument) || float.IsNegativeInfinity(argument) || float.IsPositiveInfinity(argument))
				throw new NotFiniteNumberException(argument);
		}

		[Conditional("DEBUG")]
		public static void ArgumentNull(string parameter, object argument)
		{
			if (argument == null)
				throw new ArgumentNullException(parameter);
		}
	}
}