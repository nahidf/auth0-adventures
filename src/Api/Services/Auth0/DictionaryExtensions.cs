using System.Collections.Generic;
using System.Linq;

namespace Api.Services.Auth0
{
    internal static class DictionaryExtensions
    {
        public static bool IsEmpty(this IDictionary<string, string> dic)
        {
            return dic == null || dic.Count == 0 || dic.All(c => string.IsNullOrWhiteSpace(c.Key)); 
        }

        public static string GetSearchQuery(this IDictionary<string, string> dic)
        {
            if (dic.IsEmpty())
                return null;

            var query = "";
            foreach (var criteria in dic)
            {
                if (string.IsNullOrWhiteSpace(criteria.Key))
                    continue;

                if (!string.IsNullOrWhiteSpace(query))
                    query += " and ";

                query += string.Format("{0}:\"{1}\"", criteria.Key, criteria.Value);
            }

            return query;
        }
    }
}
