open System

let rec reverse (l : int[]) i j =
  if i >= j then ()
  else
    let oldLi = l.[i]
    l.[i] <- l.[j]
    l.[j] <- oldLi

    reverse l (i + 1) (j - 1)

let reverseEngineerList costs =
  let n = Array.length costs + 1
  let list = Array.init n (fun i -> i + 1)
  for i = Array.length costs - 1 downto 0 do
    let j = i + costs.[i] - 1
    reverse list i j
  list

let reverseSortEngineering n c =
  let minCost _i = 1
  let maxCost i = n - i

  let nbCosts = n - 1
  let totalMinCost = Seq.fold (fun acc i -> acc + minCost i) 0 (seq { 0 .. nbCosts - 1 })
  let totalMaxCost = Seq.fold (fun acc i -> acc + maxCost i) 0 (seq { 0 .. nbCosts - 1 })

  if not (totalMinCost <= c && c <= totalMaxCost) then None
  else
    let costs = Array.init nbCosts minCost
    let mutable totalCost = Array.sum costs
    let mutable i = 0
    while totalCost < c do
      if costs.[i] >= maxCost i then
        i <- i + 1
      
      let delta = 1
      costs.[i] <- costs.[i] + delta
      totalCost <- totalCost + delta

    Some (reverseEngineerList costs)

let t = Console.ReadLine() |> int
for x = 1 to t do
  let tokens = Console.ReadLine().Split(" ")
  let n = int tokens.[0]
  let c = int tokens.[1]
  let y =
    match reverseSortEngineering n c with
    | None -> "IMPOSSIBLE"
    | Some y -> String.Join(" ", y)
  printfn "Case #%d: %s" x y
