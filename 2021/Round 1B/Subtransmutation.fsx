open System

let getHeaviestMetal (metals : int[]) =
  let rec loop metal =
    if metal < 0 then None
    else if metals.[metal] > 0 then Some metal
    else loop (metal - 1)
  loop (Array.length metals - 1)

let rec trySolve A B wanted available =
  match getHeaviestMetal wanted with
  | None -> true
  | Some heaviestWantedMetal ->
      match getHeaviestMetal available with
      | None -> false
      | Some heaviestAvailableMetal ->
          for metal = heaviestAvailableMetal downto heaviestWantedMetal + 1 do
            let metal1 = metal - A
            let metal2 = metal - B
            if metal1 >= 0 then available.[metal1] <- available.[metal1] + available.[metal]
            if metal2 >= 0 then available.[metal2] <- available.[metal2] + available.[metal]
            available.[metal] <- 0
          if available.[heaviestWantedMetal] < wanted.[heaviestWantedMetal] then false
          else
            available.[heaviestWantedMetal] <-
              available.[heaviestWantedMetal] - wanted.[heaviestWantedMetal]
            wanted.[heaviestWantedMetal] <- 0
            trySolve A B wanted available

let findStarterMetal A B wanted maxStarterMetal =
  match getHeaviestMetal wanted with
  | None -> failwith "Impossible"
  | Some heaviestWantedMetal ->
      let rec loop starterMetal =
        match maxStarterMetal with
        | Some maxStarterMetal when starterMetal > maxStarterMetal -> None
        | _ ->
            let wanted = Array.copy wanted
            let available =
              Array.init (starterMetal + 1) (fun i -> if i = starterMetal then 1 else 0)
            if trySolve A B wanted available then Some starterMetal
            else loop (starterMetal + 1)
      loop (heaviestWantedMetal + 1)

let maxStarterMetal = findStarterMetal 19 20 (Array.init 20 (fun _ -> 20)) None // 402

let () =
  let T = Console.ReadLine() |> int
  for x in 1 .. T do
    let tokens = Console.ReadLine().Split(" ")
    let _N = int tokens.[0]
    let A = int tokens.[1]
    let B = int tokens.[2]

    let U = Console.ReadLine().Split(" ") |> Array.map int

    let y =
      match findStarterMetal A B U maxStarterMetal with
      | None -> "IMPOSSIBLE"
      | Some y -> (y + 1).ToString()
    printfn "Case #%d: %s" x y
