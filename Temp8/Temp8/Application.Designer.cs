using System.Drawing;

namespace Temp8
{
    partial class Application
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.solveButton = new System.Windows.Forms.Button();
            this.areaLabel = new System.Windows.Forms.Label();
            this.tricolorButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // solveButton
            // 
            this.solveButton.Location = new System.Drawing.Point(9, 10);
            this.solveButton.Margin = new System.Windows.Forms.Padding(2);
            this.solveButton.Name = "solveButton";
            this.solveButton.Size = new System.Drawing.Size(112, 41);
            this.solveButton.TabIndex = 0;
            this.solveButton.Text = "button1";
            this.solveButton.UseVisualStyleBackColor = true;
            this.solveButton.Click += new System.EventHandler(this.OnSolveButtonClick);
            // 
            // areaLabel
            // 
            this.areaLabel.AutoSize = true;
            this.areaLabel.ForeColor = System.Drawing.Color.White;
            this.areaLabel.Location = new System.Drawing.Point(360, 37);
            this.areaLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.areaLabel.Name = "areaLabel";
            this.areaLabel.Size = new System.Drawing.Size(0, 13);
            this.areaLabel.TabIndex = 3;
            // 
            // tricolorButton
            // 
            this.tricolorButton.Location = new System.Drawing.Point(126, 10);
            this.tricolorButton.Margin = new System.Windows.Forms.Padding(2);
            this.tricolorButton.Name = "tricolorButton";
            this.tricolorButton.Size = new System.Drawing.Size(112, 41);
            this.tricolorButton.TabIndex = 1;
            this.tricolorButton.Text = "button2";
            this.tricolorButton.UseVisualStyleBackColor = true;
            this.tricolorButton.Click += new System.EventHandler(this.OnTricolorButtonClick);
            // 
            // Application
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(647, 417);
            this.Controls.Add(this.tricolorButton);
            this.Controls.Add(this.areaLabel);
            this.Controls.Add(this.solveButton);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Application";
            this.Text = "Form1";
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnFormClick);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button solveButton;
        private System.Windows.Forms.Button tricolorButton;
        private System.Windows.Forms.Label areaLabel;
    }
}

