namespace SimiGraph
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            execSearchButton = new Button();
            graphemeTextBox = new TextBox();
            SuspendLayout();
            // 
            // execSearchButton
            // 
            execSearchButton.Location = new Point(12, 45);
            execSearchButton.Name = "execSearchButton";
            execSearchButton.Size = new Size(266, 29);
            execSearchButton.TabIndex = 1;
            execSearchButton.Text = "Найти иероглифы с синонимами";
            execSearchButton.UseVisualStyleBackColor = true;
            execSearchButton.Click += execSearchButton_Click;
            // 
            // graphemeTextBox
            // 
            graphemeTextBox.Location = new Point(12, 12);
            graphemeTextBox.Name = "graphemeTextBox";
            graphemeTextBox.PlaceholderText = "Введите графему";
            graphemeTextBox.Size = new Size(266, 27);
            graphemeTextBox.TabIndex = 2;
            graphemeTextBox.KeyPress += graphemeTextBox_KeyPress;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(293, 83);
            Controls.Add(graphemeTextBox);
            Controls.Add(execSearchButton);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            Text = "SimiGraph";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button execSearchButton;
        private TextBox graphemeTextBox;
    }
}
