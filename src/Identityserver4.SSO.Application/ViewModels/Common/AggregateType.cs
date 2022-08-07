namespace Identityserver4.SSO.Application.ViewModels.Common
{
    /// <summary>
    /// Types of aggregate
    /// </summary>
    public enum AggregateType
    {
        Client,
        ApiResource,
        IdentityResource,
        ProtectedGrant,
        Users,
        Roles,
        Email,
        GlobalSettings
    }
}
