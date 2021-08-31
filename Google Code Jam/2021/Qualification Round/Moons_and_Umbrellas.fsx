open System
open System.Collections.Generic
open System.Text

type MuralSpot =
  | C // Waning moon.
  | J // Closed umbrella.
  | E // Empty space.

let muralToString mural =
  let sb = StringBuilder(List.length mural)
  for spot in mural do
    sb.Append(
      match spot with
      | C -> 'C'
      | J -> 'J'
      | E -> '?'
    ) |> ignore
  sb.ToString()

let copyrightCost cjCost jcCost s =
  let memoized = Dictionary()
  let rec recursion s =
    // Note that the dictionary key needs to be a string otherwise it's way too slow.
    let dictionaryKey = muralToString s
    match memoized.TryGetValue(dictionaryKey) with
    | (true, cost) -> cost
    | (false, _) ->
        let cost =
          match s with
          | [] -> 0
          | [_] -> 0
          | C :: J :: s -> cjCost + recursion (J :: s)
          | J :: C :: s -> jcCost + recursion (C :: s)
          | E :: s -> min (recursion (C :: s)) (recursion (J :: s))
          | C :: E :: s -> min (recursion (C :: C :: s)) (recursion (C :: J :: s))
          | J :: E :: s -> min (recursion (J :: C :: s)) (recursion (J :: J :: s))
          | C :: C :: s -> recursion (C :: s)
          | J :: J :: s -> recursion (J :: s)

        memoized.Add(dictionaryKey, cost)
        cost
  recursion s

let parseMural s =
  List.init (String.length s) (fun i ->
    match s.[i] with
    | 'C' -> C
    | 'J' -> J
    | '?' -> E
    | c -> raise (ArgumentException(sprintf "Unexpected character %A." c))
  )

let t = Console.ReadLine() |> int
for x = 1 to t do
  let tokens = Console.ReadLine().Split(" ")
  let cjCost = int tokens.[0] // x
  let jcCost = int tokens.[1] // y
  let s = parseMural tokens.[2]
  let y = copyrightCost cjCost jcCost s
  printfn "Case #%d: %d" x y
