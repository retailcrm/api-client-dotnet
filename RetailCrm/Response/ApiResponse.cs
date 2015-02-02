using Newtonsoft.Json;
using RetailCrm.Exceptions;
using RetailCrm.Extra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailCrm.Response
{
    /// <summary>
    /// Response from retailCRM API
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// HTTP response status code
        /// </summary>
        protected int statusCode;

        /// <summary>
        /// Response
        /// </summary>
        protected Dictionary<string, object> response;

        /// <summary>
        /// Creating ApiResponse
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="responseBody"></param>
        public ApiResponse(int statusCode, string responseBody = null)
        {
            this.statusCode = statusCode;

            if (responseBody != null && responseBody.Length > 0)
            {
                Dictionary<string, object> response = new Dictionary<string, object>();
                try
                {
                    response = Tools.jsonDecode(responseBody);
                }
                catch (JsonReaderException e)
                {
                    throw new InvalidJsonException("Invalid JSON in the API response body. " + e.ToString());
                }

                this.response = response;
            }
        }

        /// <summary>
        /// Return HTTP response status code
        /// </summary>
        /// <returns>int</returns>
        public int getStatusCode()
        {
            return this.statusCode;
        }

        /// <summary>
        /// HTTP request was successful
        /// </summary>
        /// <returns>boolean</returns>
        public bool isSuccessful()
        {
            return this.statusCode < 400;
        }

        /// <summary>
        /// Return response
        /// </summary>
        /// <returns>Dictionary</returns>
        public object this[string code]
        {
            get {
                if (this.response.ContainsKey(code))
                {
                    return this.response[code];
                }
                else
                {
                    return new Dictionary<string, object>();
                }
            }
            set
            {
                throw new ArgumentException("Property \"" + code + "\" is not writable");
            }
        }


    }
}
