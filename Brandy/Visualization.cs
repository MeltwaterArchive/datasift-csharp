using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Datasift.DatasiftStream;
using Datasift.Interfaces;
using Datasift;
using System.Threading;
namespace Brandy
{
    public partial class Visualization : UserControl, DatasiftStreamClient
    {
        private string username;
        private string password;
        private DatasiftStream stream;
        private bool streaming = false;
        private long lastTimePeriod = -1;
        private List<Interaction> lastTimePeriodsInteractions = new List<Interaction>();
        private int RefreshTime = 60;
        public static int minutes=30;
        /// <summary>
        /// Represents a timestamp keyed set of interactions
        /// </summary>
        private DataTable<long, List<Interaction>> data;
        public Visualization(string username, string password)
        {
            InitializeComponent();
            this.username = username;
            this.password = password;
            Config config = new Config(Config.ConfigType.STREAM, username, password);
            config.AutoReconnect = true;
            config.MaxRetries = 5;
            config.Timeout = 20000;//20 seconds time out
            config.Hash = "a292ecc39a5a942a4f0df779ddf1e7b8";
            stream = new DatasiftStream(config, this);
            data = new DataTable<long, List<Interaction>>();
            ResizeGraph();
        }

        /// <summary>
        /// Fill the right panel with the graph
        /// </summary>
        private void ResizeGraph()
        {
            graph.Width = splitContainer.Panel2.Width;
            graph.Height = splitContainer.Panel2.Height - 50;
        }
        private void resizePanel()
        {
            splitContainer.Width = this.Width;
            splitContainer.Height = this.Height;
        }
        private void splitContainerResize(object sender, EventArgs e)
        {
            ResizeGraph();
        }

        private void VisualizationResize(object sender, EventArgs e)
        {
            resizePanel();
        }
        private void start_Click(object sender, EventArgs e)
        {
            StartStreaming();
        }

        private void StartStreaming()
        {
            //start the stream
            stream.Consume();
            startGrapThread();
            streaming = true;
        }
        private void startGrapThread()
        {
            Thread th;
            th = new Thread(new ThreadStart(RefreshGraph));
            th.Start();
        }

        private void RefreshGraph()
        {
            if (streaming)
            {
                graph.Draw(data);
                //Thread.Sleep(30000);//sleep for 30 seconds before updating
            }
        }

        public void onInteraction(Interaction interaction)
        {
            
            if (interaction.HasStatus() == false)
            {
                Console.WriteLine("Interaction recieved" + interaction.Get("klout"));
                string created = interaction.Get("interaction.created_at");
                try
                {
                    DateTime time = DateTime.Parse(created);
                    //10K ticks = 1 mili second :- ticks/10000 = milli seconds and 1000 milliseconds = 1 second
                    long seconds = (time.Ticks / 10000) / 1000;
                    if (lastTimePeriod == -1)
                    {
                        lastTimePeriod = seconds + RefreshTime;
                    }
                    //if this tweet is created more than a minute a go 
                    //put this minute's list of interactions in the datatable
                    //assign a new time for last minute and create a new list
                    else if (lastTimePeriod < seconds)
                    {
                        data.AddRow(lastTimePeriod, lastTimePeriodsInteractions);
                        RefreshGraph();
                        lastTimePeriod = seconds + RefreshTime;
                        lastTimePeriodsInteractions = new List<Interaction>();
                    }
                    lastTimePeriodsInteractions.Add(interaction);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unable to convert the created at value for this interaction. Ignoring...");
                }
            }
            else if (interaction.IsError())
            {
                onStopped("An error occured" + interaction.StatusMessage());
            }
            //else if (interaction.IsTick())
            //{
            //    Console.WriteLine(interaction.StatusMessage());
            //}
            else {
                Console.WriteLine(interaction.StatusMessage());
            }
        }

        public void onStopped(string reason)
        {
            streaming = false;
            Console.WriteLine("Stopped streaming");
        }

        private void Visualization_Load(object sender, EventArgs e)
        {
            ResizeGraph();
        }
    }
}
