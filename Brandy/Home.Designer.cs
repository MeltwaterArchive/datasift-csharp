namespace Brandy
{
    partial class Home
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.username = new System.Windows.Forms.TextBox();
            this.apiKey = new System.Windows.Forms.TextBox();
            this.warning = new System.Windows.Forms.Label();
            this.visualize = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(62, 130);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Username";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(66, 159);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "API Key";
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(130, 129);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(218, 20);
            this.username.TabIndex = 16;
            this.username.Text = "zcourts";
            // 
            // apiKey
            // 
            this.apiKey.Location = new System.Drawing.Point(129, 159);
            this.apiKey.Name = "apiKey";
            this.apiKey.Size = new System.Drawing.Size(218, 20);
            this.apiKey.TabIndex = 17;
            this.apiKey.Text = "0c0ea89ac6b914f852c7111c85f85d03";
            // 
            // warning
            // 
            this.warning.AutoSize = true;
            this.warning.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.warning.ForeColor = System.Drawing.Color.Red;
            this.warning.Location = new System.Drawing.Point(5, 34);
            this.warning.Name = "warning";
            this.warning.Size = new System.Drawing.Size(0, 24);
            this.warning.TabIndex = 18;
            // 
            // visualize
            // 
            this.visualize.Location = new System.Drawing.Point(136, 191);
            this.visualize.Name = "visualize";
            this.visualize.Size = new System.Drawing.Size(210, 34);
            this.visualize.TabIndex = 19;
            this.visualize.Text = "Start Visualizing!";
            this.visualize.UseVisualStyleBackColor = true;
            this.visualize.Click += new System.EventHandler(this.visualize_Click);
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(502, 349);
            this.Controls.Add(this.visualize);
            this.Controls.Add(this.warning);
            this.Controls.Add(this.apiKey);
            this.Controls.Add(this.username);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Name = "Home";
            this.Text = "Datasift Brandy -  Brand awareness";
            this.Resize += new System.EventHandler(this.Home_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.TextBox apiKey;
        private System.Windows.Forms.Label warning;
        private System.Windows.Forms.Button visualize;



    }
}

