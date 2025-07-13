using MJRecords.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Repository
{
    public interface ILoginRepo
    {
        /// <summary>
        /// Authenticates an employee using the provided login credentials.
        /// </summary>
        /// <param name="loginDTO">The login credentials.</param>
        /// <returns>The authenticated <see cref="Employee"/> if credentials are valid; otherwise, null.</returns>
        Employee Login(LoginDTO loginDTO);

        /// <summary>
        /// Asynchronously authenticates an employee using the provided login credentials.
        /// </summary>
        /// <param name="loginDTO">The login credentials.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the authenticated <see cref="Employee"/> if credentials are valid; otherwise, null.</returns>
        Task<Employee> LoginAsync(LoginDTO loginDTO);

        /// <summary>
        /// Retrieves the password salt for a specific employee.
        /// </summary>
        /// <param name="id">The ID of the employee.</param>
        /// <returns>A byte array representing the employee's salt.</returns>
        byte[] GetEmployeeSalt(string id);

        /// <summary>
        /// Asynchronously retrieves the password salt for a specific employee.
        /// </summary>
        /// <param name="id">The ID of the employee.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a byte array representing the employee's salt.</returns>
        Task<byte[]> GetEmployeeSaltAsync(string id);

    }
}
