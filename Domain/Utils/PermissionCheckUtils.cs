using Domain.Agregates.UserAgregate;
using Domain.Exceptions;
using System;

namespace Domain.Utils
{
    public static class PermissionCheckUtils
    {
        private const string DEFAULT_LOW_PREVILEGIES_LEVEL_ERROR = "Your access level too low";

        public static Guid DoesUserHavePermission(User user, UserRoleType userRole)
        {
            EnsuredUtils.EnsureNotNull(user);

            if (user.Role.CompareTo(userRole) < 0)
            {
                throw new LowPrevilegiesLevelException(DEFAULT_LOW_PREVILEGIES_LEVEL_ERROR);
            }

            return default;
        }
    }
}
