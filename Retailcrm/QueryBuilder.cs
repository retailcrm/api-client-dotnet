using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Retailcrm
{
    /// <summary>
    /// QueryBuilder
    /// </summary>
    public class QueryBuilder
    {
        private readonly List<KeyValuePair<string, object>> _keyValuePairs
            = new List<KeyValuePair<string, object>>();

        /// <summary>
        /// Build PHP like query string
        /// </summary>
        /// <param name="queryData"></param>
        /// <param name="argSeperator"></param>
        /// <returns></returns>
        public static string BuildQueryString(object queryData, string argSeperator = "&")
        {
            var encoder = new QueryBuilder();
            encoder.AddEntry(null, queryData, allowObjects: true);

            return encoder.GetUriString(argSeperator);
        }

        /// <summary>
        /// GetUriString
        /// </summary>
        /// <param name="argSeperator"></param>
        /// <returns></returns>
        private string GetUriString(string argSeperator)
        {
            return String.Join(argSeperator,
                _keyValuePairs.Select(kvp =>
                {
                    var key = HttpUtility.UrlEncode(kvp.Key);
                    var value = HttpUtility.UrlEncode(kvp.Value.ToString());
                    return $"{key}={value}";
                }));
        }

        /// <summary>
        /// AddEntry
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="instance"></param>
        /// <param name="allowObjects"></param>
        private void AddEntry(string prefix, object instance, bool allowObjects)
        {
            var collection = instance as ICollection;

            if (instance is IDictionary dictionary)
            {
                Add(prefix, GetDictionaryAdapter(dictionary));
            }
            else if (collection != null)
            {
                Add(prefix, GetArrayAdapter(collection));
            }
            else if (allowObjects)
            {
                Add(prefix, GetObjectAdapter(instance));
            }
            else
            {
                _keyValuePairs.Add(new KeyValuePair<string, object>(prefix, instance));
            }
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="datas"></param>
        private void Add(string prefix, IEnumerable<Entry> datas)
        {
            foreach (var item in datas)
            {
                var newPrefix = String.IsNullOrEmpty(prefix)
                    ? item.Key
                    : $"{prefix}[{item.Key}]";

                AddEntry(newPrefix, item.Value, allowObjects: false);
            }
        }

        /// <summary>
        /// Entry
        /// </summary>
        private struct Entry
        {
            public string Key;
            public object Value;
        }

        /// <summary>
        /// GetObjectAdapter
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static IEnumerable<Entry> GetObjectAdapter(object data)
        {
            var properties = data.GetType().GetProperties();

            foreach (var property in properties)
            {
                yield return new Entry
                {
                    Key = property.Name,
                    Value = property.GetValue(data)
                };
            }
        }

        /// <summary>
        /// GetArrayAdapter
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        private static IEnumerable<Entry> GetArrayAdapter(ICollection collection)
        {
            int i = 0;
            foreach (var item in collection)
            {
                yield return new Entry
                {
                    Key = i.ToString(),
                    Value = item
                };
                i++;
            }
        }

        /// <summary>
        /// GetDictionaryAdapter
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        private static IEnumerable<Entry> GetDictionaryAdapter(IDictionary collection)
        {
            foreach (DictionaryEntry item in collection)
            {
                yield return new Entry
                {
                    Key = item.Key.ToString(),
                    Value = item.Value
                };
            }
        }
    }
}