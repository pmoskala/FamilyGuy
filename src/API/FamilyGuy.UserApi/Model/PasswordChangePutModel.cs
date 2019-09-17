using System;

namespace FamilyGuy.UserApi.Model
{
    public class PasswordChangePutModel
    {
        public string NewPassword { get; set; }
        public Guid  UserId { get; set; }
    }
}