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

        private void execSearchButton_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(graphemeTextBox.Text))
            {
                MessageBox.Show("������ ������ �� ������ ���� ������", "������",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var graphemes = Regex.Replace(graphemeTextBox.Text, @"\s+", "");

            execSearchButton.Enabled = false;
            graphemeTextBox.Enabled = false;
            var execSearchButtonTextPrev = execSearchButton.Text;
            execSearchButton.Text = "����������, ���������...";

            Task.Factory.StartNew(() => {
                var searcher = new Searcher(graphemes);
                var graphemesAndResList = searcher.GetGraphemesAndResultList();

                SaveFileDialog savefile = new SaveFileDialog();
                savefile.FileName = graphemes;
                savefile.Filter = "���-���� (*.html)|*.html|��������� ���� (*.txt)|*.txt|��� ����� (*.*)|*.*";

                this.Invoke(new Action(() =>
                {
                    if (savefile.ShowDialog() == DialogResult.OK)
                    {
                        FileFormatProcessor.SaveResultAs(
                            graphemesAndResList.Value, graphemesAndResList.Key, savefile.FileName);
                    }
                }));
            }).ContinueWith(t => { 
                if (t.IsFaulted)
                {
                    ExceptionDispatchInfo.Capture(t.Exception.InnerException!).Throw();
                }

                this.Invoke(new Action(() => {
                    execSearchButton.Enabled = true;
                    graphemeTextBox.Enabled = true;
                    execSearchButton.Text = execSearchButtonTextPrev;
                }));
            });
        }

        private void graphemeTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                e.Handled = true;
                this.execSearchButton.PerformClick();
            }
        }
    }
}
