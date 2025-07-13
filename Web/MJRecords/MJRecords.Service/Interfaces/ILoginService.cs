using MJRecords.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Service
{
    public interface ILoginService
    {
        /// <summary>
        /// Authenticates a user based on the provided login credentials.
        /// </summary>
        /// <param name="loginDTO">A <see cref="LoginDTO"/> containing the user's login credentials.</param>
        /// <returns>
        /// A <see cref="LoginOutputDTO"/> containing the authentication result, such as user details and access token.
        /// </returns>
        LoginOutputDTO Login(LoginDTO loginDTO);

        /// <summary>
        /// Asynchronously authenticates a user using the provided login credentials.
        /// </summary>
        /// <param name="loginDTO">A <see cref="LoginDTO"/> containing the user's login information.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing a <see cref="LoginOutputDTO"/> 
        /// with the authentication result, such as user details and access token.
        /// </returns>
        Task<LoginOutputDTO> LoginAsync(LoginDTO loginDTO);

        /// <summary>
        /// Retrieves the cryptographic salt associated with a specific employee's password.
        /// </summary>
        /// <param name="id">The ID of the employee.</param>
        /// <returns>The salt as a byte array.</returns>
        byte[] GetEmployeeSalt(string id);

        /// <summary>
        /// Asynchronously retrieves the cryptographic salt associated with a specific employee's password.
        /// </summary>
        /// <param name="id">The ID of the employee.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing the salt as a byte array.
        /// </returns>
        Task<byte[]> GetEmployeeSaltAsync(string id);
    }
}
