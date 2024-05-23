using System.Text.RegularExpressions;

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
            List<string> hanziList = new List<string>();

            foreach (var hanzi in this.hanziTextBox.Text.Split("\n"))
            {
                if(hanzi.Trim() == "")
                {
                    continue;
                }

                if(!Regex.Match(hanzi, @"^\s*[<\d]").Success)
                {
                    hanziList.Add(hanzi.Trim());
                }
            }

            var searcher = new Searcher(hanziList);
            searcher.RunAndGetResultList();
        }
    }
}
