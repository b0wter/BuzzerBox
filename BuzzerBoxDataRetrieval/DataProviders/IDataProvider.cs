using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuzzerBoxDataRetrieval.DataProviders
{
    /// <summary>
    /// Interface for providers that offer the option to retrieve data from arbitrary sources.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDataProvider<T>
    {
        Task<List<T>> LoadItems();
        Task<T> LoadItem(int id);
    }
}