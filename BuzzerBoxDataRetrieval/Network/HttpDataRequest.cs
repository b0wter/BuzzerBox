using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.UI.HtmlControls;

namespace BuzzerBoxDataRetrieval.Network
{
    /// <summary>
    /// Contains all the information necessary to create an http request.
    /// </summary>
    public class HttpDataRequest
    {
        /// <summary>
        /// Type of request sent, usually either GET or POST.
        /// </summary>
        public HttpMethod Method { get; set; }
        /// <summary>
        /// Path that needs to be queried for this request.
        /// </summary>
        public string RemotePath { get; set; }
        /// <summary>
        /// Returns the <see cref="RemotePath"/> with all <see cref="PathParameters"/> added.
        /// </summary>
        public string TotalPath
        {
            get
            {
                string totalPath = (RemotePath.EndsWith("/") == true ? RemotePath : RemotePath + "/");
                foreach (var pair in PathParameters)
                {
                    if (pair.Key == null)
                        totalPath += pair.Value;
                    else
                        totalPath += $"{pair.Key}/{pair.Value}";
                }
                return totalPath;
            }
        }
        /// <summary>
        /// Dictionary of parameters that will be appended to the url in the form:
        /// {remotePath}/{key1}/{value1}/{key2}/{value2}
        /// The parts are added in the order added.
        /// </summary>
        public List<KeyValuePair<string, string>> PathParameters { get; set; } = new List<KeyValuePair<string, string>>();
        /// <summary>
        /// Dictionary of parameters that will be added to the url as parameters. This applies to Get and Post requests.
        /// </summary>
        public List<KeyValuePair<string, string>> UrlParameters { get; set; } = new List<KeyValuePair<string, string>>();

        /// <summary>
        /// Dictionary of parameters that will be added to the POST body as form url encoded parameters.
        /// </summary>
        public List<KeyValuePair<string, string>> FormParameters { get; set; } = new List<KeyValuePair<string, string>>();
        /// <summary>
        /// Raw content of the body (when POSTing).
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// Sets wether to expect a single instance of the requested data or a list.
        /// </summary>
        public bool ResultIsList { get; set; } = true;

        /// <summary>
        /// Adds <paramref name="newParams"/> to the existing url parameters.
        /// </summary>
        /// <param name="newParams"></param>
        /// <returns></returns>
        public HttpDataRequest AddUrlParameters(Dictionary<string, string> newParams)
        {
            foreach (var pair in newParams)
                AddUrlParameter(pair.Key, pair.Value);
            return this;
        }

        /// <summary>
        /// Adds a single parameter to the existing url parameters.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public HttpDataRequest AddUrlParameter(string key, object value)
        {
            UrlParameters.Add(new KeyValuePair<string, string>(key, value.ToString()));
            return this;
        }

        /// <summary>
        /// Adds <paramref name="newParams"/> to the existing form-url-encoded parameters.
        /// </summary>
        /// <param name="newParams"></param>
        /// <returns></returns>
        public HttpDataRequest AddFormParameters(Dictionary<string, string> newParams)
        {
            foreach (var pair in newParams)
                AddFormParameter(pair.Key, pair.Value);
            return this;
        }

        /// <summary>
        /// Adds a single parameter to the existing form-url-encoded parameters.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public HttpDataRequest AddFormParameter(string key, object value)
        {
            FormParameters.Add(new KeyValuePair<string, string>(key, value.ToString()));
            return this;
        }

        /// <summary>
        /// Replaces the current body with <param name="body"></param>.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public HttpDataRequest AddBody(string body)
        {
            this.Body = body;
            return this;
        }

        /// <summary>
        /// Adds <paramref name="newParams"/> to the existing path parameters.
        /// </summary>
        /// <param name="newParams"></param>
        /// <returns></returns>
        public HttpDataRequest AddPathParameters(Dictionary<string, object> newParams)
        {
            foreach (var pair in newParams)
                AddPathParameter(pair.Key, pair.Value);
            return this;
        }

        /// <summary>
        /// Adds a single path parameter to the existing path parameters.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public HttpDataRequest AddPathParameter(string key, object value)
        {
            PathParameters.Add(new KeyValuePair<string, string>(key, value.ToString()));
            return this;
        }
    }
}