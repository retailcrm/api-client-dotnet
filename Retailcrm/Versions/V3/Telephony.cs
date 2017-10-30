using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Retailcrm.Versions.V3
{
    public partial class Client
    {
        /// <summary>
        /// Get manager
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        public Response TelephonyManagerGet(string phone, string details = "1")
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (string.IsNullOrEmpty(phone))
            {
                throw new ArgumentException("Parameter `phone` must contains a data");
            }

            parameters.Add("details", details);
            parameters.Add("phone", phone);

            return Request.MakeRequest("/telephony/manager", Request.MethodGet, parameters);
        }

        /// <summary>
        /// Send call event
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="type"></param>
        /// <param name="status"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public Response TelephonyCallEvent(string phone, string type, string status, string code)
        {
            if (string.IsNullOrEmpty(phone))
            {
                throw new ArgumentException("Parameter `phone` must contains a data");
            }

            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentException("Parameter `phone` must contains a data");
            }

            List<string> statuses = new List<string> { "answered", "busy", "cancel", "failed", "no answered" };
            List<string> types = new List<string> { "hangup", "in", "out" };

            if (!statuses.Contains(status))
            {
                throw new ArgumentException("Parameter `status` must be equal one of `answered|busy|cancel|failed|no answered`");
            }

            if (!types.Contains(type))
            {
                throw new ArgumentException("Parameter `type` must be equal one of `hangup|in|out`");
            }

            return Request.MakeRequest(
                "/telephony/call/event",
                Request.MethodPost,
                new Dictionary<string, object>
                {
                    { "phone", phone },
                    { "type", type },
                    { "hangupStatus", status},
                    { "code", code }
                }
            );
        }

        /// <summary>
        /// Upload calls
        /// </summary>
        /// <param name="calls"></param>
        /// <returns></returns>
        public Response TelephonyCallsUpload(List<object> calls)
        {
            if (calls.Count < 1)
            {
                throw new ArgumentException("Parameter `calls` must contains a data");
            }

            if (calls.Count > 50)
            {
                throw new ArgumentException("Parameter `calls` must contain 50 or less records");
            }

            return Request.MakeRequest(
                "/telephony/calls/upload",
                Request.MethodPost,
                new Dictionary<string, object>
                {
                    { "calls", new JavaScriptSerializer().Serialize(calls) }
                }
            );
        }

        /// <summary>
        /// Edit telephony settings
        /// </summary>
        /// <param name="code"></param>
        /// <param name="clientId"></param>
        /// <param name="url"></param>
        /// <param name="name"></param>
        /// <param name="logo"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public Response TelephonySettingsEdit(string code, string clientId, string url, string name, string logo, string active = "true")
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Parameter `name` must contains a data");
            }

            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentException("Parameter `phone` must contains a data");
            }

            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("Parameter `url` must contains a data");
            }

            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentException("Parameter `clientId` must contains a data");
            }

            return Request.MakeRequest(
                $"/telephony/setting/{code}",
                Request.MethodPost,
                new Dictionary<string, object>
                {
                    { "code", code },
                    { "name", name },
                    { "clientId", clientId},
                    { "makeCallUrl", url },
                    { "image", logo },
                    { "active", active }
                }
            );
        }
    }
}
