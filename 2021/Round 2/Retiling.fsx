open System
open System.Collections.Generic
open System.Linq

type DefaultDict<'k, 'v when 'k : equality>(generateDefaultValue : unit -> 'v) =
  let d = Dictionary<'k, 'v>()

  member this.Item
    with get(key : 'k) : 'v =
      match d.TryGetValue(key) with
      | (false, _) ->
          let defaultValue = generateDefaultValue ()
          d.Add(key, defaultValue)
          defaultValue
      | (true, value) -> value

type Color =
  | M // Magenta
  | G // Green

type Vertex =
  | Source
  | Tile of int * int
  | Sink

type Edge = {
  s : Vertex; // Source
  t : Vertex; // Sink

  // Fields used in the Hungarian algorithm.
  cost0 : int;
  mutable cost : int;

  // Fields used in the Ford-Fulkerson algorithm.
  capacity : int;
  mutable flow : int;
}

// Orthogonal distance
let dist (y : int, x : int) (y' : int, x' : int) : int = Math.Abs(y' - y) + Math.Abs(x' - x)

module FordFulkerson =
  type PathElement =
    | Forward of Edge
    | Backward of Edge

  let private getRemainingCapacity : PathElement -> int = function
    | Forward e -> e.capacity - e.flow
    | Backward e -> e.flow

  // Get edges with remaining capacity and zero cost going out of v.
  let private outgoing
    (graph : DefaultDict<Vertex, List<PathElement>>)
    (v : Vertex)
    : seq<PathElement>
    =
    graph.[v].Where(fun element ->
      match element with
      | Forward e
      | Backward e -> e.cost = 0 && getRemainingCapacity element > 0
    )

  let private getSinkVertex : PathElement -> Vertex = function
    | Forward e -> e.t
    | Backward e -> e.s

  // Find a path with remaining capacity.
  let private bfs (graph : DefaultDict<Vertex, List<PathElement>>) : option<PathElement[]> =
    let q = Queue()
    let visited = HashSet()

    visited.Add(Source) |> ignore
    for element in outgoing graph Source do
      q.Enqueue([| element |])
      let v = getSinkVertex element
      visited.Add(v) |> ignore

    let rec loop () =
      if q.Count = 0 then None
      else
        let path = q.Dequeue()
        let v = getSinkVertex (path.Last())

        if v = Sink then Some path
        else
          for element in outgoing graph v do
            let w = getSinkVertex element
            if not (visited.Contains(w)) then
              q.Enqueue(Array.append path [| element |])
              visited.Add(w) |> ignore
          loop ()
    loop ()

  let private augment (path : PathElement[]) : unit =
    let remainingCapacity = path.Min(getRemainingCapacity)
    for element in path do
      match element with
      | Forward edge -> edge.flow <- edge.flow + remainingCapacity
      | Backward edge -> edge.flow <- edge.flow - remainingCapacity

  let buildGraph (edges : List<Edge>) : DefaultDict<Vertex, List<PathElement>> =
    let d = DefaultDict(List)
    for e in edges do
      for (srcVertex, element) in [| (e.s, Forward e); (e.t, Backward e) |] do
        d.[srcVertex].Add(element)
    d

  let maximizeFlow (d : DefaultDict<Vertex, List<PathElement>>) : unit =
    let rec loop () =
      match bfs d with
      | None -> ()
      | Some path ->
          augment path
          loop ()
    loop ()

// https://en.wikipedia.org/wiki/K%C5%91nig%27s_theorem_(graph_theory)#Constructive_proof
let getMinVertexCover
  (maxMatching : HashSet<Edge>)
  (zeroCostMiddleEdges : Edge[])
  (graph : DefaultDict<Vertex, List<Edge>>)
  : Set<Vertex>
  =
  let ms = Set(zeroCostMiddleEdges.Select(fun e -> e.s))
  let gs = Set(zeroCostMiddleEdges.Select(fun e -> e.t))
  let mutable Z = Set.empty

  let stack = Stack()
  let visited = HashSet()
  let push v color =
    stack.Push((v, color))
    visited.Add(v) |> ignore

  let unmatchedMs = HashSet(ms)
  for e in maxMatching do
    let matchedM = e.s
    unmatchedMs.Remove(matchedM) |> ignore
  for unmatchedM in unmatchedMs do
    push unmatchedM M

  while stack.Count > 0 do
    let (v, color) = stack.Pop()
    Z <- Set.add v Z

    match color with
    | M ->
        for e in graph.[v] do
          if e.cost = 0 && not (maxMatching.Contains(e)) then
            let w = e.t
            if not (visited.Contains(w)) then
              push w G
    | G ->
        for e in graph.[v] do
          if e.cost = 0 && maxMatching.Contains(e) then
            let w = e.s
            if not (visited.Contains(w)) then
              push w M

  let minVertexCover = Set.union (Set.difference ms Z) (Set.intersect gs Z)
  if minVertexCover.Count <> maxMatching.Count then
    failwithf "The minimum vertex cover should have the same length as the maximum matching."
  minVertexCover

// https://www.topcoder.com/thrive/articles/Assignment%20Problem%20and%20Hungarian%20Algorithm
let hungarianAlgorithm
  (R : int)
  (C : int)
  (F : int)
  (S : int)
  (current : Color[][])
  (wanted : Color[][])
  : HashSet<Edge>
  =
  let ms = List() // M vertices
  let gs = List() // G vertices
  for y in 0 .. R - 1 do
    for x in 0 .. C - 1 do
      match (current.[y].[x], wanted.[y].[x]) with
      | (M, G) -> ms.Add((y, x))
      | (G, M) -> gs.Add((y, x))
      | _ -> ()

  // Add middle edges
  let edges = List(ms.Count + ms.Count * gs.Count + gs.Count)
  let middleEdges = List(ms.Count * gs.Count)
  for m in ms do
    for g in gs do
      let costOfSwapping = dist m g * S
      let costOfFlipping = 2 * F
      let cost = min costOfSwapping costOfFlipping
      let edge = {
        s = Tile m;
        t = Tile g;
        cost0 = cost;
        cost = cost;
        capacity = 1;
        flow = 0;
      }
      edges.Add(edge)
      middleEdges.Add(edge)

  // Add source edges
  for m in ms do
    edges.Add({
      s = Source;
      t = Tile m;
      cost0 = 0;
      cost = 0;
      capacity = 1;
      flow = 0;
    })
  
  // Add sink edges
  for g in gs do
    edges.Add({
      s = Tile g;
      t = Sink;
      cost0 = 0;
      cost = 0;
      capacity = 1;
      flow = 0;
    })

  let ffGraph = FordFulkerson.buildGraph edges

  let mvcGraph = DefaultDict(List)
  for e in middleEdges do
    for v in [| e.s; e.t |] do
      mvcGraph.[v].Add(e)

  let expectedMaxFlow = min ms.Count gs.Count
  let rec loop () =
    FordFulkerson.maximizeFlow ffGraph
    let maxMatching = middleEdges.Where(fun e -> e.flow > 0).ToHashSet()

    if maxMatching.Count = expectedMaxFlow then maxMatching
    else
      let zeroCostMiddleEdges = middleEdges.Where(fun e -> e.cost = 0).ToArray()
      let V = getMinVertexCover maxMatching zeroCostMiddleEdges mvcGraph

      let delta =
        middleEdges
          .Where(fun e -> not (V.Contains(e.s)) && not (V.Contains(e.t)))
          .Min(fun e -> e.cost)
      
      for e in middleEdges do
        match (V.Contains(e.s), V.Contains(e.t)) with
        | (false, false) -> e.cost <- e.cost - delta
        | (true, true) -> e.cost <- e.cost + delta
        | _ -> ()
      loop ()
  loop ()

let minCostOfRetiling
  (R : int)
  (C : int)
  (F : int)
  (S : int)
  (current : Color[][])
  (wanted : Color[][])
  : int
  =
  let maxMatching = hungarianAlgorithm R C F S current wanted
  let nbLongSwaps = maxMatching.Count
  let costOfLongSwaps = maxMatching.Sum(fun e -> e.cost0)

  let mutable nbDifferences = 0
  for y in 0 .. R - 1 do
    for x in 0 .. C - 1 do
      if current.[y].[x] <> wanted.[y].[x] then
        nbDifferences <- nbDifferences + 1

  let nbFlips = nbDifferences - nbLongSwaps * 2
  costOfLongSwaps + nbFlips * F

let readRows (R : int) (C : int) : Color[][] =
  Array.init R (fun _ ->
    let row =
      Console.ReadLine().Select(function
        | 'M' -> M
        | 'G' -> G
        | c -> failwithf "Unexpected character %A" c
      ).ToArray()

    if row.Length <> C then
      failwithf "Unexpected number of columns %d" row.Length

    row
  )

let () =
  let T = Console.ReadLine() |> int
  for x in 1 .. T do
    let tokens = Console.ReadLine().Split(" ")
    let R = int tokens.[0] // Number of rows
    let C = int tokens.[1] // Number of columns
    let F = int tokens.[2] // Cost of a flip
    let S = int tokens.[3] // Cost of a swap

    let current = readRows R C
    let wanted = readRows R C

    let y = minCostOfRetiling R C F S current wanted
    printfn "Case #%d: %d" x y
