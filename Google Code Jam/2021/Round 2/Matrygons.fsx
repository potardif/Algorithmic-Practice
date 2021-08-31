open System
open System.Linq

let rec maxOuterPolygons prevPolygon N =
  if N = 0 then 0
  else
    let nextPolygons = { prevPolygon * 2 .. prevPolygon .. N }.Where(fun i -> N % i = 0).ToArray()
    if nextPolygons.Length = 0 then 0
    else 1 + nextPolygons.Select(fun next -> maxOuterPolygons next (N - next)).Max()

let rec maxNbPolygons N =
  let starterPolygons = { 3 .. N }.Where(fun i -> N % i = 0)
  1 + starterPolygons.Select(fun starter -> maxOuterPolygons starter (N - starter)).Max()

let () =
  let T = Console.ReadLine() |> int
  for x in 1 .. T do
    let N = Console.ReadLine() |> int
    let y = maxNbPolygons N
    printfn "Case #%d: %d" x y
