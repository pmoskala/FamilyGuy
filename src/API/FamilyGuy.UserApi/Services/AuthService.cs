using FamilyGuy.Accounts.AccountQuery.Model;
using FamilyGuy.Contracts.Communication.Interfaces;
using FamilyGuy.Infrastructure.DI;
using FamilyGuy.Processes.UserRegistration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

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
            AccountByUserNameQuery accountByUserNameQuery = new AccountByUserNameQuery
            {
                UserName = userName,
            };

            AccountWithCredentialsModel account = await _query.Query<Task<AccountWithCredentialsModel>, AccountByUserNameQuery>(accountByUserNameQuery);
            if (account == null) return null;

            if (!await _passwordHasher.CheckHash(password, account.PasswordHash, account.PasswordSalt))
                return null; // todo throw exception

            byte[] key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, account.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
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