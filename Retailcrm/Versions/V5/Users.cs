using System;
using System.Collections.Generic;

namespace Retailcrm.Versions.V5
{
    public partial class Client
    {
        /// <summary>
        /// Update user status
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public Response UsersStatus(int id, string status)
        {
            List<string> statuses = new List<string> { "free", "busy", "dinner", "break"};

            if (!statuses.Contains(status))
            {
                throw new ArgumentException("Parameter `status` must be equal one of these values: `free|busy|dinner|break`");
            }

            return Request.MakeRequest(
                $"/users/{id}/status",
                Request.MethodPost,
                new Dictionary<string, object>
                {
                    { "status", status }
                }
            );
        }
    }
}
