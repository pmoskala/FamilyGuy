using FamilyGuy.Accounts.AccountQuery.Model;
using FamilyGuy.Contracts.Communication.Interfaces;
using FamilyGuy.Processes.UserRegistration;
using FamilyGuy.Settings;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FamilyGuy.UserApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IQuery _query;
        private readonly IPasswordHasher _passwordHasher;
        private readonly JwtSettings _jwtSettings;

        public AuthService(IQuery query, IPasswordHasher passwordHasher, JwtSettings jwtSettings)
        {
            _query = query;
            _passwordHasher = passwordHasher;
            _jwtSettings = jwtSettings;
        }

        public async Task<AuthenticatedUserReadModel> Authenticate(string userName, string password)
        {
            UserAuthenticationModel userAuthenticationModel = new UserAuthenticationModel
            {
                UserName = userName,
                PasswordHash = _passwordHasher.Hash(password)
            };

            AccountReadModel account = await _query.Query<Task<AccountReadModel>, UserAuthenticationModel>(userAuthenticationModel);
            if (account == null) return null;

            byte[] key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, account.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            return new AuthenticatedUserReadModel
            {
                Token = tokenHandler.WriteToken(token),
                Name = account.Name,
                Surname = account.Surname
            };
        }
    }

    public class AuthenticatedUserReadModel
    {
        public string Token { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}