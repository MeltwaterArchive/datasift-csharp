using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;
using Datasift.Interfaces;
using Datasift;
namespace Datasift.DatasiftStream
{
    /**
     * Provides methods for consuming the datasift DatasiftStream
     */
    public class DatasiftStream : Publisher
    {
        private Config config;
        private List<DatasiftStreamClient> subcribers;
        private Thread streamThread;
        private State status = State.NOT_STARTED;
        public enum State { NOT_STARTED, RUNNING, STOPPED }
        private JsonSerializer json;
        /// <summary>
        /// How many times a connection has been made to the stream
        /// </summary>
        private int connectCount = 0;
        /// <summary>
        /// For time outs we can reconnect with increasing time outs of 1
        /// </summary>
        private int linearConnectTimeoutLength = 1;
        /// <summary>
        /// For none 200 statuses we can reconnect (if configured to) doubling the timeout each time and starting from 10
        /// </summary>
        private int exponentialConnectTimeoutLength = 10;
        /// <summary>
        /// Consumes the Datasift http DatasiftStream and notifies all subscribers on each interaction.
        /// </summary>
        /// <param name="config">
        /// A <see cref="Config"/> provides the DatasiftStream with the configuration needed to authenticate
        /// </param>
        /// <param name="consumer">
        /// A <see cref="DatasiftStreamClient"/> an optional consumer to subscribe to the DatasiftStream. If this is not null then the object's onInteraction
        /// will be invoked when an interaction is detected
        /// </param>
        public DatasiftStream(Config config, DatasiftStreamClient consumer)
        {
            this.config = config;
            subcribers = new List<DatasiftStreamClient>();
            if (consumer != null)
            {
                this.subcribers.Add(consumer);
            }
            json = new JsonSerializer();
        }
        /// <summary>
        /// Get the status of the DatasiftStream
        /// </summary>
        public State Status
        {
            get { return status; }
            set { }//do nothing, client shouldn't be able to change state
        }
        /// <summary>
        /// Subscribe to data updates. Basically any consumer passed will be updated when new data is available
        /// </summary>
        /// <param name="consumer" > The consumer to subscribe for updates
        /// A <see cref="DatasiftStreamClient"/>
        /// </param>
        public void Subscribe(DatasiftStreamClient consumer)
        {
            subcribers.Add(consumer);
        }
        /// <summary>
        /// StartStreaming consuming the DatasiftStream with teh given configurations
        /// Starts a new thread for the DatasiftStream 
        /// </summary>
        public void Consume()
        {
            streamThread = new Thread(new ThreadStart(this.StartStreaming));
            streamThread.Name = "DatasiftStream";
            streamThread.Start();
            //cycle a bit until DatasiftStream thread is active/alive
            while (!streamThread.IsAlive) ;
            //once the DatasiftStream is started updated the status
            status = State.RUNNING;
        }
        /// <summary>
        /// StartStreaming monitoring the datasift DatasiftStream
        /// Must be done in its own thread. :-)
        /// </summary>
        private void StartStreaming()
        {
            // used on each read operation
            byte[] buf = new byte[config.BufferSize];
            HttpWebRequest request;
            HttpWebResponse response;
            try
            {
                // DatasiftStream url to make request
                request = (HttpWebRequest)WebRequest.Create(config.getStreamUrl());
                request.Headers["Authorization"] = config.Authorization;
                request.UserAgent = config.UserAgent;
                // execute the request
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException e)
            {
                status = State.STOPPED;
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    //grab the header status code if it is a protocol error
                    switch (((HttpWebResponse)e.Response).StatusCode)
                    {
                        //do nothing...should never happen since, if it was okay an exception wouldn't have been raised but just in case!
                        case HttpStatusCode.OK: break;

                        case HttpStatusCode.Unauthorized:
                            PropagateStopped("Error 401 - Unauthorized. The credentials supplied were not valid.");
                            return;

                        case HttpStatusCode.Forbidden:
                            PropagateStopped("Error 403 - Forbidden. Your account has been denied access due to a violation.");
                            return;

                        case HttpStatusCode.NotFound:
                            PropagateStopped("Error 404 - Not Found. The DatasiftStream you requested could not be found. Are you using  a valid hash?.");
                            return;

                        case HttpStatusCode.ServiceUnavailable:
                            //stop if we've run out of retries
                            Retry("Error 503 - Service Unavailable. The node you were routed to is unavailable. Please try again");
                            return;
                        //if some unknown DatasiftStream response is detected then flag it!
                        default:
                            PropagateStopped(((HttpWebResponse)e.Response).StatusCode.ToString());
                            return;
                    }
                }
                if (e.Status.ToString() == "NameResolutionFailure")
                {
                    //if connection is not available then propagate error back up for user to handle
                    PropagateStopped("Unable to resolve the Datasift domain name. A possible cause is the local connection to the internet \n" + e.Message);
                    return;
                }
                //if we get this far, possibly other local network issues - too many to handle and be more specific
                PropagateStopped("The connection to the DatasiftStream could not be established! \n" + e.Message);
                return;
            }
            // get data from response DatasiftStream
            Stream resStream = response.GetResponseStream();
            //sets the read timeout
            resStream.ReadTimeout = config.Timeout;
            int count = -1;
            while (count > 0 || count == -1)
            {
                //first thing's first see if we've changed state to stop and break out of the loop
                if (this.status == State.STOPPED)
                {
                    PropagateStopped("Stop request received.");
                    break;
                }
                try
                {
                    // fill the buffer with data
                    count = resStream.Read(buf, 0, buf.Length);
                }
                catch (Exception ex)
                {
                     //stop if we've run out of retries
                     Retry(ex.Message);
                }

                // must have data in the buffer
                if (count != 0)
                {
                    // tell all subscribers new data is available after translating from bytes to UTF8
                    NotifyConsumers(Encoding.UTF8.GetString(buf, 0, count));
                }
            }
            resStream.Close();
            //if we've stopped reading for some reason may need to do some checks
            //i.e if state hasn't been changed to stopped then DatasiftStream may have ended prematurely/unexpectedly
            if (this.status != State.STOPPED)
            {
                Retry("DatasiftStream ended prematurely last read total =>"+count);
            }

        }

        private void Retry(string msg)
        {
            //this is the only documented none 200 status where the client should try to reconnect
            if (config.AutoReconnect && connectCount <= config.MaxRetries)
            {
                //we can reconnect increasing delay between each reconnect linearly (up to max configured retries, default=5)
                Thread.Sleep(exponentialConnectTimeoutLength * 1000);
                exponentialConnectTimeoutLength *= 2;//double wait time
                connectCount++;
                Consume();
            }
            else
            {
                status = State.STOPPED;
                //if we end up here, we've run out of retries 
                PropagateStopped(msg);
            }
        }
        /// <summary>
        /// Let all subscribers know the DatasiftStream has stopped
        /// </summary>
        private void PropagateStopped(string reason)
        {
            foreach (DatasiftStreamClient c in this.subcribers)
            {
                //invoke consumer's onstop, passing the reason for stopping
                c.onStopped(reason);
            }
        }
        private StringBuilder interactionCache = null;
        /// <summary>
        /// Update all consumers of the new data
        /// </summary>
        /// <param name="data"> The JSON/XML recieved from the server
        /// A <see cref="System.String"/>
        /// </param>
        private void NotifyConsumers(string data)
        {

            if (interactionCache == null)
            {
                interactionCache = new StringBuilder();
            }
            //split the data on each new line
            string[] interactions = data.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            //if no new line is encountered we need to cache the data we have and wait for the next notification
            if (interactions.Length == 1 && interactions[0] != "\r" && interactions[0] != "")
            {
                interactionCache.Append(data);
                return;
            }
            else if (interactions.Length > 1)
            {
                //if a new line is encountered a few things are possible
                //1. We have a full interaction at index 0
                //2. interactionCache is not null and has buffered data, in which case we append the contents of index 0
                //and clear the buffer then append all other indexes to the buffer
                //3. interactionCache is null

                //if interaction cache has some data already then append the contents of the first index
                if (interactionCache.Length > 0)
                {
                    interactionCache.Append(interactions[0]);
                    //we should now have a full interaction in the cache so push to clients
                    this.PushToSubscribers(interactionCache.ToString());
                    interactionCache = new StringBuilder();
                    //cache index 1 to N
                    for (int i = 1; i < interactions.Length; i++)
                    {
                        interactionCache.Append(interactions[i]);
                    }
                    return;
                }
                //if we have no data in the cache then take the contents at index 0 as a full interaction and put the rest in the cache
                else
                {
                    this.PushToSubscribers(interactions[0]);
                    interactionCache = new StringBuilder();
                    //cache index 1 to N
                    for (int i = 1; i < interactions.Length; i++)
                    {
                        interactionCache.Append(interactions[i]);
                    }
                    return;
                }
            }
        }
        private void PushToSubscribers(string data)
        {
            try
            {
                Interaction interaction = new Interaction(data);
                if (interaction.IsError())
                {
                    Retry(interaction.StatusMessage());
                    return;
                }
                foreach (DatasiftStreamClient c in this.subcribers)
                {
                    c.onInteraction(interaction);
                }
            }
            catch (Datasift.Exceptions.DataSiftIncompleteInteraction dii)
            {

            }
        }
        /// <summary>
        /// Stop the DatasiftStream
        /// </summary>
        public void Stop()
        {
            status = State.STOPPED;
            PropagateStopped("Client requested stop.");
        }
    }
}

