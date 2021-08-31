open System
open System.Collections.Generic

let listHoles K P =
  Array.sortInPlace P
  let holes = List()

  let mutable left = 1 // Tentative left part of a hole
  for soldTicket in P do
    if soldTicket > left then
      let right = soldTicket - 1
      holes.Add((left, right))
    left <- soldTicket + 1

  if left <= K then
    holes.Add((left, K))

  holes

let isBorderHole K (left, right) = left = 1 || right = K

let holeSize (left, right) = right - left + 1

let getScore1 K hole =
  if isBorderHole K hole then holeSize hole
  else 1 + (holeSize hole - 1) / 2

let getScore2 K hole1 hole2 =
  if hole1 <> hole2 then getScore1 K hole2
  else
    let hole = hole2
    holeSize hole - getScore1 K hole

let winProbability K P =
  let holes = listHoles K P
  if holes.Count = 0 then 0.
  else
    let hole1 = holes |> Seq.maxBy (getScore1 K)
    let hole2 = holes |> Seq.maxBy (getScore2 K hole1)
    float (getScore1 K hole1 + getScore2 K hole1 hole2) / float K

let () =
  let T = Console.ReadLine() |> int
  for x in 1 .. T do
    let tokens = Console.ReadLine().Split(" ")
    let _N = int tokens.[0] // Number of tickets sold
    let K = int tokens.[1] // Highest possible ticket integer

    let P = Console.ReadLine().Split(" ") |> Array.map int // Sold tickets

    let y = winProbability K P
    printfn "Case #%d: %f" x y
