open System
open System.Collections.Generic
open System.Linq

let solve (N : int) (M : int[][]) : int * int * int =
  let k = { 0 .. N - 1 }.Sum(fun i -> M.[i].[i])
  let r = { 0 .. N - 1 }.Count(fun row ->
    let prevElems = HashSet()
    let rec loop col =
      if col >= N then false
      else if prevElems.Add(M.[row].[col]) then loop (col + 1)
      else true
    loop 0
  )
  let c = { 0 .. N - 1 }.Count(fun col ->
    let prevElems = HashSet()
    let rec loop row =
      if row >= N then false
      else if prevElems.Add(M.[row].[col]) then loop (row + 1)
      else true
    loop 0
  )
  (k, r, c)

let () =
  let T = Console.ReadLine() |> int
  for x in 1 .. T do
    let N = Console.ReadLine() |> int
    let M = Array.init N (fun _ -> Console.ReadLine().Split(" ") |> Array.map int)
    let (k, r, c) = solve N M
    printfn "Case #%d: %d %d %d" x k r c
