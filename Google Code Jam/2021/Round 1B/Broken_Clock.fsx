open System
open System.Linq

let oneSecond = 1_000_000_000L
let oneMinute = 60L * oneSecond
let oneHour = 60L * oneMinute
let fullClock = 12L * oneHour

let getAngles ticks =
  let h = ticks % fullClock
  let m = (ticks * 12L) % fullClock
  let s = (ticks * 12L * 60L) % fullClock
  let n = (ticks * 12L * 60L * 60L) % fullClock
  (h, m, s, n)

let getDisplayTime ticks =
  let (h, remainder) = Math.DivRem(ticks, oneHour)
  let (m, remainder) = Math.DivRem(remainder, oneMinute)
  let (s, n) = Math.DivRem(remainder, oneSecond)
  (h, m, s, n)

let normalizeAngles a b c =
  let angles =
    [| a; b; c |]
      .Select(fun correctionFactor ->
        [| a; b; c |]
          .Select(fun x ->
            let x = x - correctionFactor
            if x < 0L then x + fullClock else x
          )
          .OrderBy(fun x -> x)
          .ToArray()
      )
      .Aggregate(fun acc x -> min acc x)
  match angles with
  | [| a; b; c |] -> (a, b, c)
  | _ -> failwith "Unreachable"

let normalizeTicks ticks =
  let (h, m, s, _) = getAngles ticks
  normalizeAngles h m s

let getPermutations a b c =
  [|
    (a, b, c);
    (a, c, b);
    (b, a, c);
    (b, c, a);
    (c, a, b);
    (c, b, a);
  |]

exception Found of int64

let findTimeFromAngles A B C =
  let ABC = normalizeAngles A B C
  try
    for (h, m, s) in getPermutations A B C do
      let d = h - m
      for h in 0L .. 11L do
        let (n, remainder) = Math.DivRem(h * oneHour - d, 11L)
        if remainder = 0L then
          let ticks = h * oneHour + n
          let ticks = if ticks < 0L then ticks + fullClock else ticks
          if normalizeTicks ticks = ABC then
            raise (Found ticks)
    failwith "Not found"
  with Found ticks -> getDisplayTime ticks

let () =
  let T = Console.ReadLine() |> int
  for x in 1 .. T do
    let tokens = Console.ReadLine().Split(" ")
    let A = int64 tokens.[0]
    let B = int64 tokens.[1]
    let C = int64 tokens.[2]
    let (h, m, s, n) = findTimeFromAngles A B C
    printfn "Case #%d: %d %d %d %d" x h m s n
