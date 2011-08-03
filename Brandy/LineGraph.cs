using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Datasift.DatasiftStream;

namespace Brandy
{
    public partial class LineGraph : UserControl
    {
        private int height = 0;
        private int width = 0;
        private DataTable<long, List<Datasift.DatasiftStream.Interaction>> data;
        private string legend;
        public LineGraph()
            : this(-1, -1)
        {
            InitializeComponent();
        }
        public LineGraph(int height, int width)
        {
            if (height != -1 && width != -1)
            {
                this.height = height;
                this.width = width;
                this.Size = new System.Drawing.Size(width, height);
            }
            data = new DataTable<long, List<Datasift.DatasiftStream.Interaction>>();
        }
        /// <summary>
        /// Used to invoke refresh on the graph when created from different a thread
        /// </summary>
        private delegate void SafeRefresh();
        private void MyGraph_Paint(object sender, PaintEventArgs e)
        {
            // Create a local version of the graphics object for the PictureBox.
            Graphics g = e.Graphics;
            int timePeriod = 30;
            int kloutMax = 10;//the max we can have for a klout score
            int xdistance = (this.Width - 30) / timePeriod;//tracking 30 timePeriod
            int ydistance = (this.Height - 30) / 2;//klout score -100 to +100
            //where does x start?
            int xorigin = 40;
            //and y is half way - i.e half the height
            int yorigin = this.Height / 2;
            int yscale = 10;
            DrawGrid(g, yscale, timePeriod, kloutMax, xdistance, ydistance, xorigin, yorigin);
            //now draw our data points
            if (data.Size > 0)
            {
                PointF previousPoint = new PointF(-1, -1);
                int x = xorigin;
                int xi = 0;
                float y = yorigin;
                //Each iteration is = 1 minute
                foreach (KeyValuePair<long, List<Interaction>> entry in data.Next())
                {
                    float sum = 0;
                    foreach (Interaction i in entry.Value)
                    {
                        sum += Convert.ToInt32(i.Get("klout.score"));
                    }
                    //average klout score for this minute is the Y co-ord
                    float avg = (sum / entry.Value.Count);

                    if (avg > 0)
                    {
                        y = yorigin - (((float)ydistance / kloutMax) * ((float)avg / yscale));
                    }
                    else
                    {
                        y = yorigin - (((float)ydistance / kloutMax) * ((float)(avg * -1) / yscale));
                    }
                    x = xorigin + (xdistance * xi++);
                    //g.DrawRectangle(Pens.Red, x, y,5,5);
                    //draw an x at the point
                    g.DrawLine(Pens.Purple, x - 3, y + 3, x + 3, y - 3);
                    g.DrawLine(Pens.Purple, x + 3, y + 3, x - 3, y - 3);
                    //interaction.author.name

                    String drawString = xi + "," + String.Format("{0:0.0}", avg); ;
                    Font drawFont = new Font("Arial", 10);
                    SolidBrush drawBrush = new SolidBrush(Color.Blue);
                    StringFormat drawFormat = new StringFormat();
                    drawFormat.FormatFlags = StringFormatFlags.DirectionVertical;
                    g.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);



                    if (previousPoint.X == -1 && previousPoint.Y == -1)
                    {
                        previousPoint = new PointF(x, y);
                    }
                    g.DrawLine(Pens.Red, new PointF(x, y), previousPoint);
                    previousPoint = new PointF(x, y);
                }
            }
        }

        private void DrawGrid(Graphics g, int yscale, int timePeriod, int kloutMax, int xdistance, int ydistance, int xorigin, int yorigin)
        {
            g.DrawLine(Pens.Black, xorigin, yorigin, this.Right, yorigin);
            g.DrawLine(Pens.Black, xorigin, 0, xorigin, Bottom);

            float x = xorigin;
            float y = yorigin;

            String drawString = "";
            Font drawFont = new Font("Arial", 10);//need at least 300px to see all numbers 10*30mins
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            StringFormat drawFormat = new StringFormat();
            drawFormat.FormatFlags = StringFormatFlags.NoClip;

            //draw X axis
            for (int i = 1; i <= timePeriod; i++)
            {
                drawString = "" + i;
                g.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);
                g.DrawRectangle(Pens.Black, x, y - 5, 1, 10);
                x += xdistance;
            }
            x = xorigin - 25;
            y = yorigin;
            float fy = yorigin;
            //draw -y axis
            for (int i = 0; i <= kloutMax; i++)
            {
                drawString = "" + (i * yscale * -1);
                g.DrawString(drawString, drawFont, drawBrush, x, fy, drawFormat);
                g.DrawRectangle(Pens.Black, x + 20, fy, 10, 1);
                fy += (float)ydistance / kloutMax;
            }
            x = xorigin - 25;
            y = yorigin;
            fy = yorigin;
            //draw +y axis
            for (int i = 0; i <= kloutMax; i++)
            {
                drawString = "" + (i * yscale);
                g.DrawString(drawString, drawFont, drawBrush, x, fy, drawFormat);
                g.DrawRectangle(Pens.Black, x + 20, fy, 10, 1);
                fy -= (float)ydistance / kloutMax;
            }
        }

        public void Draw(DataTable<long, List<Datasift.DatasiftStream.Interaction>> data)
        {
            this.data = data;
            if (this.InvokeRequired)
            {
                SafeRefresh r = Refresh;
                this.Invoke(r);
            }
            else
            {
                Refresh();
            }
        }
    }
}
