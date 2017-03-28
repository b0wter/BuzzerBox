using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace BuzzerBoxDataRetrieval.Network
{
    /// <summary>
    /// Regular http connection that connects to a real server and tries to retrieve data.
    /// </summary>
    public class HttpConnection : IHttpConnection
    {
        /// <summary>
        /// Base url for all api endpoints.
        /// </summary>
        private readonly string baseUrl = "http://buzzerbox.azurewebsites.net/";
        /// <summary>
        /// Httpclient that is doing the actual work.
        /// </summary>
        private readonly System.Net.Http.HttpClient client = new HttpClient();

        public HttpConnection(string baseUrl)
        {
            this.baseUrl = baseUrl;
        }

        /// <summary>
        /// Loads a string from a remote endpoint.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<HttpResult> LoadFromUrl(HttpDataRequest request)
        {
            if (request.Method == HttpMethod.Get)
                return await LoadFromUrlByGet(request);
            else if(request.Method == HttpMethod.Post)
                return await LoadFromUrlByPost(request);
            else
                throw new ArgumentException($"Currently only Get and Post are supported but {request.Method} was requested.");
        }

        /// <summary>
        /// Retrieves a string and the request result details from an url. Is encapsulated in a HttpResult.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<HttpResult> LoadFromUrlByGet(HttpDataRequest request)
        {
            var query = BuildRequestUrl(request);
            HttpResponseMessage response = null;
            response = await client.GetAsync(query);
            var result = await ConvertHttpResponseToHttpResult(response, request.ResultIsList);
            return result;
        }

        /// <summary>
        /// Retrieves the data from a http response and saves status code, exception and result in an instance of <see cref="HttpResult"/>.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task<HttpResult> ConvertHttpResponseToHttpResult(HttpResponseMessage message, bool resultIsList)
        {
            var result = new HttpResult
            {
                StatusCode = (int)message.StatusCode,
                Exception = GetExceptionFromHttpResponse(message),
                Content = await GetContentFromHttpResponse(message),
                ResultIsList = resultIsList
            };

            return result;
        }

        /// <summary>
        /// Ensures that the given response is a success status code.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>An exception if status code is a non-success, otherwise null.</returns>
        private Exception GetExceptionFromHttpResponse(HttpResponseMessage message)
        {
            Exception exception = null;
            try
            {
                message.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            return exception;
        }

        /// <summary>
        /// Reads the actual content from a <see cref="HttpResponseMessage"/>.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task<string> GetContentFromHttpResponse(HttpResponseMessage message)
        {
            if (message.IsSuccessStatusCode)
            {
                var content = await message.Content.ReadAsStringAsync();
                return content;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieves a string by using a http post to a remote url.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<HttpResult> LoadFromUrlByPost(HttpDataRequest request)
        {
            var uri = BuildRequestUrl(request);
            var content = CreateHttpContentFromRequest(request);
            var response = await client.PostAsync(uri, content);
            var result = await ConvertHttpResponseToHttpResult(response, request.ResultIsList);
            return result;
        }

        /// <summary>
        /// Creates the content for a http post.
        /// </summary>
        /// <param name="request"></param>
        /// <returns><see cref="StringContent"/> if the request specifies a body, <see cref="FormUrlEncodedContent"/> is the request specifies form url encoded parameters or an empty <see cref="StringContent"/> if nothing is specified.</returns>
        /// <exception cref="ArgumentException"></exception>
        private HttpContent CreateHttpContentFromRequest(HttpDataRequest request)
        {
            if(!string.IsNullOrEmpty(request.Body) && request.FormParameters.Count > 0)
                throw new ArgumentException("A request has a body set as well as form parameters. Only one can be processed at a time.");

            if(!string.IsNullOrEmpty(request.Body))
                return new StringContent(request.Body);
            else if(request.FormParameters.Count > 0)
                return new FormUrlEncodedContent(request.FormParameters);
            else
                return new StringContent(string.Empty);
        }

        /// <summary>
        /// Builds an url containing the base url, remote endpoint and url parameters.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private string BuildRequestUrl(HttpDataRequest request)
        {
            var builder = new UriBuilder(baseUrl);
            builder.Path = request.TotalPath;

            var query = HttpUtility.ParseQueryString(builder.Query);
            foreach (var pair in request.UrlParameters)
                query[pair.Key] = pair.Value;
            builder.Query = query.ToString();
            return builder.ToString();
        }
    }
}