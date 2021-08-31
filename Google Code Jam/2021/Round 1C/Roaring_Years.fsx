open System
open System.Linq

let consRoaringYear (nbConcatenations : int) (starter : int) : bigint =
  String.init nbConcatenations (fun i -> (starter + i).ToString()) |> bigint.Parse

let nextRoaringYearWithFixedConcats (Y : bigint) (nbConcatenations : int) : bigint =
  let mutable lbStarter = 1
  let mutable ubStarter = 1

  // Find a starter that is above Y.
  while consRoaringYear nbConcatenations ubStarter <= Y do
    ubStarter <- ubStarter * 2
  
  // Find the smallest starter above Y using bisection.
  while ubStarter - lbStarter > 1 do
    let middle = (lbStarter + ubStarter) / 2
    if consRoaringYear nbConcatenations middle <= Y then lbStarter <- middle
    else ubStarter <- middle
  
  consRoaringYear nbConcatenations ubStarter

let nextRoaringYear (Y : bigint) : bigint =
  let mutable ubConcatenations = 2
  while consRoaringYear ubConcatenations 1 <= Y do
    ubConcatenations <- ubConcatenations + 1

  { 2 .. ubConcatenations }.Select(nextRoaringYearWithFixedConcats Y).Min()

let () =
  let T = Console.ReadLine() |> int
  for x in 1 .. T do
    let Y = Console.ReadLine() |> bigint.Parse
    let z = nextRoaringYear Y
    printfn "Case #%d: %A" x z
