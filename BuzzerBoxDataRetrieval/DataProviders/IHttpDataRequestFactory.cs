using System.Collections.Generic;
using BuzzerBoxDataRetrieval.Network;

namespace BuzzerBoxDataRetrieval.DataProviders
{
    /// <summary>
    /// Interface for factories that create http requests.
    /// </summary>
    public interface IHttpDataRequestFactory
    {
        void SetDefaultUrlParameters(string key, string value);
        void SetDefaultUrlParameters(Dictionary<string, string> parameters);

        HttpDataRequest CreateGet<T>(bool resultIsList);
        HttpDataRequest CreatePost<T>(bool resultIsList);
    }
}