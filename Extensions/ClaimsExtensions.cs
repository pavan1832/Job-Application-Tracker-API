using System.Security.Claims;

namespace JobTrackerAPI.Extensions;

/// <summary>
/// Extension methods for extracting typed values from ClaimsPrincipal.
/// </summary>
public static class ClaimsExtensions
{
    /// <summary>
    /// Extracts the authenticated user's ID from the JWT subject claim.
    /// Throws if the claim is missing or invalid.
    /// </summary>
    public static int GetUserId(this ClaimsPrincipal user)
    {
        var claim = user.FindFirstValue(ClaimTypes.NameIdentifier)
                 ?? user.FindFirstValue("sub");

        if (string.IsNullOrEmpty(claim) || !int.TryParse(claim, out var id))
            throw new UnauthorizedAccessException("Unable to identify the authenticated user.");

        return id;
    }
}
