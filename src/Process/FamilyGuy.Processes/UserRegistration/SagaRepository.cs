using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyGuy.Processes.UserRegistration
{
    internal class SagaRepository : ISagaRepository
    {
        private static readonly ConcurrentDictionary<Guid, ISagaData> Items = new ConcurrentDictionary<Guid, ISagaData>();

        public async Task Save(Guid id, ISagaData data)
        {
            await Task.Run(() => Items[id] = data);
        }

        public async Task<T> Get<T>(Guid id) where T : class
        {
            if (!Items.ContainsKey(id))
                return null;

            ISagaData data = Items[id];
            return await Task.FromResult((T)data);
        }

        public async Task<T> Get<T>(Func<T, bool> condition) where T : class
        {
            IEnumerable<T> ofType = Items.Values.OfType<T>();
            IEnumerable<T> enumerable = ofType.Where(condition);
            T firstOrDefault = enumerable.FirstOrDefault();
            return await Task.FromResult(firstOrDefault);
        }
    }
}