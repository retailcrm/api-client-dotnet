using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Retailcrm.Versions.V5
{
    public partial class Client
    {
        /// <summary>
        /// Create payment
        /// </summary>
        /// <param name="payment"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        public Response PaymentsCreate(Dictionary<string, object> payment, string site = "")
        {
            if (payment.Count < 1)
            {
                throw new ArgumentException("Parameter `payment` must contains a data");
            }

            if (!payment.ContainsKey("type"))
            {
                throw new ArgumentException("Parameter `type` must be set");
            }

            if (!payment.ContainsKey("order"))
            {
                throw new ArgumentException("Parameter `order` must be set");
            }

            return Request.MakeRequest(
                "/orders/payments/create",
                Request.MethodPost,
                FillSite(
                    site,
                    new Dictionary<string, object>
                    {
                        { "payment", new JavaScriptSerializer().Serialize(payment) }
                    }
                )
            );
        }

        /// <summary>
        /// Update payment
        /// </summary>
        /// <param name="payment"></param>
        /// <param name="by"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        public Response PaymentsUpdate(Dictionary<string, object> payment, string by = "id", string site = "")
        {
            if (payment.Count < 1)
            {
                throw new ArgumentException("Parameter `payment` must contains a data");
            }

            if (!payment.ContainsKey("id") && !payment.ContainsKey("externalId"))
            {
                throw new ArgumentException("Parameter `payment` must contains an id or externalId");
            }

            CheckIdParameter(by);

            string uid = by == "externalId" ? payment["externalId"].ToString() : payment["id"].ToString();

            return Request.MakeRequest(
                $"/orders/payments/{uid}/edit",
                Request.MethodPost,
                FillSite(
                    site,
                    new Dictionary<string, object>
                    {
                        { "by", by },
                        { "payment", new JavaScriptSerializer().Serialize(payment) }
                    }
                )
            );
        }

        /// <summary>
        /// Delete payment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Response PaymentsDelete(string id)
        {
            return Request.MakeRequest(
                $"/orders/payments/{id}/delete",
                Request.MethodPost
            );
        }
    }
}
