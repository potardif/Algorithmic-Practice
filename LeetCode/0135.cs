using System.Linq;

public class Solution {
	public int Candy(int[] ratings) {
		int n = ratings.Length;

		var candies = new int[n];
		candies[0] = 1;

		for (int i = 1; i < n; ++i) {
			if (ratings[i - 1] < ratings[i]) {
				candies[i] = candies[i - 1] + 1;
			} else {
				candies[i] = 1;
			}
		}

		for (int i = n - 2; i >= 0; --i) {
			if (ratings[i] > ratings[i + 1] && candies[i] <= candies[i + 1]) {
				candies[i] = candies[i + 1] + 1;
			}
		}

		return candies.Sum();
	}
}
