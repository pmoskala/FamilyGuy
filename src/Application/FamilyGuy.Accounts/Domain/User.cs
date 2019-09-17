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
            SetUsername(userName);
            SetName(name);
            SetSurname(surname);
            SetEmail(email);
            SetPassword(password, passwordSalt);
            SetTelephoneNumber(telephoneNumber);
        }

        public void SetPassword(string passwordHash, string passwordSalt)
        {
            if(string.IsNullOrWhiteSpace(passwordSalt) || string.IsNullOrWhiteSpace(passwordHash))
                throw  new ArgumentNullException(passwordHash); // todo: throw domain exception  
            
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
        }

        public void SetUsername(string userName) => UserName = userName;
        public void SetName(string name) => Name = name;
        public void SetSurname(string surname) => Surname = surname;
        public void SetEmail(string email) => Email = email;
        public void SetTelephoneNumber(string telephoneNumber) => TelephoneNumber = telephoneNumber;

    }
}
