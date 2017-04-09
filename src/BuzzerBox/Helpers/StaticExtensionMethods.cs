using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BuzzerBox.Helpers
{
    public class StaticExtensionMethods
    {
        public static ExpandoObject CreateExpandoFromObject(object source)
        {
            var result = new ExpandoObject();
            IDictionary<string, object> dictionary = result;
            foreach (var property in source
                .GetType()
                .GetProperties()
                .Where(p => p.CanRead && p.GetMethod.IsPublic))
            {
                dictionary[property.Name] = property.GetValue(source, null);
            }
            return result;
        }
    }
}
