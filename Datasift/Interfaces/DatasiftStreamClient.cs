using System;
using Datasift.DatasiftStream;
//Datasift namespace has all the interfaces,exceptions and abstract classes
namespace Datasift.Interfaces
{
    /// <summary>
    /// Any object that wishes to consume the datasift DatasiftStream
    /// </summary>
    public interface DatasiftStreamClient
    {
        /// <summary>
        /// This is called for each interaction recieved from the DatasiftStream 
        /// </summary>
        /// <param name="data"> The</param>
        void onInteraction(Interaction interaction);

        /// <summary>
        /// Invoked when the DatasiftStream is stopped
        /// </summary>
        /// <param name="reason">The reason the DatasiftStream is stopped</param>
        void onStopped(string reason);
    }
}

