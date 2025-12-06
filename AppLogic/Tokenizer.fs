module Tokenizer

open System
open System.Text.RegularExpressions

let private cleanWord (word: string) =
    word.Trim([|' '; ','; '.'; '!'; '?'; ';'; ':'; '"'; '\''; '('; ')'; '['; ']'; '{'; '}'; '-'; '_'|])
        .ToLower()

let splitIntoParagraphs (text: string) =
    text.Split([|"\n\n"; "\r\n\r\n"|], StringSplitOptions.RemoveEmptyEntries)
    |> Array.map (fun p -> p.Trim())
    |> Array.filter (fun p -> not (String.IsNullOrWhiteSpace(p)))
    |> Array.toList

let splitIntoSentences (text: string) =
    let pattern = @"[.!?]+\s+"
    Regex.Split(text, pattern)
    |> Array.map (fun s -> s.Trim())
    |> Array.filter (fun s -> not (String.IsNullOrWhiteSpace(s)))
    |> Array.toList

let splitIntoWords (text: string) =
    let pattern = @"\s+"
    Regex.Split(text, pattern)
    |> Array.map cleanWord
    |> Array.filter (fun w -> not (String.IsNullOrWhiteSpace(w)))
    |> Array.toList

let tokenize (text: string) =
    let paragraphs = splitIntoParagraphs text
    let sentences = splitIntoSentences text
    let words = splitIntoWords text
    (paragraphs, sentences, words)
