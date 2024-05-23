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
            hanziTextBox = new TextBox();
            execSearchButton = new Button();
            SuspendLayout();
            // 
            // hanziTextBox
            // 
            hanziTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            hanziTextBox.Location = new Point(12, 12);
            hanziTextBox.Multiline = true;
            hanziTextBox.Name = "hanziTextBox";
            hanziTextBox.PlaceholderText = "Скопируйте и вставьте сюда иероглифы";
            hanziTextBox.ScrollBars = ScrollBars.Vertical;
            hanziTextBox.Size = new Size(775, 451);
            hanziTextBox.TabIndex = 0;
            // 
            // execSearchButton
            // 
            execSearchButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            execSearchButton.Location = new Point(521, 469);
            execSearchButton.Name = "execSearchButton";
            execSearchButton.Size = new Size(266, 29);
            execSearchButton.TabIndex = 1;
            execSearchButton.Text = "Найти иероглифы с синонимами";
            execSearchButton.UseVisualStyleBackColor = true;
            execSearchButton.Click += execSearchButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(799, 510);
            Controls.Add(execSearchButton);
            Controls.Add(hanziTextBox);
            Name = "Form1";
            Text = "SimiGraph";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox hanziTextBox;
        private Button execSearchButton;
    }
}
