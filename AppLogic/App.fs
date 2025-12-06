module App

open System
open System.IO
open Tokenizer
open StatisticsBuilder
open Display
open InputHandler
open FileIO
open Models

let analyzeText text fileName =
    let (paragraphs, sentences, words) = Tokenizer.tokenize text
    let statistics = StatisticsBuilder.buildStatistics paragraphs sentences words
    {
        FileName = fileName
        AnalysisDate = DateTime.Now
        OriginalText = if text.Length > 500 then text.Substring(0, 500) + "..." else text
        Statistics = statistics
    }

let processText text fileName =
    match InputHandler.validateText text with
    | Error msg ->
        printfn "\nError: %s" msg
        false
    | Ok validText ->
        printfn "\nAnalyzing text...\n"

        let report = analyzeText validText fileName
        Display.printStatistics report.Statistics

        printf "\nDo you want to export the report to JSON? (y/n): "
        let response = Console.ReadLine().ToLower()

        if response = "y" || response = "yes" then
            let outputPath = FileIO.generateDefaultOutputPath()
            match FileIO.exportToJson report outputPath with
            | Ok path -> printfn "\nReport exported successfully to: %s" path
            | Error msg -> printfn "\nExport failed: %s" msg
        true

let rec run () =
    Display.printMenu()    

    match Console.ReadLine() with
    | "1" ->
        printf "\nEnter file path: "
        let filePath = Console.ReadLine()

        match InputHandler.loadFromFile filePath with
        | Ok text -> processText text (Path.GetFileName(filePath)) |> ignore
        | Error msg -> printfn "\nError: %s" msg
        ()

    | "2" ->
        let text = InputHandler.getUserInput()
        processText text "Manual Input" |> ignore
        ()

    | "3" ->
        printfn "\nThank you for using Text Analyzer System!"
        printfn "Goodbye!\n"
        ()

    | _ ->
        printfn "\nInvalid option. Please try again."
        System.Threading.Thread.Sleep(1500)
        run()
