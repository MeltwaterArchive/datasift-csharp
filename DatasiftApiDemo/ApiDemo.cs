using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Datasift.DatasiftStream;
using Datasift.Interfaces;
using Datasift;
using Datasift.Api;
namespace DataSiftDemo
{
    class ApiDemo
    {
        private ApiDemo()
        {
            //the configuration for this request
            Config config = new Config(Config.ConfigType.API, "zcourts", "0c0ea89ac6b914f852c7111c85f85d03");
            //config.BufferSize = 32768;//32kb custom request buffer size,smaller tends to be faster, tune to preference
            //config.Timeout = 20000;//20 seconds time out, try not to set too low
            //config.AutoReconnect = true;
            //config.MaxRetries = 10;

            //create an API request object
            DatasiftApiRequest request = new DatasiftApiRequest(config);
            //send a CSDL to be compiled and get the response
            DatasiftApiResponse response = request.Compile("interaction.content contains \"google\"");
            Console.WriteLine("Compiled and got hash :" + response.Hash);
            Thread.Sleep(5000);//wait 5 seconds
            //make a new request, getting some interactions for the csdl we just compiled
            //to get all available interactions
            //response = request.Stream(response.Hash);
            string hash = response.Hash;
            //get a single interaction, using the third param to set the count
            response = request.Stream(hash, null, 1);
            Console.WriteLine("Number of interactions returned : " + response.Stream.Capacity + "\n\n");
            Console.WriteLine("Stream response objects\n");

            foreach (Interaction i in response.Stream)
            {
                Console.WriteLine(i.ToString());
            }

            Console.WriteLine("Stream response objects end\n\n");

            //now see the costs for this stream
            response = request.Cost(hash);
            Console.WriteLine("\nCosts : "+response.StreamCosts+"\n");

        }

        static void Main(string[] args)
        {
            ApiDemo p = new ApiDemo();
            //prevent console from closing until it recieves an input...just so we can look at any output made
            Console.ReadLine();

        }
    }
}
