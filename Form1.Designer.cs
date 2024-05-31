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
            ExecSearchButton = new Button();
            GraphemeTextBox = new TextBox();
            SuspendLayout();
            // 
            // ExecSearchButton
            // 
            ExecSearchButton.Location = new Point(290, 45);
            ExecSearchButton.Name = "ExecSearchButton";
            ExecSearchButton.Size = new Size(266, 29);
            ExecSearchButton.TabIndex = 1;
            ExecSearchButton.Text = "Найти иероглифы с синонимами";
            ExecSearchButton.UseVisualStyleBackColor = true;
            ExecSearchButton.Click += ExecSearchButton_Click;
            // 
            // GraphemeTextBox
            // 
            GraphemeTextBox.Location = new Point(12, 12);
            GraphemeTextBox.Name = "GraphemeTextBox";
            GraphemeTextBox.PlaceholderText = "Введите графему или их последовательность без разделителей";
            GraphemeTextBox.Size = new Size(544, 27);
            GraphemeTextBox.TabIndex = 2;
            GraphemeTextBox.KeyPress += GraphemeTextBox_KeyPress;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(568, 83);
            Controls.Add(GraphemeTextBox);
            Controls.Add(ExecSearchButton);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            Text = "SimiGraph";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button ExecSearchButton;
        private TextBox GraphemeTextBox;
    }
}
