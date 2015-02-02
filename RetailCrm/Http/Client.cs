using RetailCrm.Extra;
using RetailCrm.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RetailCrm.Http
{
    /// <summary>
    /// HTTP client
    /// </summary>
    public class Client
    {
        public const string METHOD_GET = "GET";
        public const string METHOD_POST = "POST";
        private const string USER_AGENT = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
        private const string CONTENT_TYPE = "application/x-www-form-urlencoded";

        protected string url;
        protected Dictionary<string, object> defaultParameter;

        /// <summary>
        /// Creating HTTP client
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="parameters"></param>
        public Client(string apiUrl, Dictionary<string, object> parameters = null)
        {
            if (apiUrl.IndexOf("https://") == -1)
            {
                throw new ArgumentException("API schema requires HTTPS protocol");
            }

            url = apiUrl;
            defaultParameter = parameters;
        }

        /// <summary>
        /// Make HTTP request
        /// </summary>
        /// <param name="path"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public ApiResponse makeRequest(string path, string method, Dictionary<string, object> parameters = null, int timeout = 30)
        {
            string[] allowedMethods = new string[] { METHOD_GET, METHOD_POST };
            if (allowedMethods.Contains(method) == false)
            {
                throw new ArgumentException("Method \"" + method + "\" is not valid. Allowed methods are " + String.Join(", ", allowedMethods));
            }
            if (parameters == null) {
                parameters = new Dictionary<string, object>();
            }
            parameters = defaultParameter.Union(parameters).ToDictionary(k => k.Key, v => v.Value);
            path = url + path;
            string httpQuery = Tools.httpBuildQuery(parameters);

            if (method.Equals(METHOD_GET) && parameters.Count > 0)
            {
                path += "?" + httpQuery;
            }

            Exception exception = null;

            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(path);
            request.Method = method;

            if (method.Equals(METHOD_POST))
            {
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] bytes = encoding.GetBytes(httpQuery);
                request.ContentLength = bytes.Length;
                request.ContentType = CONTENT_TYPE;
                request.UserAgent = USER_AGENT;

                Stream post = request.GetRequestStream();
                post.Write(bytes, 0, bytes.Length);
                post.Flush();
                post.Close();
            }

            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
                exception = ex;
            }

            if (request == null || response == null)
            {
                throw new WebException(exception.ToString(), exception);
            }

            StreamReader reader = new StreamReader((Stream) response.GetResponseStream());
            string responseBody = reader.ReadToEnd();
            int statusCode = (int) response.StatusCode;

            return new ApiResponse(statusCode, responseBody);
        }
    }
}
