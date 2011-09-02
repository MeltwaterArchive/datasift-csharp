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
    class RecordingDemo
    {
        private RecordingDemo()
        {
            //We first need to create a configuration object
            Config config = new Config(Config.ConfigType.API, "zcourts", "0c0ea89ac6b914f852c7111c85f85d03");// "<USERNAME>", "<API_KEY>");
            //create an API request object and pass the configuration object as a param
            DatasiftApiRequest request = new DatasiftApiRequest(config);
            //send a CSDL to be compiled and get the response
            DatasiftApiResponse response = request.Compile("interaction.content contains \"google\"");
            Console.WriteLine("Compiled and got hash :" + response.Hash);

            //we can now work with this hash
            string hash = response.Hash;
 
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

//Step 4    list the recordings we have created
           response = request.Recording(RecordingOperation.Recording);
            //lets see how many recordings we have
            List<Recording> recoridngs = response.AllRecordings;//get all the exports returned
            foreach (Recording r in recoridngs)
            {
                if (response.IsError)
                {
                    Console.WriteLine(response.Error);
                }
                else
                {
                    recordingPrinter(response,r);
                }
            }            
            
//step 5    //exports, after data is recorded you may wish to export it, no point otherwise right?
            param = new Dictionary<string, string>();//virtually the same process, pass some parameters
            param.Add("recording_id", response.Recording.Id);//required
            param.Add("format", "json");//required, supports json,xls or xlsx
            param.Add("name", "exporting my awesome recording");//optional
            //param.Add("start", "<UNIX_TIMESTAMP>");
            //param.Add("end", "<UNIX_TIMESTAMP>");
            response = request.Recording(RecordingOperation.Export_Start,param);
            //API will return an Export object for the export we just started
            exportPrinter(response.Export);

//step 6    Get a list of our exports
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

//step 7    We may want to delete a recording some time in the future
            param = new Dictionary<string, string>();
            param.Add("id", "<Recording_ID>");//only requried param
            //make the API req.
            //request.Recording(RecordingOperation.Delete, param);// won't do this in the demo need to use it later

//step 8   We may also want to delete our exports
            param = new Dictionary<string, string>();
            param.Add("id", "<EXPORT_ID>");//only requried param
            request.Recording(RecordingOperation.Export_Delete, param);//no content is returned!

//step 9   After all these operations we must have used up some of our allowances!!! Lets get our usage
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
        //Must uncomment to run and comment out main in ApiDemo.cs
        //static void Main(string[] args)
        //{
        //    RecordingDemo p = new RecordingDemo();
        //    //prevent console from closing until it recieves an input...just so we can look at any output made
        //    Console.ReadLine();

        //}
    }
}
