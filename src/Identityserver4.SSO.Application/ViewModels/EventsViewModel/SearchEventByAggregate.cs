using AspNetCore.IQueryable.Extensions.Attributes;
using AspNetCore.IQueryable.Extensions.Filter;
using AspNetCore.IQueryable.Extensions.Pagination;
using AspNetCore.IQueryable.Extensions.Sort;
using Identityserver4.SSO.Application.ViewModels.Common;

namespace ByLearning.SSO.Application.ViewModels.EventsViewModel
{
    public class SearchEventByAggregate : ICustomEventQueryable, IQuerySort, IQueryPaging
    {
        [QueryOperator(HasName = "AggregateId", Operator = WhereOperator.Equals)]
        public string Aggregate { get; set; }

        public string Sort { get; set; }

        [QueryOperator(Max = 100)]
        public int? Limit { get; set; }
        public int? Offset { get; set; }
    }
}