using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Types
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Retrieves the description attribute of an enum value, if present; otherwise, returns the enum name as a string.
        /// </summary>
        /// <param name="value">The enum value.</param>
        /// <returns>
        /// The description specified by the <see cref="DescriptionAttribute"/>, 
        /// or the enum's name if no description is found.
        /// </returns>
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attr = field?.GetCustomAttribute<DescriptionAttribute>();
            return attr?.Description ?? value.ToString();
        }
    }
}
