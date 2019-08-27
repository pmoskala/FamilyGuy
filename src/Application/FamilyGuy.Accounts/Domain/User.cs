using System;

namespace FamilyGuy.Accounts.Domain
{
    public class User
    {
        public Guid Id { get; protected set; }
        public string UserName { get; protected set; }
        public string Name { get; protected set; }
        public string Surname { get; protected set; }
        public string Email { get; protected set; }
        public string PasswordHash { get; protected set; }
        public string PasswordSalt { get; protected set; }
        public string TelephoneNumber { get; protected set; }

        public User()
        {

        }

        public User(Guid id, string userName, string name, string surname, string email, string password, string passwordSalt, string telephoneNumber)
        {
            Id = id;
            UserName = userName;
            Name = name;
            Surname = surname;
            Email = email;
            PasswordHash = password;
            PasswordSalt = passwordSalt;
            TelephoneNumber = telephoneNumber;
        }
    }
}
