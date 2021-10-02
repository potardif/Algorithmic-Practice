using System;
using System.Collections.Generic;

public class Solution {
	public int MaxPoints(int[][] points) {
		int max = 1;

		for (int i = 0; i < points.Length; ++i) {
			int[] p1 = points[i];
			int x1 = p1[0];
			int y1 = p1[1];

			var nbPoints = new Dictionary<(bool, int, int), int>();

			for (int j = i + 1; j < points.Length; ++j) {
				int[] p2 = points[j];
				int x2 = p2[0];
				int y2 = p2[1];

				int numerator = y2 - y1;
				int denominator = x2 - x1;

				bool negative =
					(numerator < 0 ^ denominator < 0) && numerator != 0 && denominator != 0;
				numerator = Math.Abs(numerator);
				denominator = Math.Abs(denominator);

				int gcd = Gcd(numerator, denominator);
				(bool, int, int) slope = (negative, numerator / gcd, denominator / gcd);

				int n;
				if (!nbPoints.TryGetValue(slope, out n)) {
					n = 1;
				}
				nbPoints[slope] = n + 1;
			}

			foreach (int n in nbPoints.Values) {
				max = Math.Max(max, n);
			}
		}

		return max;
	}

	private int Gcd(int a, int b) {
		while (a != 0 && b != 0) {
			if (a > b) {
				a %= b;
			} else {
				b %= a;
			}
		}
		return a | b;
	}
}
