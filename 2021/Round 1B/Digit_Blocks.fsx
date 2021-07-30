open System
open System.Collections.Generic
open System.Linq

let highest indexes (towers : int[]) = indexes |> Seq.maxBy (fun i -> towers.[i])

let sameHeight (indexes : seq<int>) (towers : int[]) =
  let h = towers.[indexes.First()]
  indexes.Skip(1).All(fun i -> towers.[i] = h)

let placeBlock i towers =
  let towers = Array.copy towers
  towers.[i] <- towers.[i] + 1
  towers

let chooseTower (cache : Dictionary<string, float>) N B towers D =
  let rec chooseTower (towers : int[]) D =
    let available = { 0 .. N - 1 }.Where(fun i -> towers.[i] < B)
    // If the digit is 9, the optimal move is always to put it on the highest tower.
    if D = 9 then highest available towers
    // If the available towers are all the same height, any one will do.
    else if sameHeight available towers then available.First()
    // If all available towers are of non critical height, just use the greedy strategy.
    else if available.All(fun i -> towers.[i] < B - 2) then highest available towers
    // If there are towers of critical height, compare expected scores.
    else
      let candidates = List()
      candidates.AddRange(available.Where(fun i -> towers.[i] = B - 1).Take(1))
      candidates.AddRange(available.Where(fun i -> towers.[i] = B - 2).Take(1))
      candidates.AddRange(available.Where(fun i -> towers.[i] < B - 2)
        .OrderByDescending(fun i -> towers.[i]).ThenBy(fun i -> i).Take(1))

      candidates |> Seq.maxBy (fun i ->
        let rank = towers.[i]
        let towers = placeBlock i towers
        float D * Math.Pow(10., float rank) + getExpectedScore towers
      )
  
  and getExpectedScore towers =
    let key = String.Join("|", towers)
    match cache.TryGetValue(key) with
    | (true, expectedScore) -> expectedScore
    | (false, _) ->
        let expectedScore =
          if towers.All(fun h -> h = B) then 0.
          else
            let mutable e = 0.
            for D in 0 .. 9 do
              let i = chooseTower towers D
              let rank = towers.[i]
              let towers = placeBlock i towers
              e <- e + 0.1 * (float D * Math.Pow(10., float rank) + getExpectedScore towers)
            e
        cache.Add(key, expectedScore)
        expectedScore

  chooseTower towers D

let buildTowers cache N B =
  let towers = Array.init N (fun _ -> 0)
  for _ in 1 .. N * B do
    let D = Console.ReadLine() |> int
    let i = chooseTower cache N B towers D
    towers.[i] <- towers.[i] + 1
    printfn "%d" (i + 1)

let () =
  let tokens = Console.ReadLine().Split(" ")
  let T = int tokens.[0]
  let N = int tokens.[1] // Number of towers
  let B = int tokens.[2] // Number of blocks per tower
  let _P = int64 tokens.[3]

  let cache = Dictionary()
  for _ in 1 .. T do
    buildTowers cache N B

  let verdict = Console.ReadLine() |> int
  if verdict = -1 then failwith "Unfavorable verdict"
