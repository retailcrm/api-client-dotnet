using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Retailcrm
{
    /// <summary>
    /// Request
    /// </summary>
    public class Request
    {
        /// <summary>
        /// Get method
        /// </summary>
        public const string MethodGet = "GET";
        /// <summary>
        /// Post method
        /// </summary>
        public const string MethodPost = "POST";
        private const string UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
        private const string ContentType = "application/x-www-form-urlencoded";

        private readonly string _url;
        private readonly Dictionary<string, object> _defaultParameters;

        /// <summary>
        /// Request constructor
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="parameters"></param>
        /// <exception cref="ArgumentException"></exception>
        public Request(string apiUrl, Dictionary<string, object> parameters = null)
        {
            if (apiUrl.IndexOf("https://", StringComparison.Ordinal) == -1)
            {
                throw new ArgumentException("API schema requires HTTPS protocol");
            }

            _url = apiUrl;
            _defaultParameters = parameters;
        }

        /// <summary>
        /// Make request method
        /// </summary>
        /// <param name="path"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="WebException"></exception>
        public Response MakeRequest(string path, string method, Dictionary<string, object> parameters = null)
        {
            string[] allowedMethods = { MethodGet, MethodPost };

            if (allowedMethods.Contains(method) == false)
            {
                throw new ArgumentException($"Method {method} is not valid. Allowed HTTP methods are {string.Join(", ", allowedMethods)}");
            }

            if (parameters == null)
            {
                parameters = new Dictionary<string, object>();
            }

            parameters = _defaultParameters.Union(parameters).ToDictionary(k => k.Key, v => v.Value);
            path = _url + path;

            string httpQuery = QueryBuilder.BuildQueryString(parameters);

            if (method.Equals(MethodGet) && parameters.Count > 0)
            {
                path += "?" + httpQuery;
            }

            Exception exception = null;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(path);
            request.Method = method;

            if (method.Equals(MethodPost))
            {
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] bytes = encoding.GetBytes(httpQuery);
                request.ContentLength = bytes.Length;
                request.ContentType = ContentType;
                request.UserAgent = UserAgent;

                Stream post = request.GetRequestStream();
                post.Write(bytes, 0, bytes.Length);
                post.Flush();
                post.Close();
            }

            HttpWebResponse response;

            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException webException)
            {
                response = (HttpWebResponse)webException.Response;
                exception = webException;
            }

            if (request == null || response == null)
            {
                throw new WebException(exception.ToString(), exception);
            }

            StreamReader reader = new StreamReader(response.GetResponseStream());
            string responseBody = reader.ReadToEnd();
            int statusCode = (int)response.StatusCode;

            return new Response(statusCode, responseBody);
        }
    }
}