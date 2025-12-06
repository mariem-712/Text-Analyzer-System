using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Microsoft.FSharp.Core;

namespace TextAnalyzerWinForms
{
    public class MainForm : Form
    {
        private Models.AnalysisReport? currentReport = null;
        
        // UI Controls
        private Panel headerPanel = null!;
        private Label titleLabel = null!;
        private Label subtitleLabel = null!;
        private Panel contentPanel = null!;
        private Panel leftPanel = null!;
        private Panel rightPanel = null!;
        private Panel footerPanel = null!;
        
        // Left Panel Controls
        private GroupBox inputGroupBox = null!;
        private TextBox inputTextBox = null!;
        private Panel inputControlsPanel = null!;
        private Button loadFileButton = null!;
        private Button pasteButton = null!;
        private Label fileNameLabel = null!;
        private Panel actionButtonsPanel = null!;
        private Button analyzeButton = null!;
        private Button clearButton = null!;
        private Label wordCountLabel = null!;
        
        // Right Panel Controls
        private GroupBox resultsGroupBox = null!;
        private Panel statsPanel = null!;
        private Button exportButton = null!;
        private Label statusLabel = null!;
        
        public MainForm()
        {
            InitializeComponents();
            SetupLayout();
            WireUpEvents();
            DisplayWelcomeMessage();
        }
        
        private void InitializeComponents()
        {
            // Configure Form
            this.Text = "Text Analyzer Pro";
            this.Size = new Size(1400, 850);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(1200, 700);
            this.BackColor = Color.FromArgb(240, 242, 245);
            this.Font = new Font("Segoe UI", 9f);
            
            // Header Panel
            headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.White,
                Padding = new Padding(30, 20, 30, 10)
            };
            
            titleLabel = new Label
            {
                Text = "Text Analyzer",
                Font = new Font("Segoe UI", 24f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                AutoSize = true,
                Location = new Point(30, 15)
            };
            
            subtitleLabel = new Label
            {
                Text = "Analyze your text and get detailed statistics",
                Font = new Font("Segoe UI", 10f),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = true,
                Location = new Point(30, 50)
            };
            
            // Content Panel
            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };
            
            // Left Panel
            leftPanel = new Panel
            {
                Width = 600,
                Dock = DockStyle.Left,
                Padding = new Padding(0, 0, 10, 0)
            };
            
            inputGroupBox = new GroupBox
            {
                Text = "  Input Text  ",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Padding = new Padding(15),
                BackColor = Color.White
            };
            
            inputTextBox = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Consolas", 10f),
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(248, 250, 252),
                Padding = new Padding(10)
            };
            
            inputControlsPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                Padding = new Padding(5)
            };
            
            loadFileButton = new Button
            {
                Text = "📁 Load File",
                Width = 120,
                Height = 38,
                Left = 5,
                Top = 6,
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            loadFileButton.FlatAppearance.BorderSize = 0;
            
            pasteButton = new Button
            {
                Text = "📋 Paste",
                Width = 100,
                Height = 38,
                Left = 135,
                Top = 6,
                BackColor = Color.FromArgb(139, 92, 246),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            pasteButton.FlatAppearance.BorderSize = 0;
            
            fileNameLabel = new Label
            {
                Text = "No file loaded",
                Left = 245,
                Top = 15,
                AutoSize = true,
                ForeColor = Color.FromArgb(100, 116, 139),
                Font = new Font("Segoe UI", 9f, FontStyle.Italic)
            };
            
            actionButtonsPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 70,
                Padding = new Padding(5)
            };
            
            wordCountLabel = new Label
            {
                Text = "Words: 0",
                Left = 10,
                Top = 10,
                AutoSize = true,
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(100, 116, 139)
            };
            
            analyzeButton = new Button
            {
                Text = "✓ Analyze Text",
                Width = 150,
                Height = 45,
                Left = 5,
                Top = 35,
                BackColor = Color.FromArgb(34, 197, 94),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            analyzeButton.FlatAppearance.BorderSize = 0;
            
            clearButton = new Button
            {
                Text = "✕ Clear",
                Width = 100,
                Height = 45,
                Left = 165,
                Top = 35,
                BackColor = Color.FromArgb(239, 68, 68),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            clearButton.FlatAppearance.BorderSize = 0;
            
            // Right Panel
            rightPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10, 0, 0, 0)
            };
            
            resultsGroupBox = new GroupBox
            {
                Text = "  Analysis Results  ",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Padding = new Padding(15),
                BackColor = Color.White
            };
            
            statsPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.White
            };
            
            // Footer Panel
            footerPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 70,
                BackColor = Color.White,
                Padding = new Padding(30, 15, 30, 15)
            };
            
            statusLabel = new Label
            {
                Text = "Ready to analyze text...",
                AutoSize = true,
                Left = 30,
                Top = 25,
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = Color.FromArgb(100, 116, 139)
            };
            
            exportButton = new Button
            {
                Text = "💾 Export Results ▼",
                Width = 180,
                Height = 42,
                Top = 14,
                BackColor = Color.FromArgb(168, 85, 247),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Enabled = false
            };
            exportButton.FlatAppearance.BorderSize = 0;
        }
        
        private void SetupLayout()
        {
            // Header
            headerPanel.Controls.AddRange(new Control[] { titleLabel, subtitleLabel });
            
            // Left Panel
            actionButtonsPanel.Controls.AddRange(new Control[] { wordCountLabel, analyzeButton, clearButton });
            inputControlsPanel.Controls.AddRange(new Control[] { loadFileButton, pasteButton, fileNameLabel });
            inputGroupBox.Controls.Add(inputTextBox);
            inputGroupBox.Controls.Add(actionButtonsPanel);
            inputGroupBox.Controls.Add(inputControlsPanel);
            leftPanel.Controls.Add(inputGroupBox);
            
            // Right Panel
            resultsGroupBox.Controls.Add(statsPanel);
            rightPanel.Controls.Add(resultsGroupBox);
            
            // Content Panel
            contentPanel.Controls.Add(rightPanel);
            contentPanel.Controls.Add(leftPanel);
            
            // Footer - Position export button on the right
            exportButton.Left = footerPanel.ClientSize.Width - exportButton.Width - 30;
            footerPanel.Controls.AddRange(new Control[] { statusLabel, exportButton });
            
            // Form
            this.Controls.Add(contentPanel);
            this.Controls.Add(footerPanel);
            this.Controls.Add(headerPanel);
        }
        
        private void WireUpEvents()
        {
            loadFileButton.Click += (s, e) => LoadFile();
            pasteButton.Click += (s, e) => PasteFromClipboard();
            analyzeButton.Click += (s, e) => AnalyzeText();
            clearButton.Click += (s, e) => ClearAll();
            exportButton.Click += (s, e) => ShowExportMenu();
            inputTextBox.TextChanged += (s, e) => UpdateWordCount();
            
            // Keep export button aligned to the right when resizing
            this.Resize += (s, e) => 
            {
                if (footerPanel != null && exportButton != null)
                {
                    exportButton.Left = footerPanel.ClientSize.Width - exportButton.Width - 30;
                }
            };
        }
        
        private void UpdateWordCount()
        {
            if (!string.IsNullOrWhiteSpace(inputTextBox.Text))
            {
                int wordCount = inputTextBox.Text.Split(new[] { ' ', '\n', '\r', '\t' }, 
                    StringSplitOptions.RemoveEmptyEntries).Length;
                wordCountLabel.Text = $"Words: {wordCount:N0}";
            }
            else
            {
                wordCountLabel.Text = "Words: 0";
            }
        }
        
        private void PasteFromClipboard()
        {
            if (Clipboard.ContainsText())
            {
                inputTextBox.Text = Clipboard.GetText();
                fileNameLabel.Text = "Text pasted from clipboard";
                statusLabel.Text = "Text pasted successfully";
            }
            else
            {
                MessageBox.Show("No text found in clipboard", "Paste", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        private void LoadFile()
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                dialog.Title = "Select a text file";
                
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string content = File.ReadAllText(dialog.FileName);
                        inputTextBox.Text = content;
                        fileNameLabel.Text = Path.GetFileName(dialog.FileName);
                        statusLabel.Text = $"Loaded: {Path.GetFileName(dialog.FileName)}";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading file: {ex.Message}", "Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        statusLabel.Text = "Error loading file";
                    }
                }
            }
        }
        
        private void AnalyzeText()
        {
            string text = inputTextBox.Text;
            
            var validationResult = InputHandler.validateText(text);
            
            if (validationResult.IsError)
            {
                string error = validationResult.ErrorValue;
                MessageBox.Show(error, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                statusLabel.Text = "Validation failed";
                return;
            }
            
            try
            {
                statusLabel.Text = "Analyzing text...";
                Application.DoEvents();
                
                string fileName = fileNameLabel.Text == "No file loaded" ? "Manual Input" : fileNameLabel.Text;
                string validText = validationResult.ResultValue;
                
                var tokenizeResult = Tokenizer.tokenize(validText);
                var paragraphs = tokenizeResult.Item1;
                var sentences = tokenizeResult.Item2;
                var words = tokenizeResult.Item3;
                
                var statistics = StatisticsBuilder.buildStatistics(paragraphs, sentences, words);
                
                string originalText = validText.Length > 500 
                    ? validText.Substring(0, 500) + "..." 
                    : validText;
                
                var report = new Models.AnalysisReport(
                    fileName,
                    DateTime.Now,
                    originalText,
                    statistics
                );
                
                currentReport = report;
                DisplayResults(statistics);
                exportButton.Enabled = true;
                statusLabel.Text = "✓ Analysis complete!";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during analysis: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusLabel.Text = "Analysis failed";
            }
        }
        
        private void DisplayResults(Models.TextStatistics stats)
        {
            statsPanel.Controls.Clear();
            int yPos = 10;
            
            // Create stat cards
            AddStatCard(statsPanel, "Total Words", stats.TotalWords.ToString("N0"), 
                Color.FromArgb(59, 130, 246), ref yPos);
            AddStatCard(statsPanel, "Total Sentences", stats.TotalSentences.ToString("N0"), 
                Color.FromArgb(139, 92, 246), ref yPos);
            AddStatCard(statsPanel, "Total Paragraphs", stats.TotalParagraphs.ToString("N0"), 
                Color.FromArgb(236, 72, 153), ref yPos);
            AddStatCard(statsPanel, "Unique Words", stats.UniqueWords.ToString("N0"), 
                Color.FromArgb(34, 197, 94), ref yPos);
            
            yPos += 10;
            AddSectionHeader(statsPanel, "Averages", ref yPos);
            AddStatRow(statsPanel, "Sentence Length", $"{stats.AverageSentenceLength:F2} words", ref yPos);
            AddStatRow(statsPanel, "Word Length", $"{stats.AverageWordLength:F2} characters", ref yPos);
            
            yPos += 10;
            AddSectionHeader(statsPanel, "Readability", ref yPos);
            AddStatRow(statsPanel, "Score", $"{stats.ReadabilityScore:F2}/100", ref yPos);
            AddStatRow(statsPanel, "Level", Display.interpretReadability(stats.ReadabilityScore), ref yPos);
            
            yPos += 10;
            AddSectionHeader(statsPanel, "Word Extremes", ref yPos);
            AddStatRow(statsPanel, "Longest", $"{stats.LongestWord} ({stats.LongestWord.Length} chars)", ref yPos);
            AddStatRow(statsPanel, "Shortest", $"{stats.ShortestWord} ({stats.ShortestWord.Length} chars)", ref yPos);
            
            yPos += 10;
            AddSectionHeader(statsPanel, "Top 10 Most Frequent Words", ref yPos);
            AddFrequencyTable(statsPanel, stats.TopWordFrequencies, ref yPos);
        }
        
        private void AddStatCard(Panel parent, string label, string value, Color color, ref int yPos)
        {
            Panel card = new Panel
            {
                Width = parent.Width - 30,
                Height = 70,
                Left = 10,
                Top = yPos,
                BackColor = Color.FromArgb(248, 250, 252)
            };
            
            Panel colorBar = new Panel
            {
                Width = 4,
                Height = 70,
                Left = 0,
                Top = 0,
                BackColor = color
            };
            
            Label lblLabel = new Label
            {
                Text = label,
                Left = 15,
                Top = 15,
                AutoSize = true,
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(100, 116, 139)
            };
            
            Label lblValue = new Label
            {
                Text = value,
                Left = 15,
                Top = 35,
                AutoSize = true,
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59)
            };
            
            card.Controls.AddRange(new Control[] { colorBar, lblLabel, lblValue });
            parent.Controls.Add(card);
            yPos += 80;
        }
        
        private void AddSectionHeader(Panel parent, string text, ref int yPos)
        {
            Label header = new Label
            {
                Text = text,
                Left = 10,
                Top = yPos,
                AutoSize = true,
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59)
            };
            parent.Controls.Add(header);
            yPos += 35;
        }
        
        private void AddStatRow(Panel parent, string label, string value, ref int yPos)
        {
            Panel row = new Panel
            {
                Width = parent.Width - 30,
                Height = 35,
                Left = 10,
                Top = yPos,
                BackColor = Color.FromArgb(248, 250, 252)
            };
            
            Label lblLabel = new Label
            {
                Text = label,
                Left = 15,
                Top = 8,
                AutoSize = true,
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = Color.FromArgb(71, 85, 105)
            };
            
            Label lblValue = new Label
            {
                Text = value,
                Top = 8,
                AutoSize = true,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Anchor = AnchorStyles.Right | AnchorStyles.Top
            };
            lblValue.Left = row.Width - lblValue.Width - 15;
            
            row.Controls.AddRange(new Control[] { lblLabel, lblValue });
            parent.Controls.Add(row);
            yPos += 40;
        }
        
        private void AddFrequencyTable(Panel parent, Microsoft.FSharp.Collections.FSharpList<Tuple<string, int>> frequencies, ref int yPos)
        {
            int index = 1;
            foreach (var item in frequencies)
            {
                Panel row = new Panel
                {
                    Width = parent.Width - 30,
                    Height = 35,
                    Left = 10,
                    Top = yPos,
                    BackColor = index % 2 == 0 ? Color.White : Color.FromArgb(248, 250, 252)
                };
                
                Label lblRank = new Label
                {
                    Text = $"{index}.",
                    Left = 15,
                    Top = 8,
                    Width = 30,
                    Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                    ForeColor = Color.FromArgb(100, 116, 139)
                };
                
                Label lblWord = new Label
                {
                    Text = item.Item1,
                    Left = 50,
                    Top = 8,
                    AutoSize = true,
                    Font = new Font("Consolas", 9.5f),
                    ForeColor = Color.FromArgb(30, 41, 59)
                };
                
                Label lblCount = new Label
                {
                    Text = $"{item.Item2} times",
                    Top = 8,
                    AutoSize = true,
                    Font = new Font("Segoe UI", 9f),
                    ForeColor = Color.FromArgb(100, 116, 139),
                    Anchor = AnchorStyles.Right | AnchorStyles.Top
                };
                lblCount.Left = row.Width - lblCount.Width - 15;
                
                row.Controls.AddRange(new Control[] { lblRank, lblWord, lblCount });
                parent.Controls.Add(row);
                yPos += 35;
                index++;
            }
        }
        
        private void DisplayWelcomeMessage()
        {
            statsPanel.Controls.Clear();
            
            Label welcome = new Label
            {
                Text = "Welcome! 👋",
                Left = 20,
                Top = 50,
                AutoSize = true,
                Font = new Font("Segoe UI", 20f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59)
            };
            
            Label instructions = new Label
            {
                Text = "Get started by:\n\n" +
                       "1. Loading a text file using the 'Load File' button\n" +
                       "2. Pasting text from your clipboard\n" +
                       "3. Or typing directly into the text box\n\n" +
                       "Then click 'Analyze Text' to see detailed statistics!",
                Left = 20,
                Top = 100,
                Width = statsPanel.Width - 40,
                Height = 200,
                Font = new Font("Segoe UI", 10.5f),
                ForeColor = Color.FromArgb(100, 116, 139)
            };
            
            statsPanel.Controls.AddRange(new Control[] { welcome, instructions });
        }
        
        private void ClearAll()
        {
            inputTextBox.Text = "";
            fileNameLabel.Text = "No file loaded";
            statusLabel.Text = "Ready to analyze text...";
            exportButton.Enabled = false;
            currentReport = null;
            DisplayWelcomeMessage();
        }
        
        private void ShowExportMenu()
        {
            if (currentReport == null)
            {
                MessageBox.Show("No analysis report to export. Please analyze text first.", "Export", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            ContextMenuStrip exportMenu = new ContextMenuStrip();
            exportMenu.BackColor = Color.White;
            exportMenu.Font = new Font("Segoe UI", 9.5f);
            
            // JSON Export
            ToolStripMenuItem jsonItem = new ToolStripMenuItem("📄 Export as JSON");
            jsonItem.Click += (s, e) => ExportAsJSON();
            exportMenu.Items.Add(jsonItem);
            
            // TXT Export
            ToolStripMenuItem txtItem = new ToolStripMenuItem("📝 Export as Text Report");
            txtItem.Click += (s, e) => ExportAsText();
            exportMenu.Items.Add(txtItem);
            
            // CSV Export
            ToolStripMenuItem csvItem = new ToolStripMenuItem("📊 Export as CSV");
            csvItem.Click += (s, e) => ExportAsCSV();
            exportMenu.Items.Add(csvItem);
            
            exportMenu.Items.Add(new ToolStripSeparator());
            
            // Copy to Clipboard
            ToolStripMenuItem clipboardItem = new ToolStripMenuItem("📋 Copy Results to Clipboard");
            clipboardItem.Click += (s, e) => CopyToClipboard();
            exportMenu.Items.Add(clipboardItem);
            
            exportMenu.Show(exportButton, new Point(0, exportButton.Height));
        }
        
        private void ExportAsJSON()
        {
            if (currentReport == null) return;
            
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "JSON files (*.json)|*.json";
                dialog.DefaultExt = ".json";
                dialog.FileName = $"TextAnalysis_{DateTime.Now:yyyyMMdd_HHmmss}.json";
                dialog.Title = "Export Analysis as JSON";
                
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var exportResult = FileIO.exportToJson(currentReport, dialog.FileName);
                    
                    if (exportResult.IsOk)
                    {
                        string path = exportResult.ResultValue;
                        MessageBox.Show($"JSON report exported successfully!\n\nLocation:\n{path}", 
                            "Export Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        statusLabel.Text = $"✓ Exported to: {Path.GetFileName(path)}";
                    }
                    else
                    {
                        string error = exportResult.ErrorValue;
                        MessageBox.Show($"Export failed:\n{error}", "Export Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        statusLabel.Text = "Export failed";
                    }
                }
            }
        }
        
        private void ExportAsText()
        {
            if (currentReport == null) return;
            
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "Text files (*.txt)|*.txt";
                dialog.DefaultExt = ".txt";
                dialog.FileName = $"TextAnalysis_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                dialog.Title = "Export Analysis as Text Report";
                
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var stats = currentReport.Statistics;
                        var sb = new System.Text.StringBuilder();
                        
                        sb.AppendLine("╔════════════════════════════════════════════════════════════════╗");
                        sb.AppendLine("║              TEXT ANALYSIS REPORT                              ║");
                        sb.AppendLine("╚════════════════════════════════════════════════════════════════╝");
                        sb.AppendLine();
                        sb.AppendLine($"File Name: {currentReport.FileName}");
                        sb.AppendLine($"Analysis Date: {currentReport.AnalysisDate:yyyy-MM-dd HH:mm:ss}");
                        sb.AppendLine();
                        sb.AppendLine("═══════════════════════════════════════════════════════════════");
                        sb.AppendLine("BASIC STATISTICS");
                        sb.AppendLine("═══════════════════════════════════════════════════════════════");
                        sb.AppendLine($"Total Words:      {stats.TotalWords:N0}");
                        sb.AppendLine($"Total Sentences:  {stats.TotalSentences:N0}");
                        sb.AppendLine($"Total Paragraphs: {stats.TotalParagraphs:N0}");
                        sb.AppendLine($"Unique Words:     {stats.UniqueWords:N0}");
                        sb.AppendLine();
                        sb.AppendLine("═══════════════════════════════════════════════════════════════");
                        sb.AppendLine("AVERAGES");
                        sb.AppendLine("═══════════════════════════════════════════════════════════════");
                        sb.AppendLine($"Average Sentence Length: {stats.AverageSentenceLength:F2} words");
                        sb.AppendLine($"Average Word Length:     {stats.AverageWordLength:F2} characters");
                        sb.AppendLine();
                        sb.AppendLine("═══════════════════════════════════════════════════════════════");
                        sb.AppendLine("READABILITY");
                        sb.AppendLine("═══════════════════════════════════════════════════════════════");
                        sb.AppendLine($"Readability Score: {stats.ReadabilityScore:F2}/100");
                        sb.AppendLine($"Interpretation:    {Display.interpretReadability(stats.ReadabilityScore)}");
                        sb.AppendLine();
                        sb.AppendLine("═══════════════════════════════════════════════════════════════");
                        sb.AppendLine("WORD EXTREMES");
                        sb.AppendLine("═══════════════════════════════════════════════════════════════");
                        sb.AppendLine($"Longest Word:  {stats.LongestWord} ({stats.LongestWord.Length} characters)");
                        sb.AppendLine($"Shortest Word: {stats.ShortestWord} ({stats.ShortestWord.Length} characters)");
                        sb.AppendLine();
                        sb.AppendLine("═══════════════════════════════════════════════════════════════");
                        sb.AppendLine("TOP 10 MOST FREQUENT WORDS");
                        sb.AppendLine("═══════════════════════════════════════════════════════════════");
                        
                        int rank = 1;
                        foreach (var item in stats.TopWordFrequencies)
                        {
                            sb.AppendLine($"{rank,2}. {item.Item1,-20} → {item.Item2,5} occurrences");
                            rank++;
                        }
                        
                        sb.AppendLine();
                        sb.AppendLine("═══════════════════════════════════════════════════════════════");
                        sb.AppendLine($"Report generated by Text Analyzer Pro on {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                        sb.AppendLine("═══════════════════════════════════════════════════════════════");
                        
                        File.WriteAllText(dialog.FileName, sb.ToString());
                        
                        MessageBox.Show($"Text report exported successfully!\n\nLocation:\n{dialog.FileName}", 
                            "Export Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        statusLabel.Text = $"✓ Exported to: {Path.GetFileName(dialog.FileName)}";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Export failed:\n{ex.Message}", "Export Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        statusLabel.Text = "Export failed";
                    }
                }
            }
        }
        
        private void ExportAsCSV()
        {
            if (currentReport == null) return;
            
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "CSV files (*.csv)|*.csv";
                dialog.DefaultExt = ".csv";
                dialog.FileName = $"TextAnalysis_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                dialog.Title = "Export Analysis as CSV";
                
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var stats = currentReport.Statistics;
                        var sb = new System.Text.StringBuilder();
                        
                        // Basic Statistics
                        sb.AppendLine("Category,Metric,Value");
                        sb.AppendLine($"Basic Statistics,Total Words,{stats.TotalWords}");
                        sb.AppendLine($"Basic Statistics,Total Sentences,{stats.TotalSentences}");
                        sb.AppendLine($"Basic Statistics,Total Paragraphs,{stats.TotalParagraphs}");
                        sb.AppendLine($"Basic Statistics,Unique Words,{stats.UniqueWords}");
                        sb.AppendLine($"Averages,Average Sentence Length,{stats.AverageSentenceLength:F2}");
                        sb.AppendLine($"Averages,Average Word Length,{stats.AverageWordLength:F2}");
                        sb.AppendLine($"Readability,Readability Score,{stats.ReadabilityScore:F2}");
                        sb.AppendLine($"Readability,Interpretation,\"{Display.interpretReadability(stats.ReadabilityScore)}\"");
                        sb.AppendLine($"Word Extremes,Longest Word,{stats.LongestWord}");
                        sb.AppendLine($"Word Extremes,Shortest Word,{stats.ShortestWord}");
                        sb.AppendLine();
                        
                        // Word Frequencies
                        sb.AppendLine("Rank,Word,Frequency");
                        int rank = 1;
                        foreach (var item in stats.TopWordFrequencies)
                        {
                            sb.AppendLine($"{rank},{item.Item1},{item.Item2}");
                            rank++;
                        }
                        
                        File.WriteAllText(dialog.FileName, sb.ToString());
                        
                        MessageBox.Show($"CSV file exported successfully!\n\nLocation:\n{dialog.FileName}", 
                            "Export Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        statusLabel.Text = $"✓ Exported to: {Path.GetFileName(dialog.FileName)}";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Export failed:\n{ex.Message}", "Export Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        statusLabel.Text = "Export failed";
                    }
                }
            }
        }
        
        private void CopyToClipboard()
        {
            if (currentReport == null) return;
            
            try
            {
                var stats = currentReport.Statistics;
                var sb = new System.Text.StringBuilder();
                
                sb.AppendLine("TEXT ANALYSIS RESULTS");
                sb.AppendLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                sb.AppendLine();
                sb.AppendLine("📊 BASIC COUNTS");
                sb.AppendLine($"   • Total Words: {stats.TotalWords:N0}");
                sb.AppendLine($"   • Total Sentences: {stats.TotalSentences:N0}");
                sb.AppendLine($"   • Total Paragraphs: {stats.TotalParagraphs:N0}");
                sb.AppendLine($"   • Unique Words: {stats.UniqueWords:N0}");
                sb.AppendLine();
                sb.AppendLine("📈 AVERAGES");
                sb.AppendLine($"   • Average Sentence Length: {stats.AverageSentenceLength:F2} words");
                sb.AppendLine($"   • Average Word Length: {stats.AverageWordLength:F2} characters");
                sb.AppendLine();
                sb.AppendLine("📖 READABILITY");
                sb.AppendLine($"   • Score: {stats.ReadabilityScore:F2}/100");
                sb.AppendLine($"   • Level: {Display.interpretReadability(stats.ReadabilityScore)}");
                sb.AppendLine();
                sb.AppendLine("🔤 WORD EXTREMES");
                sb.AppendLine($"   • Longest: {stats.LongestWord} ({stats.LongestWord.Length} chars)");
                sb.AppendLine($"   • Shortest: {stats.ShortestWord} ({stats.ShortestWord.Length} chars)");
                sb.AppendLine();
                sb.AppendLine("⭐ TOP 10 MOST FREQUENT WORDS");
                
                int rank = 1;
                foreach (var item in stats.TopWordFrequencies)
                {
                    sb.AppendLine($"   {rank,2}. {item.Item1,-20} → {item.Item2} times");
                    rank++;
                }
                
                Clipboard.SetText(sb.ToString());
                
                MessageBox.Show("Results copied to clipboard successfully!", "Copied", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                statusLabel.Text = "✓ Results copied to clipboard";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to copy to clipboard:\n{ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}