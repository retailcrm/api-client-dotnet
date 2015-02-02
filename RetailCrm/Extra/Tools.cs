using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailCrm.Extra
{
    class Tools
    {
        public static string httpBuildQuery(Dictionary<string, object> data)
        {
            if (data is Dictionary<string, object> == false)
            {
                return String.Empty;
            }

            var parts = new List<string>();
            HandleItem(data, parts);
            return String.Join("&", parts);
        }

        private static void HandleItem(object data, List<string> parts, string prefix = "")
        {
            if (data == null) return;

            if (data is Dictionary<string, object>)
            {
                parts.Add(FormatDictionary((Dictionary<string, object>)data, prefix));
            }
            else
            {
                parts.Add(String.IsNullOrEmpty(data.ToString()) ? String.Empty : String.Format("{0}={1}", prefix, data.ToString()));
            }
        }

        private static string FormatDictionary(Dictionary<string, object> obj, string prefix = "")
        {
            var parts = new List<string>();
            foreach (KeyValuePair<string, object> kvp in obj)
            {
                string newPrefix = string.IsNullOrEmpty(prefix) ?
                    String.Format("{0}{1}", prefix, kvp.Key) :
                    String.Format("{0}[{1}]", prefix, kvp.Key);
                    HandleItem(kvp.Value, parts, newPrefix);
            }

            return String.Join("&", parts);
        }

        public static Dictionary<string, object> jsonDecode(string json)
        {
            return jsonObjectToDictionary((Dictionary<string, object>)JsonConvert.DeserializeObject<Dictionary<string, object>>(json));
        }

        private static Dictionary<string, object> jsonObjectToDictionary(Dictionary<string, object> data)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            foreach (KeyValuePair<string, object> kvp in data)
            {
                System.Console.WriteLine(kvp.Key.ToString());
                System.Console.WriteLine(kvp.Value.ToString());
                System.Console.ReadLine();
                object valueObj = kvp.Value;
                string value = String.Empty;

                if (valueObj.GetType() == typeof(JArray))
                {
                    string tmpValue = JsonConvert.SerializeObject(((JArray)valueObj).ToArray<dynamic>());
                    char[] charsToTrim = { '[', ' ', ']'};
                    value = tmpValue.Trim(charsToTrim);
                }
                else
                {
                    value = valueObj.ToString();
                }

                if (value != "")
                {
                    if (valueObj.GetType() == typeof(JObject) || valueObj.GetType() == typeof(JArray))
                    {
                        valueObj = jsonObjectToDictionary((Dictionary<string, object>)JsonConvert.DeserializeObject<Dictionary<string, object>>(value));
                    }
                    result.Add(kvp.Key.ToString(), valueObj);
                }
            }
            return result;
        }
    }
}
