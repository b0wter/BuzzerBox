using System.Threading.Tasks;

namespace BuzzerBoxDataRetrieval.Network
{
    /// <summary>
    /// Interface for http connecitons. Provides a method retrieve string content from
    /// a remote endoint. Does not provide any methods to parse the received content.
    /// </summary>
    public interface IHttpConnection
    {
        /// <summary>
        /// Loads an string from an url.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<HttpResult> LoadFromUrl(HttpDataRequest request);
    }
}