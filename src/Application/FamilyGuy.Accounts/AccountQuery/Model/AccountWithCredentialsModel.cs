using System;

namespace FamilyGuy.Accounts.AccountQuery.Model
{
    public class AccountWithCredentialsModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
    }
}
