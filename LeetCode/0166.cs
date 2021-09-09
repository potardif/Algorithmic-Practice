using System;
using System.Collections.Generic;
using System.Text;

public class Solution {
	public string FractionToDecimal(int numerator, int denominator) {
		bool negative = (numerator < 0 ^ denominator < 0) && numerator != 0;
		long num = Math.Abs((long)numerator);
		long denom = Math.Abs((long)denominator);

		long remainder;
		long integerPart = Math.DivRem(num, denom, out remainder);

		if (remainder == 0) {
			return FmtNumber(negative, integerPart).ToString();
		}

		var sbDecimals = new StringBuilder();
		var remainderPositions = new Dictionary<long, int>();

		for (int i = 0; ; ++i) {
			remainderPositions.Add(remainder, i);

			long digit = Math.DivRem(remainder * 10, denom, out remainder);
			sbDecimals.Append(digit);

			if (remainder == 0) {
				return FmtNumber(negative, integerPart, sbDecimals.ToString()).ToString();
			} else if (remainderPositions.TryGetValue(remainder, out int j)) {
				ReadOnlySpan<char> decimals = sbDecimals.ToString().AsSpan();
				ReadOnlySpan<char> nonRepeatingDecimals = decimals.Slice(0, j);
				ReadOnlySpan<char> repeatingDecimals = decimals.Slice(j);
				return
					FmtNumber(negative, integerPart, nonRepeatingDecimals, repeatingDecimals)
					.ToString();
			}
		}
	}

	private StringBuilder FmtNumber(bool negative, long integerPart) {
		var sb = new StringBuilder();

		if (negative) {
			sb.Append("-");
		}

		sb.Append(integerPart);

		return sb;
	}

	private StringBuilder FmtNumber(
		bool negative, long integerPart, ReadOnlySpan<char> nonRepeatingDecimals
	) {
		StringBuilder sb = FmtNumber(negative, integerPart);
		sb.Append(".");
		sb.Append(nonRepeatingDecimals);
		return sb;
	}

	private StringBuilder FmtNumber(
		bool negative,
		long integerPart,
		ReadOnlySpan<char> nonRepeatingDecimals,
		ReadOnlySpan<char> repeatingDecimals
	) {
		StringBuilder sb = FmtNumber(negative, integerPart, nonRepeatingDecimals);
		sb.Append("(");
		sb.Append(repeatingDecimals);
		sb.Append(")");
		return sb;
	}
}
