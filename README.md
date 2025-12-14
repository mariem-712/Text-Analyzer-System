# Text Analyzer System

## Overview
A desktop application that analyzes text documents and provides detailed statistics about writing style, readability, and word usage.

## Purpose
Help writers, students, and professionals understand their text complexity and improve readability by providing instant, comprehensive analysis.

## Technology Stack
- **Backend Logic:** F# (functional programming)
- **User Interface:** C# Windows Forms
- **Framework:** .NET 9.0
- **Testing:** xUnit (35 automated tests)

## Key Features

### Input Options
- Type or paste text directly
- Load text files (`.txt`)
- Paste text from clipboard

### Analysis Provided
- **Word Statistics:** Total words, unique words, longest and shortest words
- **Sentence Analysis:** Total sentences, average sentence length
- **Paragraph Count:** Total number of paragraphs
- **Readability Score:** 0–100 scale with grade-level interpretation
- **Word Frequency:** Top 10 most common words with occurrence counts

## Architecture

### Three-Layer Design
1. **Presentation Layer (C#)**  
   Modern Windows Forms graphical user interface.

2. **Business Logic Layer (F#)**  
   Core text processing, analysis, and calculation logic.

3. **Data Access Layer**  
   File input/output handling and export operations.

## Team Roles 

1. **Input Handling Developer** — *(Mohamed Ayman)*  
   Loads files, manages input text, and validates text encoding.

2. **Tokenization Engineer** — *(Mostafa )*  
   Splits text into sentences and words using pattern matching techniques.

3. **Metrics Calculator** — *(Moaz khaled)*  
   Computes statistical averages and readability formulas.

4. **Frequency Analyzer** — *(Youssef)*  
   Generates ordered frequency tables for words and tokens.

5. **File I/O Developer** — *(Moataz Mamdouh)*  
   Handles JSON export and structured saving of analysis results.

6. **UI Developer** — *(Menna khalifa)*  
   Develops Windows Forms interfaces for text input and results visualization.

7. **Tester** — *(Mariem Sayed)*  
   Validates the accuracy and correctness of computed metrics.

8. **Documenter & GitHub Manager** — *(Mariem&Menna)*  
   Responsible for documentation, diagrams, and managing commits and repositories.
   Team members:
   Mostafa Mahmoud
   Youssef Mohamed
   Mohamed Ayman
