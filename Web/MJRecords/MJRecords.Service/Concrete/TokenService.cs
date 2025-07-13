using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MJRecords.Model;
using MJRecords.Types;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IEmployeeService _employeeService;

        public TokenService(IConfiguration configuration, IEmployeeService employeeService)
        {
            _configuration = configuration;
            _employeeService = employeeService;
        }

        public string CreateToken(LoginOutputDTO employee)
        {
            // Check if Jwt:Key is null or empty
            string? jwtKey = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new InvalidOperationException("Jwt:Key is not configured.");
            }

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            //AccessLevels empAccessLevel = _employeeService.GetEmployeeAccessLevel(employee.Id);

            //List of claims we will store in the token
            List<Claim> claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, employee.Id ?? ""),
                new Claim(ClaimTypes.Role, employee.Role ?? "")
            };

            //Create new credentials for signing the token
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            //Describe the token. What goes inside
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            //Create a new token handler
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            //Create the token
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            //Write the token and return
            return tokenHandler.WriteToken(token);
        }
    }
}
