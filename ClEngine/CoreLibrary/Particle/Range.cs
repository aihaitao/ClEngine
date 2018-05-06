using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace ClEngine.CoreLibrary.Particle
{
	[Serializable]
	public struct Range : IEquatable<Range>, IFormattable
	{
		public Range(float minimum, float maximum)
		{
			Minimum = minimum;
			Maximum = maximum;
		}

		public float Minimum;

		public float Maximum;

		public float Size => Calculator.Abs(Maximum - Minimum);

		public bool Contains(Range range)
		{
			return Minimum <= range.Minimum &&
			       Maximum >= range.Maximum;
		}

		public bool Contains(float value)
		{
			return Minimum <= value &&
			       Maximum >= value;
		}

		public void Merge(Range value)
		{
			Minimum = Minimum < value.Minimum ? Minimum : value.Minimum;
			Maximum = Maximum > value.Maximum ? Maximum : value.Maximum;
		}

		public void Intersect(Range value)
		{
			Minimum = Minimum > value.Minimum ? Minimum : value.Minimum;
			Maximum = Maximum < value.Maximum ? Maximum : value.Maximum;
		}

		public void Subtract(Range value)
		{
			var intersection = Intersect(this, value);

			if (intersection.Minimum > Minimum)
				Maximum = intersection.Minimum;

			else if (intersection.Maximum > Minimum)
				Minimum = intersection.Maximum;
		}

		public static Range Union(Range x, Range y)
		{
			return new Range
			{
				Minimum = x.Minimum < y.Minimum ? x.Minimum : y.Minimum,
				Maximum = x.Maximum > y.Maximum ? x.Maximum : y.Maximum
			};
		}

		public static Range Intersect(Range x, Range y)
		{
			return new Range
			{
				Minimum = x.Minimum > y.Minimum ? x.Minimum : y.Minimum,
				Maximum = x.Maximum < y.Maximum ? x.Maximum : y.Maximum
			};
		}

		public static Range Subtract(Range x, Range y)
		{
			var intersection = Intersect(x, y);

			var value = new Range();

			if (intersection.Minimum > x.Minimum)
				value.Maximum = intersection.Minimum;

			else if (intersection.Maximum > x.Maximum)
				value.Minimum = intersection.Maximum;

			return value;
		}

		public static Range Parse(string value)
		{
			return Parse(value, CultureInfo.InvariantCulture);
		}

		private static Range Parse(string value, IFormatProvider format)
		{
			Guard.ArgumentNull("value", value);
			Guard.ArgumentNull("format", format);

			if (!value.StartsWith("[") || !value.EndsWith("]"))
				goto badformat;

			var numberFormat = NumberFormatInfo.GetInstance(format);

			var groupSeperator = numberFormat.NumberGroupSeparator.ToCharArray();

			var endpoints = value.Trim('[', ']').Split(groupSeperator);

			if (endpoints.Length != 2)
				goto badformat;

			return new Range
			{
				Minimum = float.Parse(endpoints[0], NumberStyles.Float, numberFormat),
				Maximum = float.Parse(endpoints[1], NumberStyles.Float, numberFormat)
			};

			badformat:
			throw new FormatException("封闭区间的值不符合ISO 31-11格式.");
		}

		public override bool Equals(object obj)
		{
			if (obj is Range range)
				return Equals(range);

			return false;
		}

		public bool Equals(Range value)
		{
			return Minimum.Equals(value.Minimum) &&
			       Maximum.Equals(value.Maximum);
		}

		[SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
		public override int GetHashCode()
		{
			return Minimum.GetHashCode() ^ Maximum.GetHashCode();
		}

		public override string ToString()
		{
			return ToString("G", CultureInfo.InvariantCulture);
		}

		public string ToString(IFormatProvider formatProvider)
		{
			return ToString("G", formatProvider);
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			var numberFormat = NumberFormatInfo.GetInstance(formatProvider);

			var minimum = Minimum.ToString(format, numberFormat);
			var maximum = Maximum.ToString(format, numberFormat);
			var seperator = numberFormat.NumberGroupSeparator;

			return string.Format(formatProvider, "[{0}{1}{2}]", minimum, seperator, maximum);
		}

		public static Range operator +(Range x, Range y)
		{
			return Union(x, y);
		}

		public static Range operator -(Range x, Range y)
		{
			return Subtract(x, y);
		}

		public static Range operator |(Range x, Range y)
		{
			return Intersect(x, y);
		}

		public static bool operator ==(Range x, Range y)
		{
			return x.Equals(y);
		}

		public static bool operator !=(Range x, Range y)
		{
			return !x.Equals(y);
		}
	}
}