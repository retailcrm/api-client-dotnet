using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Retailcrm.Versions.V4
{
    public partial class Client
    {
        /// <summary>
        /// Send call event
        /// </summary>
        /// <param name="ievent"></param>
        /// <returns></returns>
        public Response TelephonyCallEvent(Dictionary<string, object> ievent)
        {
            if (ievent.Count < 1)
            {
                throw new ArgumentException("Parameter `event` must contain data");
            }

            if (!ievent.ContainsKey("phone"))
            {
                throw new ArgumentException("Parameter `phone` must contains a data");
            }

            if (!ievent.ContainsKey("type"))
            {
                throw new ArgumentException("Parameter `type` must contains a data");
            }

            if (!ievent.ContainsKey("hangupStatus"))
            {
                throw new ArgumentException("Parameter `hangupStatus` must contains a data");
            }

            List<string> statuses = new List<string> { "answered", "busy", "cancel", "failed", "no answered" };
            List<string> types = new List<string> { "hangup", "in", "out" };

            if (!statuses.Contains(ievent["hangupStatus"].ToString()))
            {
                throw new ArgumentException("Parameter `status` must be equal one of `answered|busy|cancel|failed|no answered`");
            }

            if (!types.Contains(ievent["type"].ToString()))
            {
                throw new ArgumentException("Parameter `type` must be equal one of `hangup|in|out`");
            }

            return Request.MakeRequest(
                "/telephony/call/event",
                Request.MethodPost,
                new Dictionary<string, object>
                {
                    { "event", new JavaScriptSerializer().Serialize(ievent) }
                }
            );
        }

        /// <summary>
        /// Get telephony settings
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Response TelephonySettingsGet(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentException("Parameter `code` should contain data");
            }

            return Request.MakeRequest($"/telephony/setting/{code}", Request.MethodGet);
        }

        /// <summary>
        /// Edit telephony settings
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public Response TelephonySettingsEdit(Dictionary<string, object> configuration)
        {
            if (configuration.Count < 1)
            {
                throw new ArgumentException("Parameter `configuration` must contain data");
            }

            if (!configuration.ContainsKey("code"))
            {
                throw new ArgumentException("Parameter `configuration` should contain `code`");
            }

            if (!configuration.ContainsKey("name"))
            {
                throw new ArgumentException("Parameter `configuration` should contain `name`");
            }

            if (!configuration.ContainsKey("makeCallUrl"))
            {
                throw new ArgumentException("Parameter `configuration` should contain `makeCallUrl`");
            }

            if (!configuration.ContainsKey("clientId"))
            {
                throw new ArgumentException("Parameter `configuration` should contain `clientId`");
            }

            return Request.MakeRequest(
                $"/telephony/setting/{configuration["code"].ToString()}/edit",
                Request.MethodPost,
                new Dictionary<string, object>
                {
                    { "configuration", new JavaScriptSerializer().Serialize(configuration) }
                }
            );
        }
    }
}
