using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Solution {
	public IList<IList<string>> Partition(string s) {
		int[] radii = ManachersAlgorithm(s);

		var dp = new List<List<(int, int)>>[s.Length + 1];
		dp[s.Length] = new List<List<(int, int)>>() { new List<(int, int)>() };

		for (int i = s.Length - 1; i >= 0; --i) {
			dp[i] = new List<List<(int, int)>>();

			foreach (int initialPalindromeLen in FindInitialPalindromes(radii, i)) {
				(int, int) initialPalindrome = (i, initialPalindromeLen);
				foreach (List<(int, int)> rightPartition in dp[i + initialPalindromeLen]) {
					var partition = new List<(int, int)>(1 + rightPartition.Count);
					partition.Add(initialPalindrome);
					partition.AddRange(rightPartition);

					dp[i].Add(partition);
				}
			}
		}

		return dp[0].Select(partition =>
			(IList<string>)partition.Select(palindrome => {
				(int i, int len) = palindrome;
				return s.Substring(i, len);
			}).ToList()
		).ToList();
	}

	private IEnumerable<int> FindInitialPalindromes(int[] radii, int i) {
		int i0 = i * 2 + 1;
		int middle = i0 + (radii.Length - i0) / 2 + 1;
		for (i = i0; i < middle; ++i) {
			int desiredRadius = i - i0 + 1;
			if (radii[i] >= desiredRadius) {
				yield return desiredRadius;
			}
		}
	}

	private int[] ManachersAlgorithm(string s) {
		var sb = new StringBuilder(2 * s.Length + 1);
		char sep = '|';
		sb.Append(sep);
		foreach (char c in s) {
			sb.Append(c);
			sb.Append(sep);
		}
		s = sb.ToString();

		var radii = new int[s.Length];
		int center = 0;
		int radius = 0;

		while (center < s.Length) {
			while (center - (radius + 1) >= 0
				&& center + radius + 1 < s.Length
				&& s[center - (radius + 1)] == s[center + radius + 1]) {
				++radius;
			}

			radii[center] = radius;

			int oldCenter = center;
			int oldRadius = radius;
			++center;
			radius = 0;
			while (center <= oldCenter + oldRadius) {
				int mirroredCenter = oldCenter - (center - oldCenter);
				int maxMirroredRadius = oldCenter + oldRadius - center;
				if (radii[mirroredCenter] < maxMirroredRadius) {
					radii[center] = radii[mirroredCenter];
					++center;
				} else if (radii[mirroredCenter] > maxMirroredRadius) {
					radii[center] = maxMirroredRadius;
					++center;
				} else {
					radius = maxMirroredRadius;
					break;
				}
			}
		}

		return radii;
	}
}
