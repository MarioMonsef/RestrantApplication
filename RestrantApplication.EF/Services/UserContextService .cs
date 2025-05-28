using Microsoft.AspNetCore.Http;
using RestrantApplication.Core.Services;
using System.Security.Claims;

namespace RestrantApplication.EF.Services
{
    /// <summary>
    /// Service to retrieve information about the currently authenticated user.
    /// </summary>
    public class UserContextService : IUserContextService
    {
        #region Fields

        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserContextService"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">Provides access to the current HTTP context.</param>
        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Handle Functions

        /// <summary>
        /// Retrieves the current user's ID from the HTTP context claims.
        /// </summary>
        /// <returns>The user ID as a string, or null if not found.</returns>
        public string GetCurrentUserId()
            => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        // Gets the unique identifier (usually user ID) from claims

        /// <summary>
        /// Retrieves the current user's role from the HTTP context claims.
        /// </summary>
        /// <returns>The user's role as a string, or null if not found.</returns>
        public string GetRoleCurrentUser()
            => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        // Gets the user's assigned role from claims (e.g., Admin, User)

        #endregion
    }
}
