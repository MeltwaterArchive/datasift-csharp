using System;
using Datasift.Exceptions;
namespace Datasift
{
    /**
     * A simple configuration  object for a DatasiftStream. One per hash
     */
    public class Config
    {
        public enum ConfigType { STREAM, API };
        private string host;
        private string username;
        private string api_key;
        private string hash;
        private int streamBufferSize = 32768;//32KB default buffer size
        private int streamTimeout = 10000;
        private bool autoReconnect = false;
        private int maxRetries = 5;
        private ConfigType configType = ConfigType.STREAM;
        
        /// <summary>
        /// <see cref="Config(string username, string api_key, string hash)"/>
        /// This assumes a config configType of STREAM
        /// </summary>
        public Config(string host, string username, string api_key, string hash)
        {
            if (username == null || api_key == null)
            {
                throw new InvalidStreamConfiguration();
            }
            if (host == null)
            {
                this.host = "http://stream.datasift.com/";
            }
            else
            {
                this.host = host;
            }
            this.username = username;
            this.api_key = api_key;
            //blindly assign hash even if its null since datasift now supports opening a stream/connection without subscribing to a specific stream
            this.hash = hash;
        }
        /// <summary>
        /// Create a set of configurations to be used for creating a DatasiftStream
        /// This assumes a config configType of STREAM
        /// </summary>
        /// <param name="username">Your Datasift username</param>
        /// <param name="api_key">Your Datasift API Key</param>
        /// <param name="hash">A DatasiftStream hash, obtained from the datasift website</param>
        public Config(string username, string api_key, string hash)
            : this(null, username, api_key, hash) { }

        /// <summary>
        /// Creates a configuration specifying the configType of config it is, i.e. Stream or API
        /// </summary>
        /// <param name="configType"> One of the available types <see cref="ConfigType"/> STREAM or API</param>
        /// <param name="username">Datasift username</param>
        /// <param name="api_key">Datasift API key</param>
        public Config(ConfigType type, string username, string api_key)
        {
            if (username == null || api_key == null)
            {
                throw new InvalidStreamConfiguration();
            }
            if (type == ConfigType.STREAM)
            {
                this.host = "http://stream.datasift.com/";
            }
            else if (type == ConfigType.API)
            {
                this.configType = type;
                this.host = "http://api.datasift.com/";
            }
            this.username = username;
            this.api_key = api_key;
        }
        /// <summary>
        /// Return the DatasiftStream url generated from this config's settings
        /// </summary> 
        public string getStreamUrl()
        {
            return host + hash;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns> Return the  URL to the datasift core API with only username and api key get params being set</returns>
        public string getApiUrl(string method)
        {
            return host + method;
        }

        /// <summary>
        /// Get and set the buffer size used for a DatasiftStream
        /// </summary>
        public int BufferSize
        {
            get { return this.streamBufferSize; }
            set { this.streamBufferSize = value; }
        }
        /// <summary>
        /// Get and Set the time (in miliseconds) out for this DatasiftStream. i.e. the time afterwhich the DatasiftStream will stop
        /// if it hasn't recieved anything from the server.
        /// </summary>
        public int Timeout
        {
            get { return this.streamTimeout; }
            set { this.streamTimeout = value; }
        }
        /// <summary>
        /// Get and Set whether the client should automatically try to reconnect. 
        /// </summary>
        public bool AutoReconnect
        {
            get { return this.autoReconnect; }
            set { this.autoReconnect = value; }
        }
        /// <summary>
        /// Get or Set the maximum amount of times the client should try to re-connect automatically.
        /// Not relevant unless <see cref="AutoReconnect"/> is set to true, defaults to 5
        /// </summary>
        public int MaxRetries
        {
            get { return this.maxRetries; }
            set { this.maxRetries = value; }
        }
        /// <summary>
        /// Get or set the has value for this config.
        /// Changing the hash of a config instance after it is passed to a stream does not affect/change the stream
        /// unless the stream is stopped and restarted with the same config.
        /// </summary>
        public string Hash
        {
            get { return hash; }
            set { hash = value; }
        }

        public string Authorization
        {
            get { return username + ":" + api_key; }
            set { }
        }
        public string Version
        {
            get { return "1.0.1"; }
            set { }
        }
        public string UserAgent
        {
            get { return "DataSiftCSharp/"+Version; }
            set { }
        }
    }

}

