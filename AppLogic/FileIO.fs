module FileIO

open System
open System.IO
open System.Text.Json
open Models

let exportToJson (report: AnalysisReport) (outputPath: string) =
    try
        let options = JsonSerializerOptions()
        options.WriteIndented <- true
        let json = JsonSerializer.Serialize(report, options)
        File.WriteAllText(outputPath, json)
        Ok outputPath
    with
    | ex -> Error $"Error exporting to JSON: {ex.Message}"

let generateDefaultOutputPath () =
    let folder = "Jsons"
    if not (Directory.Exists(folder)) then
        Directory.CreateDirectory(folder) |> ignore
    let timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss")
    Path.Combine(folder, $"TextAnalysisReport_{timestamp}.json")
