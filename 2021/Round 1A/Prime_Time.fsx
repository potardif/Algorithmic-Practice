open System
open System.Collections.Generic
open System.Linq

let pow x y =
  let rec loop y acc =
    if y <= 0L then acc
    else loop (y - 1L) (acc * x)
  loop y 1L

let getOrDefault (dictionary : Dictionary<'a, 'b>) key defaultValue =
  match dictionary.TryGetValue(key) with
  | (false, _) -> defaultValue
  | (true, value) -> value

let minPrime = 2L
let maxPrime = 499L

let primes =
  let sieve = Array.init (int maxPrime + 1) (fun _i -> true)
  for i in minPrime .. Math.Sqrt(float maxPrime) |> int64 do
    if sieve.[int i] then
      let mutable j = i * i
      while j <= maxPrime do
        sieve.[int j] <- false
        j <- j + i
  { minPrime .. maxPrime }.Where(fun i -> sieve.[int i]).ToArray()

let factorize n =
  let factors = Dictionary()
  let rec loop n i =
    if n = 1L then Some factors
    else if i >= Array.length primes then None
    else
      let prime = primes.[i]
      let (quotient, remainder) = Math.DivRem(n, prime)
      if remainder = 0L then
        match factors.TryGetValue(prime) with
        | (false, _) -> factors.Add(prime, 1L)
        | (true, n) -> factors.[prime] <- n + 1L
        loop quotient 0
      else loop n (i + 1)
  loop n 0

let maxNbCardsInSecondGroup =
  let maxNbCards = pow 10L 15L
  let rec loop n =
    let sumFirstGroup = maxPrime * (maxNbCards - n)
    let productSecondGroup = pow minPrime n
    if productSecondGroup >= sumFirstGroup then n
    else loop (n + 1L)
  loop 1L

let getMaxScore (cards : Dictionary<int64, int64>) =
  let mutable maxScore = 0L
  let sumAllCards = cards.Sum(fun (KeyValue (prime, n)) -> prime * n)
  let minCandidateScore =
    let maxSumSecondGroup = maxPrime * maxNbCardsInSecondGroup
    max minPrime (sumAllCards - maxSumSecondGroup)
  let maxCandidateScore = sumAllCards
  for candidateScore in minCandidateScore .. maxCandidateScore do
    match factorize candidateScore with
    | None -> ()
    | Some factors ->
        let sumFirstGroup =
          let rec loop i acc =
            if i >= primes.Length then Some acc
            else
              let prime = primes.[i]
              let n = getOrDefault cards prime 0L - getOrDefault factors prime 0L
              if n < 0L then None
              else loop (i + 1) (acc + prime * n)
          loop 0 0L
        match sumFirstGroup with
        | None -> ()
        | Some sumFirstGroup ->
            let productSecondGroup =
              factors.Aggregate(1L, fun acc (KeyValue (prime, n)) -> acc * pow prime n)
            if sumFirstGroup = productSecondGroup then
              let score = sumFirstGroup
              if score > maxScore then
                maxScore <- score
  maxScore

let () =
  let T = Console.ReadLine() |> int
  for x in 1 .. T do
    let M = Console.ReadLine() |> int

    let cards = Dictionary()
    for _ in 1 .. M do
      let tokens = Console.ReadLine().Split(" ")
      let P = int64 tokens.[0]
      let N = int64 tokens.[1]
      cards.Add(P, N)

    let y = getMaxScore cards
    printfn "Case #%d: %d" x y
