using ByLearning.SSO.Domain.Interfaces;

namespace ByLearning.SSO.Domain.Models
{
    public class Role : IRole
    {
        public Role(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; }
        public string Name { get; }
    }
}
