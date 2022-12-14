namespace ByProject.AutoUpgrade
{
    public class UpgradeResult
    {
        public UpgradeStatus Status { get; set; }
        public string Message { get; set; }
    }

    public enum UpgradeStatus
    {
        UpgradeSuccess = 0,
        UpgradeFailed = 1,
        VersionLatest = 2,
        UnzipFailed = 3,
        EmptyUpgrade = 4,
        ServerError = 5,
        UpgradeTimeout = 6
    }
}
