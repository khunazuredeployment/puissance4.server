using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Puissance4.API.Extensions
{
    public static class UserExtensions
    {
        public static int GetId(this HubCallerContext context)
        {
            string? idString = context?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (idString == null)
            {
                throw new Exception();
            }
            return int.Parse(idString);
        }
    }
}
