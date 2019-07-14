using System;

namespace FamilyGuy.Accounts.Domain
{
    public class User
    {
        public Guid Id { get; }
        public string UserName { get; }
        public string Name { get; }
        public string Surname { get; }
        public string Email { get; }
        public string PasswordHash { get; }

        public User(Guid id, string userName, string name, string surname, string email, string password)
        {
            Id = id;
            UserName = userName;
            Name = name;
            Surname = surname;
            Email = email;
            PasswordHash = password;
        }
    }
}
