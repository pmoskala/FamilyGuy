using FamilyGuy.Accounts.AccountExceptions;
using FamilyGuy.Accounts.AccountQuery.Model;
using FamilyGuy.Contracts.Communication.Interfaces;
using FamilyGuy.Infrastructure.DI;
using FamilyGuy.Processes.UserRegistration;
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
            AccountByUserNameQuery accountByUserNameQuery = new AccountByUserNameQuery
            {
                UserName = userName,
            };

            AccountWithCredentialsModel account = await GetUser(accountByUserNameQuery);
            await AssertPassword(password, account);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = CreateToken(account, tokenHandler);

            return new AuthenticatedUserReadModel
            {
                Token = tokenHandler.WriteToken(token),
                FirstName = account.Name,
                LastName = account.Surname,
                Id = account.Id,
                UserName = userName
            };
        }

        private JwtSecurityToken CreateToken(AccountWithCredentialsModel account,
            JwtSecurityTokenHandler tokenHandler)
        {
            byte[] key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, account.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            return tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
        }

        private async Task AssertPassword(string password, AccountWithCredentialsModel account)
        {
            if (!await _passwordHasher.CheckHash(password, account.PasswordHash, account.PasswordSalt))
                throw new UnauthorizedAccessException();
        }

        private async Task<AccountWithCredentialsModel> GetUser(AccountByUserNameQuery accountByUserNameQuery)
        {
            try
            {
                return await _query.Query<Task<AccountWithCredentialsModel>, AccountByUserNameQuery>(
                    accountByUserNameQuery);

            }
            catch (UserNotFoundFgException)
            {
                throw new UnauthorizedAccessException();
            }
        }
    }

    public class AuthenticatedUserReadModel
    {
        public string Token { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public Guid Id { get; set; }
    }
}