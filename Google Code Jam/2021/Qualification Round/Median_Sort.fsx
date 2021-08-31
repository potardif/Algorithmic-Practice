open System

type MedianResponse =
  | MedianIsI
  | MedianIsJ
  | MedianIsK

let findMedian i j k =
  printfn "%d %d %d" i j k
  let median = Console.ReadLine() |> int

  if median = -1 then failwithf "Incorrect median query %d %d %d." i j k
  else if median = i then MedianIsI
  else if median = j then MedianIsJ
  else if median = k then MedianIsK
  else failwithf "Unexpected median %d." median

let splitInThree startIndex endIndex =
  let nbPivots = 2
  let n = endIndex - startIndex + 1
  if n < nbPivots then
    failwithf "Cannot split a length of %d in three." n

  let spaceBeforeFirstPivot = (n - nbPivots) / 3
  let pivot1 = startIndex + spaceBeforeFirstPivot

  let spaceAfterFirstPivot = (n - nbPivots - spaceBeforeFirstPivot) / 2
  let pivot2 = pivot1 + 1 + spaceAfterFirstPivot

  (pivot1, pivot2)

let rec ternarySearch (list : int[]) startIndex endIndex unsortedValue =
  let (pivot1, pivot2) = splitInThree startIndex endIndex
  let median = findMedian list.[pivot1] list.[pivot2] unsortedValue
  match median with
  | MedianIsI -> // Before pivot1.
      if startIndex = pivot1 then startIndex
      else
        let newEndIndex = max (pivot1 - 1) (startIndex + 1)
        ternarySearch list startIndex newEndIndex unsortedValue
  | MedianIsK -> // Between pivot1 and pivot2.
      if pivot1 + 1 = pivot2 then pivot2
      else
        let newStartIndex = pivot1 + 1
        let newEndIndex = max (pivot2 - 1) (pivot1 + 2)
        ternarySearch list newStartIndex newEndIndex unsortedValue
  | MedianIsJ -> // After pivot2.
      if pivot2 = endIndex then endIndex + 1
      else
        let newStartIndex = min (pivot2 + 1) (endIndex - 1)
        ternarySearch list newStartIndex endIndex unsortedValue

let insertionSort list =
  // The first two indexes can be ordered either way, so we start at index 2.
  for unsortedIndex = 2 to Array.length list - 1 do
    let unsortedValue = list.[unsortedIndex]
    let sortedIndex = ternarySearch list 0 (unsortedIndex - 1) unsortedValue
    for i = unsortedIndex downto sortedIndex + 1 do
      list.[i] <- list.[i - 1]
    list.[sortedIndex] <- unsortedValue

let submitOrderedList (list : int[]) =
  printfn "%s" (String.Join(" ", list))
  let verdict = Console.ReadLine() |> int
  match verdict with
  | 1 -> () // Right order.
  | -1 -> failwith "Wrong order."
  | _ -> failwithf "Unexpected verdict %d." verdict

let medianSort n =
  let list = Array.init n (fun i -> i + 1)
  insertionSort list
  submitOrderedList list

let () =
  let tokens = Console.ReadLine().Split(" ")
  let t = int tokens.[0] // the number of test cases
  let n = int tokens.[1] // the number of elements to sort within each test case
  let _q = int tokens.[2] // the total number of questions you are allowed across all test cases
  for _ = 1 to t do
    medianSort n
