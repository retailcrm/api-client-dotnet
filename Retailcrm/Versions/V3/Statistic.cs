namespace Retailcrm.Versions.V3
{
    public partial class Client
    {
        /// <summary>
        /// Update statistic
        /// </summary>
        /// <returns></returns>
        public Response StatisticUpdate()
        {
            return Request.MakeRequest(
                "/statistic/update",
                Request.MethodGet
            );
        }
    }
}
