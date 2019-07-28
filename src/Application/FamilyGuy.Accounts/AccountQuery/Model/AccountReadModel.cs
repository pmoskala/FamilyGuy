using System;

namespace FamilyGuy.Accounts.AccountQuery.Model
{
    public class AccountReadModel
    {
        public Guid Id { get; set; }
        public string LoginName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}