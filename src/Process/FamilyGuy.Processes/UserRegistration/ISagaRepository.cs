using System;
using System.Threading.Tasks;

namespace FamilyGuy.Processes.UserRegistration
{
    public interface ISagaRepository
    {
        Task Save(Guid id, ISagaData data);
        Task<T> Get<T>(Guid id) where T : class;
        Task<T> Get<T>(Func<T, bool> condition) where T : class;
    }
}