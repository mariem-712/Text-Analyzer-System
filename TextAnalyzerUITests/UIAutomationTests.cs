using System;
using System.Threading;
using System.Windows.Forms;
using Xunit;
using TextAnalyzerWinForms;

namespace TextAnalyzerUITests
{
    public class UIAutomationTests : IDisposable
    {
        private MainForm form;
        private Thread uiThread;
        
        public UIAutomationTests()
        {
            // Create form on STA thread
            uiThread = new Thread(() =>
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                form = new MainForm();
                form.Show();
                Application.Run(form);
            });
            
            uiThread.SetApartmentState(ApartmentState.STA);
            uiThread.Start();
            
            // Wait for form to be created
            Thread.Sleep(1000);
        }
        
        [Fact]
        public void Form_ShouldLoad_WithCorrectTitle()
        {
            Assert.NotNull(form);
            InvokeOnUIThread(() =>
            {
                Assert.Equal("Text Analyzer Pro", form.Text);
            });
        }
        
        [Fact]
        public void Form_ShouldHave_InitialWelcomeMessage()
        {
            InvokeOnUIThread(() =>
            {
                var statsPanel = FindControl<Panel>(form, "statsPanel");
                Assert.NotNull(statsPanel);
                Assert.True(statsPanel.Controls.Count > 0);
            });
        }
        
        [Fact]
        public void InputTextBox_ShouldUpdateWordCount_WhenTextEntered()
        {
            InvokeOnUIThread(() =>
            {
                var inputTextBox = FindControl<TextBox>(form, "inputTextBox");
                var wordCountLabel = FindControl<Label>(form, "wordCountLabel");
                
                Assert.NotNull(inputTextBox);
                Assert.NotNull(wordCountLabel);
                
                inputTextBox.Text = "Hello world test";
                Application.DoEvents();
                
                Assert.Contains("3", wordCountLabel.Text);
            });
        }
        
        [Fact]
        public void AnalyzeButton_ShouldBeEnabled_WhenFormLoads()
        {
            InvokeOnUIThread(() =>
            {
                var analyzeButton = FindControl<Button>(form, "analyzeButton");
                Assert.NotNull(analyzeButton);
                Assert.True(analyzeButton.Enabled);
            });
        }
        
        [Fact]
        public void ExportButton_ShouldBeDisabled_Initially()
        {
            InvokeOnUIThread(() =>
            {
                var exportButton = FindControl<Button>(form, "exportButton");
                Assert.NotNull(exportButton);
                Assert.False(exportButton.Enabled);
            });
        }
        
        [Fact]
        public void AnalyzeButton_ShouldProcessText_AndShowResults()
        {
            InvokeOnUIThread(() =>
            {
                var inputTextBox = FindControl<TextBox>(form, "inputTextBox");
                var analyzeButton = FindControl<Button>(form, "analyzeButton");
                var statsPanel = FindControl<Panel>(form, "statsPanel");
                
                // Enter test text
                inputTextBox.Text = "Hello world. This is a test.";
                Application.DoEvents();
                
                // Click analyze button
                analyzeButton.PerformClick();
                Application.DoEvents();
                Thread.Sleep(500); // Wait for processing
                
                // Check if results are displayed
                Assert.True(statsPanel.Controls.Count > 1);
            });
        }
        
        [Fact]
        public void ExportButton_ShouldBeEnabled_AfterAnalysis()
        {
            InvokeOnUIThread(() =>
            {
                var inputTextBox = FindControl<TextBox>(form, "inputTextBox");
                var analyzeButton = FindControl<Button>(form, "analyzeButton");
                var exportButton = FindControl<Button>(form, "exportButton");
                
                // Enter text and analyze
                inputTextBox.Text = "Sample text for testing.";
                analyzeButton.PerformClick();
                Application.DoEvents();
                Thread.Sleep(500);
                
                // Check if export button is enabled
                Assert.True(exportButton.Enabled);
            });
        }
        
        [Fact]
        public void ClearButton_ShouldResetForm_WhenClicked()
        {
            InvokeOnUIThread(() =>
            {
                var inputTextBox = FindControl<TextBox>(form, "inputTextBox");
                var clearButton = FindControl<Button>(form, "clearButton");
                var wordCountLabel = FindControl<Label>(form, "wordCountLabel");
                
                // Enter text
                inputTextBox.Text = "Test text";
                Application.DoEvents();
                
                // Click clear
                clearButton.PerformClick();
                Application.DoEvents();
                
                // Verify cleared
                Assert.Empty(inputTextBox.Text);
                Assert.Contains("0", wordCountLabel.Text);
            });
        }
        
        [Fact]
        public void PasteButton_ShouldPasteFromClipboard_WhenClipboardHasText()
        {
            InvokeOnUIThread(() =>
            {
                var pasteButton = FindControl<Button>(form, "pasteButton");
                var inputTextBox = FindControl<TextBox>(form, "inputTextBox");
                
                // Set clipboard content
                Clipboard.SetText("Clipboard test content");
                
                // Click paste button
                pasteButton.PerformClick();
                Application.DoEvents();
                
                // Verify pasted
                Assert.Equal("Clipboard test content", inputTextBox.Text);
            });
        }
        
        [Fact]
        public void LoadFileButton_ShouldExist_AndBeClickable()
        {
            InvokeOnUIThread(() =>
            {
                var loadFileButton = FindControl<Button>(form, "loadFileButton");
                Assert.NotNull(loadFileButton);
                Assert.True(loadFileButton.Enabled);
                Assert.Contains("Load File", loadFileButton.Text);
            });
        }
        
        [Fact]
        public void StatusLabel_ShouldShowInitialMessage()
        {
            InvokeOnUIThread(() =>
            {
                var statusLabel = FindControl<Label>(form, "statusLabel");
                Assert.NotNull(statusLabel);
                Assert.Contains("Ready", statusLabel.Text);
            });
        }
        
        [Fact]
        public void Form_ShouldHaveCorrectMinimumSize()
        {
            InvokeOnUIThread(() =>
            {
                Assert.Equal(1200, form.MinimumSize.Width);
                Assert.Equal(700, form.MinimumSize.Height);
            });
        }
        
        // Helper Methods
        private void InvokeOnUIThread(Action action)
        {
            if (form != null && form.InvokeRequired)
            {
                form.Invoke(action);
            }
            else
            {
                action();
            }
        }
        
        private T FindControl<T>(Control parent, string name) where T : Control
        {
            foreach (Control control in parent.Controls)
            {
                if (control.Name == name && control is T typedControl)
                {
                    return typedControl;
                }
                
                var found = FindControl<T>(control, name);
                if (found != null)
                {
                    return found;
                }
            }
            return null;
        }
        
        public void Dispose()
        {
            if (form != null && !form.IsDisposed)
            {
                InvokeOnUIThread(() => form.Close());
            }
            
            if (uiThread != null && uiThread.IsAlive)
            {
                uiThread.Join(2000);
            }
        }
    }
}