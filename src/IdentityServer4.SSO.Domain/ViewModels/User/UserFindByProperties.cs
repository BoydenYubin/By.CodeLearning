using AspNetCore.IQueryable.Extensions.Pagination;
using AspNetCore.IQueryable.Extensions.Sort;

namespace ByLearning.SSO.Domain.ViewModels.User
{
    public class UserFindByProperties : IQuerySort, IQueryPaging
    {
        public string Query { get; }

        public UserFindByProperties(string query)
        {
            Query = query;
        }
        public string Sort { get; set; }
        public int? Limit { get; set; }
        public int? Offset { get; set; }
    }
}
