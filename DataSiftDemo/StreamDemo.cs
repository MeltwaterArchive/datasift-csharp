using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Datasift.DatasiftStream;
using Datasift.Interfaces;
using Datasift;
namespace DataSiftDemo
{
    class StreamDemo : DatasiftStreamClient //must implement the datasiftstreamclient interface
    {
        private StreamDemo()
        {
            //the configuration for this stream
            Config config = new Config("zcourts", "0c0ea89ac6b914f852c7111c85f85d03", "a292ecc39a5a942a4f0df779ddf1e7b8");
            config.BufferSize = 32768;//32kb custom stream buffer size,smaller tends to be faster, tune to preference
            config.Timeout = 20000;//20 seconds time out, try not to set too low
            config.AutoReconnect = true;
            config.MaxRetries = 10;
            //create a stream
            DatasiftStream stream = new DatasiftStream(config, this);
            //start consuming
            stream.Consume();
        }

        static void Main(string[] args)
        {
            StreamDemo p = new StreamDemo();
            //prevent console from closing until it recieves an input...just so we can look at any output made
            Console.ReadLine();

        }

        public void onInteraction(Interaction data)
        {
            Console.WriteLine();
            if (data.IsError())
            {

                Console.WriteLine("Interaction is an error : " + data.StatusMessage());
            }
            else if (data.IsTick())
            {
                Console.WriteLine("Interaction is a tick : " + data.StatusMessage());
            }
            else if (data.IsWarning())
            {
                Console.WriteLine("Interaction is a warning : " + data.StatusMessage());
            }
            else
            {
                Console.WriteLine("Source : " + data.Get("interaction.source"));
                Console.WriteLine("Authorname : " + data.Get("interaction.author.name"));
                Console.WriteLine("Content : " + data.Get("interaction.content"));
            }
            //print the whole interaction
            // Console.WriteLine("Interaction \n" + data.ToString() + "\nInteraction End\n\n");
        }

        public void onStopped(string reason)
        {
            Console.WriteLine("DatasiftStream stopped : " + reason);
        }
    }
}
