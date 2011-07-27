using System;
namespace Datasift.Interfaces
{
    /**
     * DatasiftStreamClient interface for subscribers to use to get data from a DatasiftStream
     */
    public interface Publisher
    {
        /// <summary>
        /// Add additional consumers to the set of cunsomers subscribed to a DatasiftStream
        /// </summary>
        /// <param name="consumer"></param>
        void Subscribe(DatasiftStreamClient consumer);
        /// <summary>
        /// Invoked to start consuming the DataSift http DatasiftStream
        /// </summary>
        //void StartStreaming();
        /// <summary>
        /// Invoked to stop the DatasiftStream
        /// </summary>
        void Stop();
        /// <summary>
        /// StartStreaming consuming the DatasiftStream
        /// </summary>
        void Consume();
    }
}

