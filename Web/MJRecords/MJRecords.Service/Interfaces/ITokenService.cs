using MJRecords.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Service
{
    public interface ITokenService
    {

        /// <summary>
        /// Creates a JWT token for the specified authenticated employee.
        /// </summary>
        /// <param name="employee">The authenticated employee as a <see cref="LoginOutputDTO"/>.</param>
        /// <returns>
        /// A JWT token string that can be used for authorization in subsequent requests.
        /// </returns>
        string CreateToken(LoginOutputDTO employee);
    }
}
