open System
open System.Linq

let getPadding (a : string) (b : string) =
  if b.Length > a.Length then ""
  else
    let missing = a.Length - b.Length
    match compare a.[0 .. b.Length - 1] b with
    | cmp when cmp < 0 -> String.replicate missing "0"
    | cmp when cmp > 0 -> String.replicate (missing + 1) "0"
    | 0 ->
        let remainder = a.[b.Length .. ]
        if remainder.All(fun c -> c = '9') then String.replicate (missing + 1) "0"
        else
          let remainder' = bigint.Parse remainder + bigint 1
          remainder'.ToString().PadLeft(missing, '0')
    | _ -> failwith "Unreachable"

let appendSort X =
  let mutable n = 0
  for i = 1 to Array.length X - 1 do
    let padding = getPadding X.[i - 1] X.[i]
    n <- n + String.length padding
    X.[i] <- X.[i] + padding
  n

let () =
  let T = Console.ReadLine() |> int
  for x = 1 to T do
    let _N = Console.ReadLine() |> int
    let X = Console.ReadLine().Split(" ")
    let y = appendSort X
    printfn "Case #%d: %d" x y
