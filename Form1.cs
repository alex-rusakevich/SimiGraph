using SimiGraph.FileFormat;
using System.Diagnostics;
using System.Net;
using System.Runtime.ExceptionServices;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Web;
using static System.Net.Mime.MediaTypeNames;

namespace SimiGraph
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void ExecSearchButton_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(GraphemeTextBox.Text))
            {
                MessageBox.Show("Строка поиска не должна быть пустой", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var graphemes = WhitespaceRegex().Replace(GraphemeTextBox.Text, "");

            ExecSearchButton.Enabled = false;
            GraphemeTextBox.Enabled = false;
            var execSearchButtonTextPrev = ExecSearchButton.Text;
            ExecSearchButton.Text = "Пожалуйста, подождите...";

            Task.Factory.StartNew(() => {
                var searcher = new Searcher(graphemes);
                var graphemesAndResList = searcher.GetGraphemesAndResultList();

                SaveFileDialog savefile = new()
                {
                    FileName = graphemes,
                    Filter = "Веб-сайт (*.html)|*.html|Текстовый файл (*.txt)|*.txt|Все файлы (*.*)|*.*"
                };

                this.Invoke(new Action(() =>
                {
                    if (savefile.ShowDialog() == DialogResult.OK)
                    {
                        string fileExt = Path.GetExtension(savefile.FileName.ToLower());
                        var fileFormatProcessor = FileFormatFactory.GetFormatProcessorByExt(fileExt);

                        #region Save result to file
                        using (var fileStream = new FileStream(savefile.FileName, FileMode.OpenOrCreate))
                        using (var resultStream = fileFormatProcessor.GenerateFormattedResult(
                            graphemesAndResList.Value, graphemesAndResList.Key, savefile.FileName))
                        {
                            resultStream.CopyTo(fileStream);
                        }
                        #endregion

                        var p = new Process
                        {
                            StartInfo = new ProcessStartInfo(savefile.FileName)
                            {
                                UseShellExecute = true
                            }
                        };
                        p.Start();
                    }
                }));
            }).ContinueWith(t => { 
                if (t.IsFaulted)
                {
                    ExceptionDispatchInfo.Capture(t.Exception.InnerException!).Throw();
                }

                this.Invoke(new Action(() => {
                    ExecSearchButton.Enabled = true;
                    GraphemeTextBox.Enabled = true;
                    ExecSearchButton.Text = execSearchButtonTextPrev;
                }));
            });
        }

        private void GraphemeTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                e.Handled = true;
                this.ExecSearchButton.PerformClick();
            }
        }

        [GeneratedRegex(@"\s+")]
        private static partial Regex WhitespaceRegex();
    }
}
