using System.Collections.Generic;
using System.Threading.Tasks;
using FamilyGuy.Infrastructure.PasswordHasher;
using FluentAssertions;
using Xunit;

namespace FamilyGuy.Account.Test
{
    [Collection("PasswordHasherTests")]
    public class PasswordHasherTests
    {
        [Fact]
        public async Task GetSalt()
        {
            PasswordHasher passwordHasher = new PasswordHasher();

            List<string> salts = await Salts(passwordHasher);
            salts.Should().OnlyHaveUniqueItems();
        }

        [Fact]
        public async Task HashPassword()
        {
            PasswordHasher passwordHasher = new PasswordHasher();
            List<string> hashes = new List<string>();
            List<string> salts = await Salts(passwordHasher);
            string password = "P@ssw0rd123!";
            foreach (string salt in salts)
            {
                hashes.Add(await passwordHasher.HashString(password, salt));
            }

            hashes.Should().OnlyHaveUniqueItems();
        }

        [Fact]
        public async Task CheckHash()
        {
            PasswordHasher passwordHasher = new PasswordHasher();
            Dictionary<string,string> hashes = new Dictionary<string, string>();
            List<string> salts = await Salts(passwordHasher);
            const string password = "P@ssw0rd123!";
            foreach (string salt in salts)
            {
                string hashString = await passwordHasher.HashString(password, salt);
                hashes.Add(salt, hashString);
            }

            foreach (KeyValuePair<string,string> pair in hashes)
            {
                bool checkHash = await passwordHasher.CheckHash(password,pair.Value, pair.Key);
                checkHash.Should().Be(true);
            }
        }

        private static async Task<List<string>> Salts(PasswordHasher passwordHasher)
        {
            List<string> salts = new List<string>();
            for (int i = 0; i < 1_000; i++)
            {
                salts.Add(await passwordHasher.GetSalt());
            }

            return salts;
        }
    }
}