using System;
using System.Collections.Generic;
using System.Linq;

class Program {
	private static void Main(string[] _args) {
		int T = int.Parse(Console.ReadLine());
		for (int x = 1; x <= T; ++x) {
			ExploreCave();
		}
	}

	private static void ExploreCave() {
		(int N, int K) = ReadResponse();

		int[] samplingOrder = Enumerable.Range(1, N).ToArray();
		Shuffle(new Random(), samplingOrder);

		var visited = new HashSet<int>();
		long nbPassagesW = 0;
		int nbRoomsT = 0;
		long nbPassagesT = 0;

		(int Ri, int Pi) = ReadResponse();
		visited.Add(Ri);
		nbPassagesW += Pi;

		char? lastOperation = null;
		int nextRoom = 0; // For teleportation
		while (K > 0) {
			if (lastOperation == 'T') {
				// Walk
				lastOperation = 'W';
				Console.WriteLine(lastOperation);
				
				(Ri, Pi) = ReadResponse();
				if (visited.Add(Ri)) {
					nbPassagesW += Pi;
				}
			} else {
				// Teleport
				while (visited.Contains(samplingOrder[nextRoom])) ++nextRoom;
				lastOperation = 'T';
				Console.WriteLine($"{lastOperation} {samplingOrder[nextRoom]}");
				
				(Ri, Pi) = ReadResponse();
				visited.Add(Ri);
				++nbRoomsT;
				nbPassagesT += Pi;
			}

			--K;
		}

		long estimate = (long)Math.Round(
			(nbPassagesW + nbPassagesT + (double)nbPassagesT / nbRoomsT * (N - visited.Count)) / 2
		);
		Console.WriteLine($"E {estimate}");
	}

	private static (int, int) ReadResponse() {
		string response = Console.ReadLine();
		if (response == "-1")
			throw new Exception(response);

		string[] tokens = response.Split(" ");
		int x1 = int.Parse(tokens[0]);
		int x2 = int.Parse(tokens[1]);

		return (x1, x2);
	}

	/// https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle#The_modern_algorithm
	private static void Shuffle<T>(Random random, IList<T> a) {
		int n = a.Count;
		for (int i = n - 1; i >= 1; --i) {
			int j = random.Next(i + 1);

			T tmp = a[i];
			a[i] = a[j];
			a[j] = tmp;
		}
	}
}
