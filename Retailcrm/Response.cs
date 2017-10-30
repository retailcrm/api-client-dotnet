using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Retailcrm
{
    /// <summary>
    /// Response
    /// </summary>
    public class Response
    {
        private readonly int _statusCode;
        private readonly string _rawResponse;
        private readonly Dictionary<string, object> _responseData;

        /// <summary>
        /// Response constructor
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="responseBody"></param>
        /// <exception cref="ArgumentException"></exception>
        public Response(int statusCode, string responseBody = null)
        {
            _statusCode = statusCode;

            if (string.IsNullOrEmpty(responseBody))
            {
                throw new ArgumentException("Response body is empty");
            }

            _rawResponse = responseBody;

            var jsSerializer = new JavaScriptSerializer();
            _responseData = (Dictionary<string, object>)jsSerializer.DeserializeObject(responseBody);
        }

        /// <summary>
        /// Get response status code
        /// </summary>
        /// <returns></returns>
        public int GetStatusCode()
        {
            return _statusCode;
        }

        /// <summary>
        /// Get response body data
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetResponse()
        {
            return _responseData;
        }

        /// <summary>
        /// Get raw response body
        /// </summary>
        /// <returns></returns>
        public string GetRawResponse()
        {
            return _rawResponse;
        }

        /// <summary>
        /// Check response is successfull
        /// </summary>
        /// <returns></returns>
        public bool IsSuccessfull()
        {
            return _statusCode < 400;
        }
    }
}