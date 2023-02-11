using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MeetMe.Core.Interface.Caching
{
    public interface ICacheService
    {
        T GetData<T>(string key);
        void Remove(string key);
        Task<T> GetOrAdd<T>(string key, Func<Task<T>> action, int aliveForSeconds = 15);
        void SetData<T>(string key, T value, int aliveForSeconds = 30);
    }

}
