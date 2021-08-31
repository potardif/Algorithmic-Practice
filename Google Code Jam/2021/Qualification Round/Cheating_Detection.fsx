open System
open System.Linq

let nbPlayers = 100
let nbQuestions = 10_000

let f skill difficulty =
  let x = skill - difficulty
  1. / (1. + Math.Exp(-x))

let detectCheater (lines : bool[][]) =
  let players = { 0 .. nbPlayers - 1 }
  let questions = { 0 .. nbQuestions - 1 }

  let getSkill = Array.init nbPlayers (fun i ->
    let nbCorrectAnswers = lines.[i].Count(fun correct -> correct)
    let p = float nbCorrectAnswers / float nbQuestions
    -Math.Log(1. / p - 1.)
  )

  let getDifficulty = Array.init nbQuestions (fun j ->
    let nbCorrectAnswers = players.Count(fun i -> lines.[i].[j])
    let p = float nbCorrectAnswers / float nbPlayers
    Math.Log(1. / p - 1.)
  )

  let sampleSize = nbQuestions * 5 / 100
  let hardestQuestions =
    questions.OrderBy(fun j -> getDifficulty.[j]).TakeLast(sampleSize).ToArray()

  players |> Seq.maxBy (fun i ->
    let skill = getSkill.[i]
    let score = hardestQuestions.Count(fun j -> lines.[i].[j])
    let expectedScore = hardestQuestions.Sum(fun j -> f skill getDifficulty.[j])
    float score / expectedScore
  )

let () =
  let t = Console.ReadLine() |> int
  let _p = Console.ReadLine() |> int
  for x = 1 to t do
    let lines = Array.init nbPlayers (fun _i ->
      let line = Console.ReadLine()

      let lineLength = String.length line
      if lineLength <> nbQuestions then
        failwithf "Unexpected line length %d." lineLength

      Array.init nbQuestions (fun j ->
        match line.[j] with
        | '0' -> false
        | '1' -> true
        | c -> failwithf "Unexpected character %A." c
      )
    )
    let y = detectCheater lines + 1
    printfn "Case #%d: %d" x y
