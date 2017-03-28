using System;
using System.Collections.Generic;
using System.Net.Http;
using BuzzerBoxDataRetrieval.Helpers;
using BuzzerBoxDataRetrieval.Network;
using Newtonsoft.Json;

namespace BuzzerBoxDataRetrieval.DataProviders
{
    /// <summary>
    /// Factory that creates <see cref="HttpDataRequest"/> instances.
    /// </summary>
    public class HttpDataRequestFactory : IHttpDataRequestFactory
    {
        /// <summary>
        /// Dictionary that stores the configuration of the endpoints and the corresponding types.
        /// </summary>
        private readonly Dictionary<string, string> endpoints = new Dictionary<string, string>();
        /// <summary>
        /// Dictionary that stores the default url parameters that will be added to all requests.
        /// </summary>
        private readonly Dictionary<string, string> defaultUrlParameters = new Dictionary<string, string>();

        public HttpDataRequestFactory()
        {
           InitFromFile();
        }

        /// <summary>
        /// Reads the configuration file to know which model belongs to which remote endpoint.
        /// </summary>
        private void InitFromFile()
        {
            var file = System.IO.File.ReadAllText("Assets/http_configuration.json");
            var config = JsonConvert.DeserializeObject<HttpDataRequestFactoryConfig>(file);
            foreach (var binding in config.Bindings)
            endpoints.Add(binding.Typename, binding.Endpoint);
        }

        /// <summary>
        /// Sets the default url parameter (key & value) that will be added to any request.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetDefaultUrlParameters(string key, string value)
        {
            defaultUrlParameters.Clear();
            defaultUrlParameters.Add(key, value);
        }

        /// <summary>
        /// Sets the default url parameters that will be added to any request.
        /// </summary>
        /// <param name="parameters"></param>
        public void SetDefaultUrlParameters(Dictionary<string, string> parameters)
        {
            defaultUrlParameters.Clear();
            foreach (var pair in parameters)
                 defaultUrlParameters.Add(pair.Key, pair.Value);
        }

        /// <summary>
        /// Creates a simple GET.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public HttpDataRequest CreateGet<T>(bool resultIsList)
        {
            return CreateRequest<T>(HttpMethod.Get, null, resultIsList);
        }

        /// <summary>
        /// Creates a simple POST.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public HttpDataRequest CreatePost<T>(bool resultIsList)
        {
            return CreateRequest<T>(HttpMethod.Post, null, resultIsList);
        }

        /// <summary>
        /// Creates any kind of request based on the input parameters.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="urlParameters"></param>
        /// <param name="formUrlEncodedParameters"></param>
        /// <param name="body"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private HttpDataRequest CreateRequest<T>(HttpMethod method, List<KeyValuePair<string, string>> urlParameters, bool resultIsList)
        {
            if (urlParameters == null)
                urlParameters = new List<KeyValuePair<string, string>>();
            urlParameters.AddRange(defaultUrlParameters);

            var request = new HttpDataRequest
            {
                Method = method,
                RemotePath = endpoints[typeof(T).Name],
                UrlParameters = urlParameters,
                ResultIsList = resultIsList
            };
            return request;
        }
    }

    /// <summary>
    /// Configuration to setup a HttpDataQuestFactory.
    /// </summary>
    public class HttpDataRequestFactoryConfig
    {
        public List<HttpDataRequestBinding> Bindings { get; set; }
    }

    /// <summary>
    /// Binding that represents the combination of a type and its corresponding remote path.
    /// </summary>
    public class HttpDataRequestBinding
    {
        public string Typename { get; set; }
        public string Endpoint { get; set; }
    }
}