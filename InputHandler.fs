module InputHandler

open System
open System.IO

let loadFromFile (filePath: string) =
    try
        if File.Exists(filePath) then
            let content = File.ReadAllText(filePath)
            if String.IsNullOrWhiteSpace(content) then
                Error "File is empty"
            else
                Ok content
        else
            Error "File not found"
    with
    | ex -> Error $"Error reading file: {ex.Message}"

let validateText (text: string) =
    if String.IsNullOrWhiteSpace(text) then
        Error "Text cannot be empty"
    elif text.Length < 10 then
        Error "Text is too short (minimum 10 characters)"
    else
        Ok text

let getUserInput () =
    printfn "\n=== TEXT INPUT ==="
    printfn "Enter or paste your text (press Enter twice to finish):\n"
    
    let rec readLines acc =
        let line = Console.ReadLine()
        if String.IsNullOrEmpty(line) && acc <> [] then
            acc |> List.rev |> String.concat "\n"
        else
            readLines (line :: acc)
    
    readLines []