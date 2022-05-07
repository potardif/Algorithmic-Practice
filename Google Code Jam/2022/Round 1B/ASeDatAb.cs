using System;
using System.Collections.Generic;
using System.Linq;

class Program {
	private static void Main(string[] _args) {
		int T = int.Parse(Console.ReadLine());
		for (int x = 1; x <= T; ++x) {
			TestCase();
		}
	}

	private static void TestCase() {
		var possibleXs = new HashSet<byte>();
		for (int X = 0x01; X <= 0xFF; ++X) {
			possibleXs.Add((byte)X);
		}

		while (true) {
			// Taking the first element of the set shouldn't work for Test Set 2, yet it does... If
			// you take an element at random from the set, it will NOT work! I don't understand why
			// it works, but it's almost more interesting knowing that it does than the offical
			// solution.
			byte V = possibleXs.First();

			Console.WriteLine(Convert.ToString(V, 2).PadLeft(8, '0'));

			int setBits = int.Parse(Console.ReadLine());
			if (setBits == 0) return; // Solved!
			else if (setBits == -1) Environment.Exit(0);

			var possibleXs2 = new HashSet<byte>();
			foreach (byte X in possibleXs) {
				for (int r = 0; r <= 7; ++r) {
					byte W = V.RotateRight(r);
					byte X2 = (byte)(X ^ W);
					if (X2.CountSetBits() == setBits) {
						possibleXs2.Add(X2);
					}
				}
			}

			possibleXs = possibleXs2;
		}
	}
}

static class Helper {
	public static int CountSetBits(this byte value) {
		int setBits = 0;

		for (int i = 0; i < 8; ++i) {
			bool isSet = ((value >> i) & 1) != 0;
			if (isSet) {
				++setBits;
			}
		}

		return setBits;
	}

	public static byte RotateRight(this byte value, int offset) {
		return (byte)((value >> offset) | (value << (8 - offset)));
	}
}
