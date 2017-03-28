using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BuzzerBoxDataRetrieval.DataProviders
{
    /// <summary>
    /// Interface for converters that create objects from strings.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IStringDataConverter<T>
    {
        List<T> ParseItems(string json);
        T ParseItem(string json);
    }
}