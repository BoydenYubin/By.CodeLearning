using AspNetCore.IQueryable.Extensions.Pagination;
using AspNetCore.IQueryable.Extensions.Sort;
using ByLearning.Admin.Application.Interfaces;

namespace ByLearning.Admin.Application.ViewModels
{
    public class PersistedGrantSearch : IQueryPaging, IQuerySort, IPersistedGrantCustomSearch
    {
        public int? Limit { get; set; }
        public int? Offset { get; set; }
        public string Sort { get; set; }
    }
}