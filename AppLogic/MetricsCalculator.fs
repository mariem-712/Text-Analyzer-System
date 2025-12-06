module MetricsCalculator

open System

let calculateAverageSentenceLength (sentences: string list) (words: string list) =
    if sentences.IsEmpty then 0.0
    else float words.Length / float sentences.Length

let calculateAverageWordLength (words: string list) =
    if words.IsEmpty 
        then 0.0
    else
        words |> List.map (fun w -> float w.Length) |> List.average

let calculateReadabilityScore (sentences: string list) (words: string list) =
    if sentences.IsEmpty || words.IsEmpty then 0.0
    else
        let avgSentenceLength = float words.Length / float sentences.Length
        let avgWordLength = words |> List.map (fun w -> float w.Length) |> List.average
        let score = 206.835 - (1.015 * avgSentenceLength) - (84.6 * avgWordLength / 5.0)
        Math.Round(Math.Max(0.0, Math.Min(100.0, score)), 2)

let findLongestWord (words: string list) =
    if words.IsEmpty then "" else words |> List.maxBy (fun w -> w.Length)

let findShortestWord (words: string list) =
    if words.IsEmpty then "" else words |> List.minBy (fun w -> w.Length)
