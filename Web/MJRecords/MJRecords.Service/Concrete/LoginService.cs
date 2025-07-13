using MJRecords.Model;
using MJRecords.Repository;
using MJRecords.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Service
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepo _repo;
        private readonly IEmployeeService _employeeService;

        public LoginService(ILoginRepo repo, IEmployeeService employeeService)
        {
            _repo = repo;
            _employeeService = employeeService;
        }

        public byte[] GetEmployeeSalt(string id)
        {
            return _repo.GetEmployeeSalt(id);
        }

        public async Task<byte[]> GetEmployeeSaltAsync(string id)
        {
            return await _repo.GetEmployeeSaltAsync(id);
        }

        public LoginOutputDTO Login(LoginDTO loginDTO)
        {
            if(loginDTO.Username == null)
            {
                return null;
            }

            byte[] salt = GetEmployeeSalt(loginDTO.Username);
            if (salt == null)
            {
                return null;
            }
            loginDTO.Password = PasswordUtility.HashToSHA256(loginDTO.Password, salt);
            Employee emp = _repo.Login(loginDTO);

            if (emp == null)
            {
                return null;
            }

            AccessLevels role = _employeeService.GetEmployeeAccessLevel(emp.Id);

            LoginOutputDTO login = new()
            {
                Id = emp.Id,
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                MiddleInitial = emp.MiddleInitial,
                Role = role.GetDescription(),
                DepartmentId = emp.DepartmentId ?? 0

            };

            return login;
        }

        public async Task<LoginOutputDTO> LoginAsync(LoginDTO loginDTO)
        {
            if (loginDTO.Username == null)
            {
                return null;
            }

            byte[] salt = await GetEmployeeSaltAsync(loginDTO.Username);
            if (salt == null)
            {
                return null;
            }
            loginDTO.Password = PasswordUtility.HashToSHA256(loginDTO.Password, salt);
            Employee emp = await _repo.LoginAsync(loginDTO);

            if(emp == null)
            {
                return null;
            }
            AccessLevels role = _employeeService.GetEmployeeAccessLevel(emp.Id);

            LoginOutputDTO login = new()
            {
                Id = emp.Id,
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                MiddleInitial = emp.MiddleInitial,
                Role = role.GetDescription(),
                DepartmentId = emp.DepartmentId ?? 0
            };

            return login;
        }
    }
}
