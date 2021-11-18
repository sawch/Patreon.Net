using System;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Reflection;
using System.Linq;

namespace Patreon.Net
{
    internal static class UrlHelper
    {
        private static readonly Dictionary<Includes, IncludeAttribute> includeMap;
        private static readonly Dictionary<Type, string> resourceFieldsMap;

        static UrlHelper()
        {
            // Generate cache of Includes -> Include attribute field names
            var individualIncludes = typeof(Includes).GetFields();
            includeMap = new Dictionary<Includes, IncludeAttribute>(individualIncludes.Length);
            for (int i = 0; i < individualIncludes.Length; i++)
            {
                var include = individualIncludes[i];
                var includeNameAttrib = include.GetCustomAttribute<IncludeAttribute>();
                if (includeNameAttrib != null)
                    includeMap.Add((Includes)include.GetRawConstantValue(), includeNameAttrib);
            }

            // Generate cache of resource field strings
            resourceFieldsMap = new Dictionary<Type, string>()
            {
                { typeof(Models.Address), GenerateFields(typeof(Models.Address)) },
                { typeof(Models.Benefit), GenerateFields(typeof(Models.Benefit)) },
                { typeof(Models.Campaign), GenerateFields(typeof(Models.Campaign)) },
                { typeof(Models.Goal), GenerateFields(typeof(Models.Goal)) },
                { typeof(Models.Member), GenerateFields(typeof(Models.Member)) },
                { typeof(Models.Tier), GenerateFields(typeof(Models.Tier)) },
                { typeof(Models.User), GenerateFields(typeof(Models.User)) }
            };
        }

        public static string Generate(Type resource, Includes includes)
        {
            if (!resourceFieldsMap.TryGetValue(resource, out string fields))
                throw new ArgumentException("Unsupported resource type: " + resource.Name, nameof(resource));

            StringBuilder stringBuilder = new StringBuilder(fields.Length);
            if (includes > 0)
            {
                stringBuilder.Append("include=")
                    .AppendJoin(',', includeMap.Where((i) => (i.Key & includes) > 0).Select((i) => i.Value.includeName))
                    .Append('&');

                stringBuilder.AppendJoin('&', includeMap.Where((i) => (i.Key & includes) > 0).Select((i) => resourceFieldsMap[i.Value.resourceType]))
                    .Append('&');
            }
            stringBuilder.Append(fields);

            return stringBuilder.ToString();
        }

        private static string GenerateFields(Type type)
        {
            var patreonResourceAttrib = type.GetCustomAttribute<Models.PatreonResourceAttribute>();
            if (patreonResourceAttrib == null)
                throw new ArgumentException($"{type.Name} does not have a {nameof(Models.PatreonResourceAttribute)} attribute.", nameof(type));
            if(string.IsNullOrWhiteSpace(patreonResourceAttrib.fieldName))
                throw new ArgumentException($"{type.Name} cannot have a null or empty field name.", nameof(type));

            var fields = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (fields.Length == 0)
                throw new ArgumentException($"{type.Name} has no public properties.", nameof(type));

            var jsonType = typeof(JsonPropertyAttribute);
            StringBuilder stringBuilder = new StringBuilder($"fields%5B{patreonResourceAttrib.fieldName}%5D=");
            for (int i = 0; i < fields.Length; i++)
            {
                var attributes = fields[i].GetCustomAttributes(jsonType, true);
                if (attributes.Length == 0) continue;

                for (int j = 0; j < attributes.Length; j++)
                {
                    var attribute = attributes[j] as JsonPropertyAttribute;
                    if(attribute != null)
                    {
                        string propertyName = attribute.PropertyName;
                        if (propertyName == "id" || propertyName == "type" || propertyName == "relationships")
                            continue;

                        if (i > 0)
                            stringBuilder.Append(',');
                        stringBuilder.Append(propertyName);
                        break;
                    }
                }
            }
            return stringBuilder.ToString();
        }
    }
}
