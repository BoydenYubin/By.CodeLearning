using ByLearning.SSO.Application.ViewModels.UserViewModels;
using System.Collections.Generic;
using System.Linq;

namespace ByLearning.SSO.Application.Extensions
{
    public static class UserListViewModelExtensions
    {
        public static UserListViewModel WithId(this IEnumerable<UserListViewModel> users, string subjectId) => users.FirstOrDefault(f => f.Id.Equals(subjectId));

    }
}
