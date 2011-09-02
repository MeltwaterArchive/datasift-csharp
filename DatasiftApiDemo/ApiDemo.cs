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

//Step 4a   We first need to create a configuration object
            Config config = new Config(Config.ConfigType.API, "<USERNAME>", "<API_KEY>");

//Step 4b   create an API request object and pass the configuration object as a param
            DatasiftApiRequest request = new DatasiftApiRequest(config);
//Step 4c   send a CSDL to be compiled and get the response
            DatasiftApiResponse response = request.Compile("interaction.content contains \"google\"");
            Console.WriteLine("Compiled and got hash :" + response.Hash);

            string hash = response.Hash;
//Step 4d   make a new request, getting some interactions for the csdl we just compiled
            //get a single interaction, using the third param to set the count
            response = request.Stream(hash, null, 1);
            //buffering usually starts on the first request so if we didn't get anything back wait and try again
            while (response.Stream.Count == 0)
            {
                Console.WriteLine("Nothing returned from the buffered stream sleeping for 5 seconds...");
                Thread.Sleep(5000);//wait 5 seconds
                response = request.Stream(hash, null, 1);
            }
            Console.WriteLine("Number of interactions returned : " + response.Stream.Count + "\n\n");
            Console.WriteLine("Stream response objects\n");

            foreach (Interaction i in response.Stream)
            {
                Console.WriteLine(i.ToString());
            }

            Console.WriteLine("Stream response objects end\n\n");
//Step 4e   now see the costs for this stream
            response = request.Cost(hash);
            Console.WriteLine("\nCosts : " + response.StreamCosts + "\n");
        }

        static void Main(string[] args)
        {
            ApiDemo p = new ApiDemo();
            //prevent console from closing until it recieves an input...just so we can look at any output made
            Console.ReadLine();

        }
    }
}
