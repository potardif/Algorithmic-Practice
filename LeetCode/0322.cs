public class Solution {
	public int CoinChange(int[] coins, int amount) {
		const int IMPOSSIBLE = int.MaxValue;

		var nbCoins = new int[amount + 1];
		Array.Fill(nbCoins, IMPOSSIBLE);
		nbCoins[0] = 0;

		for (int i = 1; i <= amount; ++i) {
			foreach (int coin in coins) {
				int prevAmount = i - coin;
				if (prevAmount >= 0 && nbCoins[prevAmount] != IMPOSSIBLE) {
					nbCoins[i] = Math.Min(nbCoins[i], nbCoins[prevAmount] + 1);
				}
			}
		}

		return nbCoins[amount] == IMPOSSIBLE ? -1 : nbCoins[amount];
	}
}
