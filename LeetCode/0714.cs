using System;

public class Solution {
	public int MaxProfit(int[] prices, int fee) {
		int own = -prices[0];
		int dont_own = 0;

		for (int i = 1; i < prices.Length; ++i) {
			int price = prices[i];

			int buy = dont_own - price;
			int sell = own + price - fee;

			own = Math.Max(own, buy);
			dont_own = Math.Max(dont_own, sell);
		}

		return dont_own;
	}
}
