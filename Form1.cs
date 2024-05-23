using System.Net;
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
            execSearchButton.Enabled = false;
            var execSearchButtonTextPrev = execSearchButton.Text;
            execSearchButton.Text = "Пожалуйста, подождите...";

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                var searcher = new Searcher(graphemeTextBox.Text.Trim());
                searcher.RunAndGetResultList();

                this.Invoke(new Action(() =>
                {
                    execSearchButton.Enabled = true;
                    execSearchButton.Text = execSearchButtonTextPrev;
                }));
            }).Start();
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
