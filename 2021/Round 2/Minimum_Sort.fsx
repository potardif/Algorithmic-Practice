open System

let selectionSort N =
  for i in 1 .. N - 1 do
    printfn "M %d %d" i N // Minimum query
    let j = Console.ReadLine() |> int

    if i <> j then
      printfn "S %d %d" i j // Swap operation
      Console.ReadLine() |> ignore

  printfn "D" // Done
  Console.ReadLine() |> ignore

let () =
  let tokens = Console.ReadLine().Split(" ")
  let T = int tokens.[0]
  let N = int tokens.[1]
  for _ in 1 .. T do
    selectionSort N
