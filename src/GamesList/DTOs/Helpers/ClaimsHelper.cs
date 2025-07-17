using System.Security.Claims;

namespace GamesList.Dtos.Helpers
{
    public class ClaimsHelper
    {
        public static int? GetUserId(ClaimsPrincipal user)
        {
            var value = user.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(value, out var id) ? id : null;
        }
        public static string? GetUserName(ClaimsPrincipal user)
        {
            return user.Identity?.Name;
        }
         public static string? GetUserRole(ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.Role);
        }
    }
}