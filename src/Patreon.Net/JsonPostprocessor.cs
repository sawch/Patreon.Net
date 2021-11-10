using Newtonsoft.Json.Linq;

namespace Patreon.Net
{
    internal static class JsonPostprocessor
    {
        public static void Process(JObject rootObject)
        {
            if (!rootObject.TryGetValue("included", out JToken includedToken))
                return;
            if (!rootObject.TryGetValue("data", out JToken dataToken))
                return;

            var includedArray = includedToken.ToObject<JArray>();
            if (dataToken.Type == JTokenType.Array)
            {
                var dataArray = (JArray)dataToken;
                foreach (var data in dataArray)
                    ProcessDataToken(data, includedArray);
            }
            else
            {
                ProcessDataToken(dataToken, includedArray);
            }
        }

        private static void ProcessDataToken(JToken dataToken, JArray includedArray)
        {
            var relationshipsToken = dataToken.SelectToken("relationships");
            if (relationshipsToken == null)
                return;

            foreach (var relationship in relationshipsToken)
            {
                var relationshipData = relationship.First?.SelectToken("data");
                if (relationshipData == null) continue;

                if (relationshipData.Type == JTokenType.Array)
                {
                    var relationshipDatas = (JArray)relationshipData;
                    foreach (var element in relationshipDatas)
                    {
                        var relationshipId = element.SelectToken("id");
                        if (relationshipId == null) continue;
                        var relationshipType = element.SelectToken("type");
                        if (relationshipType == null) continue;

                        MergeIncludedAttributes(element, (string)relationshipId, (string)relationshipType, includedArray);
                    }
                }
                else
                {
                    var relationshipId = relationshipData.SelectToken("id");
                    if (relationshipId == null) continue;
                    var relationshipType = relationshipData.SelectToken("type");
                    if (relationshipType == null) continue;

                    MergeIncludedAttributes(relationshipData, (string)relationshipId, (string)relationshipType, includedArray);
                }
            }
        }

        private static void MergeIncludedAttributes(JToken relationshipData, string relationshipId, string relationshipType, JArray includedArray)
        {
            foreach (var include in includedArray)
            {
                var includedTypeToken = include.SelectToken("type", false);
                if (includedTypeToken == null)
                    continue;

                var includedIdToken = include.SelectToken("id", false);
                if (includedIdToken == null)
                    continue;

                if ((string)includedIdToken == relationshipId && (string)includedTypeToken == relationshipType)
                {
                    var attributes = include.SelectToken("attributes");
                    if (attributes != null)
                        relationshipData["attributes"] = attributes;
                    return;
                }
            }
        }
    }
}
