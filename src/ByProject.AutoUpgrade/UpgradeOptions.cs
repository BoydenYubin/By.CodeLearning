namespace ByProject.AutoUpgrade
{
    public class UpgradeOptions
    {
        /// <summary>
        /// upgrade server url
        /// </summary>
        public string UpgradeUrl { get; set; }
        /// <summary>
        /// default upgrade
        /// </summary>
        public string UpgradeEndpoint { get; set; }
        /// <summary>
        /// x.x.x
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// ms
        /// </summary>
        public int Timeout { get; set; }
    }
}
