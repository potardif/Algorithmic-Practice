open System
open System.Collections.Generic
open System.Linq

let getDiff (n1 : int64) (n2 : int64) : int64 = Math.Abs(n1 - n2)

let addDigit (n : int64) (d : int) : int64 = n * 10L + int64 d

let minMax
  (nb : int[])
  (nbTotalDiv2 : int)
  (nbTotalMod2 : int)
  (n1 : int64)
  (n1Length : int)
  (n2 : int64)
  (n2Length : int)
  : int64 * int64
  =
  let wantedN1Length = nbTotalDiv2 + nbTotalMod2
  let wantedN2Length = nbTotalDiv2

  let mutable n1 = n1
  let mutable n1Length = n1Length
  let mutable d = 0
  while n1Length < wantedN1Length do
    while nb.[d] = 0 do
      d <- d + 1
    n1 <- addDigit n1 d
    n1Length <- n1Length + 1
    nb.[d] <- nb.[d] - 1

  let mutable n2 = n2
  let mutable n2Length = n2Length
  let mutable d = 9
  while n2Length < wantedN2Length do
    while nb.[d] = 0 do
      d <- d - 1
    n2 <- addDigit n2 d
    n2Length <- n2Length + 1
    nb.[d] <- nb.[d] - 1
  
  (n1, n2)

let solveOddCase (nb : int[]) (nbTotalDiv2 : int) (nbTotalMod2 : int) : int64 =
  let lowestD = 1 // A number cannot start with 0.
  { lowestD .. 9 }.Where(fun d -> nb.[d] > 0).Min(fun d1 ->
    let (n1, n1Length, n2, n2Length) = (int64 d1, 1, 0L, 0)

    let nb = Array.copy nb
    nb.[d1] <- nb.[d1] - 1

    let (n1, n2) = minMax nb nbTotalDiv2 nbTotalMod2 n1 n1Length n2 n2Length
    getDiff n1 n2
  )

let solveEvenCase (nb : int[]) (nbTotalDiv2 : int) (nbTotalMod2 : int) : int64 =
  let prefixes = Stack()
  prefixes.Push((0L, 0, Array.copy nb))

  let mutable minDiff = Int64.MaxValue
  while prefixes.Count > 0 do
    let (n, nLength, nb) = prefixes.Pop()

    if nLength = nbTotalDiv2 then
      minDiff <- 0L
      prefixes.Clear() // Stop the loop immediately. It doesn't get better than 0.
    else
      let lowestD = if nLength = 0 then 1 else 0 // A number cannot start with 0.
      
      // The digits in the prefix should be in decreasing order.
      let highestD =
        if nLength = 0 then 9
        else
          let lastDigit = n % 10L |> int
          lastDigit

      // Symmetrical case.
      for d in { lowestD .. highestD }.Where(fun d -> nb.[d] >= 2) do
        let n = addDigit n d
        let nLength = nLength + 1

        let nb = Array.copy nb
        nb.[d] <- nb.[d] - 2

        prefixes.Push((n, nLength, nb))
      
      // Asymmetrical case. We need to ensure that d1 > d2.
      let availableDs = { lowestD .. 9 }.Where(fun d -> nb.[d] > 0).ToArray()
      for d1 in availableDs.Where(fun d -> d > lowestD) do
        for d2 in availableDs.Where(fun d -> d < d1) do
          let (n1, n2) = (addDigit n d1, addDigit n d2)
          let nLength = nLength + 1

          let nb = Array.copy nb
          nb.[d1] <- nb.[d1] - 1
          nb.[d2] <- nb.[d2] - 1

          let (n1, n2) = minMax nb nbTotalDiv2 nbTotalMod2 n1 nLength n2 nLength
          minDiff <- min minDiff (getDiff n1 n2)
  minDiff

let getMinDiff (nb : int[]) : int64 =
  let nbTotal = nb.Sum()
  let (nbTotalDiv2, nbTotalMod2) = Math.DivRem(nbTotal, 2)
  if nbTotalMod2 = 1 then solveOddCase nb nbTotalDiv2 nbTotalMod2
  else solveEvenCase nb nbTotalDiv2 nbTotalMod2

let () =
  let T = Console.ReadLine() |> int
  for x in 1 .. T do
    let nb = Array.init 10 (fun _ -> 0)
    for c in Console.ReadLine() do
      let d = int c - int '0'
      nb.[d] <- nb.[d] + 1
    
    let y = getMinDiff nb
    printfn "Case #%d: %d" x y
