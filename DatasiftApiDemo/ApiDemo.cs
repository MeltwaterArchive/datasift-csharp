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
//step 4    //exports, after data is recorded you may wish to export it, no point otherwise right?
            param = new Dictionary<string, string>();//virtually the same process, pass some parameters
            param.Add("recording_id", response.Recording.Id);//required
            param.Add("format", "json");//required, supports json,xls or xlsx
            param.Add("name", "exporting my awesome recording");//optional
            //param.Add("start", "<UNIX_TIMESTAMP>");
            //param.Add("end", "<UNIX_TIMESTAMP>");
            response = request.Recording(RecordingOperation.Export_Start,param);
            //API will return an Export object for the export we just started
            exportPrinter(response.Export);

//step 5    Get a list of our exports
            //param = new Dictionary<string, string>();//virtually the same process, pass some parameters
            //param.Add("id","<RECORDING_ID>");//optional, none of the params for this end point is required
            //param.Add("page", "1");//optional :P you could paginate over all your exports if you had a vast amount
            //param.Add("count", "20");//optional :( Limit the amount of exports returned
            //the params above are shown for demonstration but aren't used in this request
            response = request.Recording(RecordingOperation.Export);//yeah, param list can be optional
            //lets see how many recordings we have
            int size = response.ExportCount;
            Console.WriteLine("We have" + size + " exports that we can iterate over");
            List<Export> exports = response.AllExports;//get all the exports returned
            foreach(Export ex in exports){
                if (response.IsError)
                {
                    Console.WriteLine(response.Error);
                }
                else
                {
                    exportPrinter(ex);
                }
            }

//step 6    We may want to delete a recording some time in the future
            param = new Dictionary<string, string>();
            param.Add("id", "<Recording_ID>");//only requried param
            //make the API req.
            //request.Recording(RecordingOperation.Delete, param);// won't do this in the demo need to use it later

//step 7   We may also want to delete our exports
            param = new Dictionary<string, string>();
            param.Add("id", "<EXPORT_ID>");//only requried param
            request.Recording(RecordingOperation.Export_Delete, param);//no content is returned!

//step 8   After all these operations we must have used up some of our allowances!!! Lets get our usage
            //param = new Dictionary<string, string>();
            //param.Add("hash", "<STREAM_HASH>");//not required but would let the API return usage for a single stream
            response=request.Usage();
            DatasiftUsage usage = response.Usage;
            Console.WriteLine("Delivered : " + usage.Delivered);
            Console.WriteLine("Processed : " + usage.Processed);
            Console.WriteLine("Twitter Delivered : " + usage.GetType("twitter").Delivered);
            Console.WriteLine("Twitter Processed : " + usage.GetType("twitter").Processed);
        }

        private void exportPrinter(Export ex)
        {
            Console.WriteLine("Export name : " + ex.Name);
            Console.WriteLine("Export ID : " + ex.Id);
            Console.WriteLine("Recording ID : " + ex.RecordingId);
            Console.WriteLine("Export start : " + ex.StartTime);
            Console.WriteLine("Export end : " + ex.FinishTime);
            Console.WriteLine("Export status : " + ex.Status);
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
