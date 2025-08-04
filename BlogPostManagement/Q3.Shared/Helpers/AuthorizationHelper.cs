using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q3.Shared.Helpers
{
    public static class AuthorizationHelper
    {
        public static bool CanDeleteOrUpdateBlog(string? currentUsername, string? currentUserRole, string blogAuthor)
        {
            if (string.IsNullOrEmpty(currentUsername) || string.IsNullOrEmpty(currentUserRole))
                return false;

            return currentUserRole == "Admin" || blogAuthor == currentUsername;
        }
    }
}
