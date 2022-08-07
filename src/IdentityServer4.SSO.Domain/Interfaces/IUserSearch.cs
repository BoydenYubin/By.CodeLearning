using AspNetCore.IQueryable.Extensions;
using AspNetCore.IQueryable.Extensions.Pagination;
using AspNetCore.IQueryable.Extensions.Sort;

namespace ByLearning.SSO.Domain.Interfaces
{
    public interface IUserSearch : ICustomQueryable, IQuerySort, IQueryPaging
    {
    }

}
