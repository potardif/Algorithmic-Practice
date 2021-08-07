open System
open System.Collections.Generic

let magicPrime = 1_000_000_007L
let maxN = 100_000

let fac =
  let a = Array.init (maxN + 1) (fun _ -> 0L)
  a.[0] <- 1L
  for i in 1 .. a.Length - 1 do
    a.[i] <- a.[i - 1] * int64 i % magicPrime
  a

let inv =
  // https://cp-algorithms.com/algebra/module-inverse.html#mod-inv-all-num
  let a = Array.init (maxN + 1) (fun _ -> 0L)
  a.[1] <- 1L
  for i in 2 .. a.Length - 1 do
    a.[i] <- magicPrime - magicPrime / int64 i * a.[int magicPrime % i] % magicPrime
  a

let invFac =
  let a = Array.init (maxN + 1) (fun _ -> 0L)
  a.[0] <- 1L
  for i in 1 .. a.Length - 1 do
    a.[i] <- a.[i - 1] * inv.[i] % magicPrime
  a

let combination n k = fac.[n] * invFac.[k] % magicPrime * invFac.[n - k] % magicPrime

let nbWaysToCookThePancakes N (V : int[]) =
  let stack = Stack()
  stack.Push((0, N - 1, 1))

  let mutable acc = 1L
  while stack.Count > 0 do
    let (start, end_, one) = stack.Pop()
    let multiplier =
      let length = end_ - start + 1
      if length = 0 then 1L
      else
        let k = // Index of the largest pancake
          let rec loop i =
            if i < start then None
            else if V.[i] = one then Some i
            else loop (i - 1)
          loop end_

        match k with
        | None ->
            stack.Clear() // Impossible case, stop the loop immediately.
            0L
        | Some k ->
            stack.Push((start, k - 1, one)) // Left
            stack.Push((k + 1, end_, one + 1)) // Right
            combination (length - 1) (k - start) // Number of ways to split in two
    acc <- acc * multiplier % magicPrime
  acc

let () =
  let T = Console.ReadLine() |> int
  for x in 1 .. T do
    let N = Console.ReadLine() |> int // Number of pancakes to cook
    let V = Console.ReadLine().Split(" ") |> Array.map int // Visible pancakes at each step
    let y = nbWaysToCookThePancakes N V
    printfn "Case #%d: %d" x y
