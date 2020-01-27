using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace Darixidor.Blazor.Settings.Extentions
{
    internal static class ConfigurationExtentions
    {
        public static ExpandoObject GetSettingsObject(this IConfigurationRoot configurationRoot, string section)
        {
            var result = new ExpandoObject();
            // retrieve all keys from your settings
            var configs = configurationRoot.GetSection(section).AsEnumerable().Where(_ => _.Key.StartsWith(section));
            foreach (var kvp in configs)
            {
                var parent = result as IDictionary<string, object>;
                var path = kvp.Key.Split(':');

                // create or retrieve the hierarchy (keep last path item for later)
                var i = 0;
                for (i = 0; i < path.Length - 1; i++)
                {
                    var nodeName = path[i] == section ? "data" : path[i];
                    if (!parent.ContainsKey(nodeName))
                    {

                        parent.Add(nodeName, new ExpandoObject());
                    }

                    parent = parent[nodeName] as IDictionary<string, object>;
                }

                if (kvp.Value == null)
                    continue;

                // add the value to the parent
                // note: in case of an array, key will be an integer and will be dealt with later
                var key = path[i];
                parent.Add(key, kvp.Value);
            }

            // at this stage, all arrays are seen as dictionaries with integer keys
            ReplaceWithArray(null, null, result);
            return result;
        }

        private static void ReplaceWithArray(ExpandoObject parent, string key, ExpandoObject input)
        {
            if (input == null)
                return;

            var dict = input as IDictionary<string, object>;
            var keys = dict.Keys.ToArray();

            // it's an array if all keys are integers
            if (keys.All(k => int.TryParse(k, out var dummy)))
            {
                var array = new object[keys.Length];
                foreach (var kvp in dict)
                {
                    array[int.Parse(kvp.Key)] = kvp.Value;
                }

                var parentDict = parent as IDictionary<string, object>;
                parentDict.Remove(key);
                parentDict.Add(key, array);
            }
            else
            {
                foreach (var childKey in dict.Keys.ToList())
                {
                    ReplaceWithArray(input, childKey, dict[childKey] as ExpandoObject);
                }
            }
        }
    }
}
