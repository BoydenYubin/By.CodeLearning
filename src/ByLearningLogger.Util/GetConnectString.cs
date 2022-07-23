namespace ByLearningLogger.Util
{
    public class GetConnectString
    {
        /// <summary>
        /// Seq Url should keep the http://xx.xx.xx.xx:5341 or like,
        /// not just the ip:port
        /// </summary>
        /// <returns></returns>
        public static string GetSeqUrl()
        {
            return "xxxxxxxxxxxxxxx";
        }
        /// <summary>
        /// API key get from the seq UI,
        /// which prot:80 or the port was reflected by docker
        /// </summary>
        /// <returns></returns>
        public static string GetSeqAPIKey()
        {
            return "xxxxxxxxxxxxxxxx";
        }
    }
}
