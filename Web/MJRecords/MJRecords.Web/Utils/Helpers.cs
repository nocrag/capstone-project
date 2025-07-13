using Humanizer;
using Microsoft.CodeAnalysis;
using MJRecords.Types;
using System.Data;
using System.Linq;
using System.Runtime.Intrinsics.X86;

namespace MJRecords.Web
{

    public static class RoleHelper
    {
        private static readonly HashSet<AccessLevels> AllowedRoles = new()
        {
            AccessLevels.CEO,
            AccessLevels.HRSupervisor,
            AccessLevels.RegularSupervisor,
            AccessLevels.HREmployee,
            AccessLevels.RegularEmployee
        };

        /// <summary>
        /// Determines whether a user with the given role is authorized based on one or more allowed access levels.
        /// </summary>
        /// <param name="userRole">The user's role as a string.</param>
        /// <param name="targetRoles">
        /// An optional list of <see cref="AccessLevels"/> values to check against. 
        /// If not provided, the method uses a predefined list of allowed roles.
        /// </param>
        /// <returns>
        /// <c>true</c> if the user's role matches one of the allowed roles; otherwise, <c>false</c>.
        /// </returns>

        public static bool IsAllowed(string userRole, params AccessLevels[] targetRoles)
        {
            if (string.IsNullOrEmpty(userRole))
            {
                return false;
            }

            if (Enum.TryParse<AccessLevels>(userRole.Replace(" ", ""), out AccessLevels parsedRole))
            {
                if (targetRoles != null && targetRoles.Length > 0)
                {
                    return targetRoles.Contains(parsedRole);
                }

                return AllowedRoles.Contains(parsedRole);
            }

            return false;
        }
    }
}
