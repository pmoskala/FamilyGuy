using System;

namespace FamilyGuy.UserApi.Model
{
    public class AccountPostModel
    {
        public Guid Id { get; set; }
        public string LoginName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string TelephoneNumber { get; set; }
    }
}