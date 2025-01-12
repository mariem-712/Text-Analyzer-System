module FrequencyAnalyzer

let calculateWordFrequency (words: string list) =
    words
    |> List.filter (fun w -> w.Length > 0)
    |> List.groupBy id
    |> List.map (fun (word, occurrences) -> (word, List.length occurrences))
    |> List.sortByDescending snd

let getTopN n frequencies =
    frequencies |> List.truncate n

let countUniqueWords words =
    words |> List.distinct |> List.length