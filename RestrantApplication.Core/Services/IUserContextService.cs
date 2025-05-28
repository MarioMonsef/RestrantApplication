using System;

namespace RestrantApplication.Core.Services
{
    public interface IUserContextService
    {
        /// <summary>
        /// Gets the ID of the currently authenticated user.
        /// </summary>
        /// <returns>The current user's ID as a string.</returns>
        string GetCurrentUserId();

        /// <summary>
        /// Gets the role of the currently authenticated user.
        /// </summary>
        /// <returns>The current user's role as a string.</returns>
        string GetRoleCurrentUser();
    }
}
