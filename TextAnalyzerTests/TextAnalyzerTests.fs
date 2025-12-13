module TextAnalyzerTests

open Xunit
open Tokenizer
open InputHandler
open MetricsCalculator
open FrequencyAnalyzer
open StatisticsBuilder
open Models


// INPUT HANDLER TESTS

[<Fact>]
let ``validateText should reject empty text`` () =
    let result = InputHandler.validateText ""
    match result with
    | Error _ -> Assert.True(true)
    | Ok _ -> Assert.True(false, "Expected Error but got Ok")

[<Fact>]
let ``validateText should reject whitespace-only text`` () =
    let result = InputHandler.validateText "   \n\t  "
    match result with
    | Error _ -> Assert.True(true)
    | Ok _ -> Assert.True(false, "Expected Error but got Ok")

[<Fact>]
let ``validateText should accept valid text`` () =
    let result = InputHandler.validateText "Hello world"
    match result with
    | Ok text -> Assert.Equal("Hello world", text)
    | Error _ -> Assert.True(false, "Expected Ok but got Error")

[<Fact>]
let ``validateText should accept words whitespace`` () =
    let result = InputHandler.validateText "  Hello world  "
    match result with
    | Ok text -> 
        Assert.NotEmpty(text)
    | Error _ -> Assert.True(false, "Expected Ok but got Error")


// TOKENIZER TESTS
[<Fact>]
let ``tokenize should split text into paragraphs`` () =
    let text = "First paragraph.\n\nSecond paragraph."
    let (paragraphs, _, _) = Tokenizer.tokenize text
    Assert.Equal(2, paragraphs |> List.length)

[<Fact>]
let ``tokenize should handle text with multiple line breaks`` () =
    let text = "Paragraph 1\n\n\n\nParagraph 2"
    let (paragraphs, _, _) = Tokenizer.tokenize text
    Assert.Equal(2, paragraphs |> List.length)

[<Fact>]
let ``tokenize should split text into sentences`` () =
    let text = "First sentence. Second sentence! Third sentence?"
    let (_, sentences, _) = Tokenizer.tokenize text
    Assert.Equal(3, sentences |> List.length)

[<Fact>]
let ``tokenize should split text into words`` () =
    let text = "Hello world test"
    let (_, _, words) = Tokenizer.tokenize text
    Assert.Equal(3, words |> List.length)

[<Fact>]
let ``tokenize should convert words to lowercase`` () =
    let text = "HELLO World"
    let (_, _, words) = Tokenizer.tokenize text
    Assert.Contains("hello", words)
    Assert.Contains("world", words)

[<Fact>]
let ``tokenize should remove punctuation from words`` () =
    let text = "hello, world!"
    let (_, _, words) = Tokenizer.tokenize text
    Assert.DoesNotContain(",", words)
    Assert.DoesNotContain("!", words)

[<Fact>]
let ``tokenize should handle text with mixed punctuation`` () =
    let text = "Hello... World!!! How are you???"
    let (_, sentences, _) = Tokenizer.tokenize text
    Assert.True(sentences |> List.length > 0)


// METRICS CALCULATOR TESTS
[<Fact>]
let ``calculateAverageWordLength should return correct average`` () =
    let words = ["cat"; "dog"; "bird"]
    let avg = MetricsCalculator.calculateAverageWordLength words
    Assert.True(avg > 3.0 && avg < 3.5)

[<Fact>]
let ``calculateAverageSentenceLength should return correct average`` () =
    let sentences = ["This is a test"; "Another sentence here"; "Short"]
    let words = ["this"; "is"; "a"; "test"; "another"; "sentence"; "here"; "short"]
    let avg = MetricsCalculator.calculateAverageSentenceLength sentences words
    Assert.True(avg > 2.0 && avg < 3.0)

[<Fact>]
let ``findLongestWord should return the longest word`` () =
    let words = ["cat"; "elephant"; "dog"]
    let longest = MetricsCalculator.findLongestWord words
    Assert.Equal("elephant", longest)

[<Fact>]
let ``findShortestWord should return the shortest word`` () =
    let words = ["cat"; "elephant"; "a"]
    let shortest = MetricsCalculator.findShortestWord words
    Assert.Equal("a", shortest)

[<Fact>]
let ``calculateReadabilityScore should return score between 0 and 100`` () =
    let words = ["the"; "quick"; "brown"; "fox"; "jumps"]
    let sentences = ["The quick brown fox jumps"]
    let score = MetricsCalculator.calculateReadabilityScore words sentences
    Assert.True(score >= 0.0 && score <= 100.0)

[<Fact>]
let ``calculateReadabilityScore should handle very short text`` () =
    let words = ["hi"]
    let sentences = ["Hi"]
    let score = MetricsCalculator.calculateReadabilityScore words sentences
    Assert.True(score >= 0.0)


// FREQUENCY ANALYZER TESTS
[<Fact>]
let ``countUniqueWords should return correct count`` () =
    let words = ["cat"; "dog"; "cat"; "bird"]
    let unique = FrequencyAnalyzer.countUniqueWords words
    Assert.Equal(3, unique)

[<Fact>]
let ``getTopN should return top words`` () =
    let words = ["a"; "a"; "a"; "b"; "b"; "c"]
    let topWords = FrequencyAnalyzer.getTopN 2 words
    Assert.Equal(2, topWords |> List.length)


// STATISTICS BUILDER TESTS

[<Fact>]
let ``buildStatistics should create complete statistics`` () =
    let text = "Hello world. This is a test."
    let (paragraphs, sentences, words) = Tokenizer.tokenize text
    let stats = StatisticsBuilder.buildStatistics paragraphs sentences words
    
    Assert.True(stats.TotalWords > 0)
    Assert.True(stats.TotalSentences > 0)
    Assert.True(stats.TotalParagraphs > 0)
    Assert.True(stats.UniqueWords > 0)
    Assert.True(stats.AverageSentenceLength > 0.0)
    Assert.True(stats.AverageWordLength > 0.0)
    Assert.True(stats.ReadabilityScore >= 0.0)
    Assert.False(System.String.IsNullOrEmpty(stats.LongestWord))
    Assert.False(System.String.IsNullOrEmpty(stats.ShortestWord))

[<Fact>]
let ``buildStatistics should handle single word`` () =
    let text = "Hello"
    let (paragraphs, sentences, words) = Tokenizer.tokenize text
    let stats = StatisticsBuilder.buildStatistics paragraphs sentences words
    
    Assert.Equal(1, stats.TotalWords)
    Assert.Equal(1, stats.UniqueWords)
    Assert.Equal("hello", stats.LongestWord)
    Assert.Equal("hello", stats.ShortestWord)

[<Fact>]
let ``buildStatistics should handle text with numbers`` () =
    let text = "There are 123 items and 456 more."
    let (paragraphs, sentences, words) = Tokenizer.tokenize text
    let stats = StatisticsBuilder.buildStatistics paragraphs sentences words
    Assert.True(stats.TotalWords > 0)



// INTEGRATION TESTS

[<Fact>]
let ``Full pipeline should process sample text correctly`` () =
    let text = "The quick brown fox jumps over the lazy dog. This is a test sentence."
    
    match InputHandler.validateText text with
    | Ok validText ->
        let (paragraphs, sentences, words) = Tokenizer.tokenize validText
        let stats = StatisticsBuilder.buildStatistics paragraphs sentences words
        
        Assert.Equal(14, stats.TotalWords)
        Assert.Equal(2, stats.TotalSentences)
        Assert.Equal(1, stats.TotalParagraphs)
        Assert.True(stats.TopWordFrequencies |> List.length <= 10)
    | Error msg -> 
        Assert.True(false, $"Validation failed: {msg}")

[<Fact>]
let ``Should handle paragraph with multiple sentences`` () =
    let text = "First sentence. Second sentence. Third sentence."
    let (paragraphs, sentences, words) = Tokenizer.tokenize text
    
    Assert.Equal(1, paragraphs |> List.length)
    Assert.Equal(3, sentences |> List.length)
    Assert.True(words |> List.length > 0)


