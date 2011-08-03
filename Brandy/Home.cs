using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Brandy
{
    public partial class Home : Form
    {
        private Visualization view;
        public Home()
        {
            InitializeComponent();
            Pen pen = new Pen(Color.Red);
            pen.Width = 10;
            Graphics g = CreateGraphics();
            g.DrawLine(pen, 0, 0, 200, 200);
            pen.Dispose();
            g.Dispose();
            this.Refresh();
        }

        private void visualize_Click(object sender, EventArgs e)
        {
            if (this.username.TextLength == 0 || this.apiKey.TextLength == 0)
            {
                this.warning.Text = "Please enter a username and API Key - Both are required!";
                this.warning.Refresh();
                return;
            }
            view = new Visualization(this.username.Text, this.apiKey.Text);
            view.Show();
            Controls.Clear();
            Controls.Add(view);
        }

        private void Home_Resize(object sender, EventArgs e)
        {
            view.Width = this.Width;
            view.Height = this.Height;
        }
    }
}
