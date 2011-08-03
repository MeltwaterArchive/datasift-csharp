namespace Brandy
{
    partial class Visualization
    {
        private System.Windows.Forms.SplitContainer splitContainer;
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.start = new System.Windows.Forms.Button();
            this.graph = new Brandy.LineGraph();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.splitContainer.Panel1.Controls.Add(this.start);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.graph);
            this.splitContainer.Size = new System.Drawing.Size(780, 497);
            this.splitContainer.SplitterDistance = 171;
            this.splitContainer.TabIndex = 0;
            this.splitContainer.Resize += new System.EventHandler(this.splitContainerResize);
            // 
            // start
            // 
            this.start.AutoSize = true;
            this.start.Location = new System.Drawing.Point(20, 3);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(94, 23);
            this.start.TabIndex = 0;
            this.start.Text = "Start Visualizing!";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.start_Click);
            // 
            // graph
            // 
            this.graph.AutoScroll = true;
            this.graph.AutoSize = true;
            this.graph.BackColor = System.Drawing.SystemColors.ControlDark;
            this.graph.Location = new System.Drawing.Point(-7, 3);
            this.graph.Name = "graph";
            this.graph.Size = new System.Drawing.Size(207, 84);
            this.graph.TabIndex = 0;
            // 
            // Visualization
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.splitContainer);
            this.Name = "Visualization";
            this.Size = new System.Drawing.Size(780, 497);
            this.Load += new System.EventHandler(this.Visualization_Load);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private LineGraph graph;
        private System.Windows.Forms.Button start;
    }
}
