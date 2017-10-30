using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Retailcrm.Versions.V5
{
    public partial class Client
    {
        /// <summary>
        /// Get integration settings
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Response IntegrationsSettingGet(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentException("Parameter `code` is mandatory");
            }

            return Request.MakeRequest(
                $"/integration-modules/{code}",
                Request.MethodGet
            );
        }

        /// <summary>
        /// Create/Update integration settings
        /// </summary>
        /// <param name="integrationModule"></param>
        /// <returns></returns>
        public Response IntegrationsSettingsEdit(Dictionary<string, object> integrationModule)
        {
            if (integrationModule.Count < 1)
            {
                throw new ArgumentException("Parameter `integrationModule` must contain data");
            }

            if (!integrationModule.ContainsKey("code"))
            {
                throw new ArgumentException("Parameter `integrationModule` should contain `code`");
            }

            if (!integrationModule.ContainsKey("name"))
            {
                throw new ArgumentException("Parameter `integrationModule` should contain `name`");
            }

            return Request.MakeRequest(
                $"/integration-modules/{integrationModule["code"].ToString()}/edit",
                Request.MethodPost,
                new Dictionary<string, object>
                {
                    { "integrationModule", new JavaScriptSerializer().Serialize(integrationModule) }
                }
            );
        }
    }
}
