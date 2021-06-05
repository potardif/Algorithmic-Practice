open System

let rec reverse (l : int[]) i j =
  if i >= j then ()
  else
    let old_li = l.[i]
    l.[i] <- l.[j]
    l.[j] <- old_li

    reverse l (i + 1) (j - 1)

let reverseSort l =
  let n = Array.length l

  let mutable cost = 0
  for i in 0 .. n - 2 do
    let mutable j = i
    for k in i + 1 .. n - 1 do
      if l.[k] < l.[j] then
        j <- k

    reverse l i j
    cost <- cost + j - i + 1
  cost

let t = Console.ReadLine() |> int
for x in 1 .. t do
  let _n = Console.ReadLine() |> int
  let l = Console.ReadLine().Split(" ") |> Array.map int
  let y = reverseSort l
  printfn "Case #%d: %d" x y
