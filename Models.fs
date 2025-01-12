module Models

open System

type TextStatistics = {
    TotalWords: int
    TotalSentences: int
    TotalParagraphs: int
    AverageSentenceLength: float
    AverageWordLength: float
    ReadabilityScore: float
    TopWordFrequencies: (string * int) list
    UniqueWords: int
    LongestWord: string
    ShortestWord: string
}

type AnalysisReport = {
    FileName: string
    AnalysisDate: DateTime
    OriginalText: string
    Statistics: TextStatistics
}