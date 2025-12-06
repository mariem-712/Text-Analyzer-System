module StatisticsBuilder

open MetricsCalculator
open FrequencyAnalyzer
open Models

let buildStatistics (paragraphs: string list) (sentences: string list) (words: string list) : TextStatistics =
    let wordFrequencies = FrequencyAnalyzer.calculateWordFrequency words
    let topFrequencies = FrequencyAnalyzer.getTopN 10 wordFrequencies
    {
        TotalWords = words.Length
        TotalSentences = sentences.Length
        TotalParagraphs = paragraphs.Length
        AverageSentenceLength = calculateAverageSentenceLength sentences words
        AverageWordLength = calculateAverageWordLength words
        ReadabilityScore = calculateReadabilityScore sentences words
        TopWordFrequencies = topFrequencies
        UniqueWords = countUniqueWords words
        LongestWord = findLongestWord words
        ShortestWord = findShortestWord words
    }
