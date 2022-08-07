using System.ComponentModel;

namespace ByLearning.SSO.Domain.ViewModels.Settings
{
    public enum LoginStrategyType
    {
        [Description("ASP.NET Identity")]
        Identity = 1,
        [Description("LDAP")]
        Ldap = 2,

    }
}