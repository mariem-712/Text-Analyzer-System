module Display

open System
open Models

let interpretReadability (score: float) =
    match score with
    | s when s >= 90.0 -> "Very Easy (5th grade)"
    | s when s >= 80.0 -> "Easy (6th grade)"
    | s when s >= 70.0 -> "Fairly Easy (7th grade)"
    | s when s >= 60.0 -> "Standard (8th-9th grade)"
    | s when s >= 50.0 -> "Fairly Difficult (10th-12th grade)"
    | s when s >= 30.0 -> "Difficult (College level)"
    | _ -> "Very Difficult (Professional)"



let printStatistics (stats: TextStatistics) =
    printfn "\n╔════════════════════════════════════════════════════════════════╗"
    printfn "║                      ANALYSIS RESULTS                          ║"
    printfn "╚════════════════════════════════════════════════════════════════╝\n"
    
    printfn " BASIC COUNTS:"
    printfn "   • Total Words:      %d" stats.TotalWords
    printfn "   • Total Sentences:  %d" stats.TotalSentences
    printfn "   • Total Paragraphs: %d" stats.TotalParagraphs
    printfn "   • Unique Words:     %d" stats.UniqueWords
    
    printfn "\n AVERAGES:"
    printfn "   • Average Sentence Length: %.2f words" stats.AverageSentenceLength
    printfn "   • Average Word Length:     %.2f characters" stats.AverageWordLength
    
    printfn "\n READABILITY:"
    printfn "   • Readability Score: %.2f/100" stats.ReadabilityScore
    printfn "   • Interpretation: %s" (interpretReadability stats.ReadabilityScore)
    
    printfn "\n WORD EXTREMES:"
    printfn "   • Longest Word:  %s (%d chars)" stats.LongestWord stats.LongestWord.Length
    printfn "   • Shortest Word: %s (%d chars)" stats.ShortestWord stats.ShortestWord.Length
    
    printfn "\n TOP 10 MOST FREQUENT WORDS:"
    stats.TopWordFrequencies
    |> List.iteri (fun i (word, count) ->
        printfn "   %2d. %-15s → %d occurrences" (i + 1) word count
    )
    
    printfn "\n╚════════════════════════════════════════════════════════════════╝"

let printMenu () =
    printfn "\n╔════════════════════════════════════════════════════════════════╗"
    printfn "║                         MAIN MENU                               ║"
    printfn "╚════════════════════════════════════════════════════════════════╝"
    printfn "  1. Load text from file"
    printfn "  2. Enter text manually"
    printfn "  3. Exit"
    printf "\nSelect an option (1-3): "
