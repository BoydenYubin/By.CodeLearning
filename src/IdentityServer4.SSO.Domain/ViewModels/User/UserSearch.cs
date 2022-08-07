using ByLearning.SSO.Domain.Interfaces;

namespace ByLearning.SSO.Domain.ViewModels.User
{
    public class UserSearch<TKey> : IUserSearch
    {
        public TKey[] Id { get; set; }
        public string Sort { get; set; }
        public int? Limit { get; set; }
        public int? Offset { get; set; }
    }
}