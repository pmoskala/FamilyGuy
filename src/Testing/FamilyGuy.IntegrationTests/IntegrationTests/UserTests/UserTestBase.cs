using System;
using FamilyGuy.UserApi.Model;

namespace FamilyGuy.IntegrationTests.IntegrationTests.UserTests
{
    public abstract class UserTestBase : TestBase
    {
        protected static PostAccountModel CreatePostAccountModel(Guid userId)
        {
            return new PostAccountModel
            {
                Id = userId,
                LoginName = Guid.NewGuid().ToString(),
                Name = "Ala",
                Surname = "Kociak",
                Password = Guid.NewGuid().ToString(),
                TelephoneNumber = "555-123-321",
                Email = "ala.ma.kotowska@gmail.com"
            };
        }
    }
}