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
            //We first need to create a configuration object
            Config config = new Config(Config.ConfigType.API, "zcourts", "0c0ea89ac6b914f852c7111c85f85d03");// "<USERNAME>", "<API_KEY>");
            //create an API request object and pass the configuration object as a param
            DatasiftApiRequest request = new DatasiftApiRequest(config);
            //send a CSDL to be compiled and get the response
            DatasiftApiResponse response = request.Compile("interaction.content contains \"google\"");
            Console.WriteLine("Compiled and got hash :" + response.Hash);

            //make a new request, getting some interactions for the csdl we just compiled
            string hash = response.Hash;
            //get a single interaction, using the third param to set the count
            //response = request.Stream(hash, null, 1);
            ////buffering usually starts on the first request so if we didn't get anything back wait and try again
            //while (response.Stream.Count == 0)
            //{
            //    Console.WriteLine("Nothing returned from the buffered stream sleeping for 5 seconds...");
            //    Thread.Sleep(5000);//wait 5 seconds
            //    response = request.Stream(hash, null, 1);
            //}
            //Console.WriteLine("Number of interactions returned : " + response.Stream.Count + "\n\n");
            //Console.WriteLine("Stream response objects\n");

            //foreach (Interaction i in response.Stream)
            //{
            //    Console.WriteLine(i.ToString());
            //}

            //Console.WriteLine("Stream response objects end\n\n");

            ////now see the costs for this stream
            //response = request.Cost(hash);
            //Console.WriteLine("\nCosts : " + response.StreamCosts + "\n");

//step 1    //recording operations need a list of parameters, some are optional others aren't
            Dictionary<string, string> param = new Dictionary<string, string>();
            //schedule a recording for this stream
            //first thing to do is set the parameters the api requires
            param.Add("hash", hash);//only requried param
            param.Add("name", "My Awesome stream");
            //we can set when to start and when to end
            //param.Add("start", "<UNIX_TIMESTAMP>");
            //param.Add("end", "<UNIX_TIMESTAMP>");
//step 2    make the request - All reording related operations are done via .Recording method
            response=request.Recording(RecordingOperation.Schedule, param);
            Recording rec = response.Recording;
            //the above returns data about the recording we just created
            recordingPrinter(response,rec);

//step 3    We've made a new recording and has its ID so if we want to we can update the recording details
            param = new Dictionary<string, string>();
            param.Add("hash", hash);//only requried param
            param.Add("name", "New name for my awesome stream");//change the name of the stream
            //we can set when to start and when to end, the time must be sensible!!!
            //param.Add("start", "<UNIX_TIMESTAMP>");
            //param.Add("end", "<UNIX_TIMESTAMP>");
            //make the request to the API
            response = request.Recording(RecordingOperation.Update, param);
            recordingPrinter(response, response.Recording);
        }

        private void recordingPrinter(DatasiftApiResponse response,Recording rec)
        {

            if (response.IsError)
            {
                Console.WriteLine(response.Error);
            }
            else
            {
                //we can not print data about it
                Console.WriteLine("Start time : " + rec.StartTime);
                Console.WriteLine("FInish time : " + rec.FinishTime);
                Console.WriteLine("Name : " + rec.Name);
                Console.WriteLine("ID : " + rec.Id);
                Console.WriteLine("Hash : " + rec.Hash);
            }
        }

        static void Main(string[] args)
        {
            ApiDemo p = new ApiDemo();
            //prevent console from closing until it recieves an input...just so we can look at any output made
            Console.ReadLine();

        }
    }
}
