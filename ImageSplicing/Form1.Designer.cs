namespace ImageSplicing
{
    partial class Form1
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
            this.btnLoadCanvas = new System.Windows.Forms.Button();
            this.btnLoadImage = new System.Windows.Forms.Button();
            this.btnMatch = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.labelStatus = new System.Windows.Forms.Label();
            this.btnStatus = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLoadCanvas
            // 
            this.btnLoadCanvas.Location = new System.Drawing.Point(12, 761);
            this.btnLoadCanvas.Name = "btnLoadCanvas";
            this.btnLoadCanvas.Size = new System.Drawing.Size(54, 27);
            this.btnLoadCanvas.TabIndex = 0;
            this.btnLoadCanvas.Text = "Canvas";
            this.btnLoadCanvas.UseVisualStyleBackColor = true;
            this.btnLoadCanvas.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnLoadImage
            // 
            this.btnLoadImage.Location = new System.Drawing.Point(72, 761);
            this.btnLoadImage.Name = "btnLoadImage";
            this.btnLoadImage.Size = new System.Drawing.Size(53, 27);
            this.btnLoadImage.TabIndex = 1;
            this.btnLoadImage.Text = "Folder";
            this.btnLoadImage.UseVisualStyleBackColor = true;
            this.btnLoadImage.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnMatch
            // 
            this.btnMatch.Location = new System.Drawing.Point(131, 761);
            this.btnMatch.Name = "btnMatch";
            this.btnMatch.Size = new System.Drawing.Size(67, 27);
            this.btnMatch.TabIndex = 2;
            this.btnMatch.Text = "Match";
            this.btnMatch.UseVisualStyleBackColor = true;
            this.btnMatch.Click += new System.EventHandler(this.button3_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(218, 762);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(1111, 27);
            this.progressBar1.TabIndex = 3;
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.labelStatus.Location = new System.Drawing.Point(1348, 763);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(35, 12);
            this.labelStatus.TabIndex = 4;
            this.labelStatus.Text = "Done!";
            // 
            // btnStatus
            // 
            this.btnStatus.BackColor = System.Drawing.Color.Green;
            this.btnStatus.Enabled = false;
            this.btnStatus.FlatAppearance.BorderSize = 0;
            this.btnStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStatus.Location = new System.Drawing.Point(1338, 778);
            this.btnStatus.Name = "btnStatus";
            this.btnStatus.Size = new System.Drawing.Size(77, 10);
            this.btnStatus.TabIndex = 5;
            this.btnStatus.Text = "button4";
            this.btnStatus.UseVisualStyleBackColor = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1422, 793);
            this.Controls.Add(this.btnStatus);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnMatch);
            this.Controls.Add(this.btnLoadImage);
            this.Controls.Add(this.btnLoadCanvas);
            this.MinimumSize = new System.Drawing.Size(1430, 820);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLoadCanvas;
        private System.Windows.Forms.Button btnLoadImage;
        private System.Windows.Forms.Button btnMatch;
        public System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Button btnStatus;
    }
}

