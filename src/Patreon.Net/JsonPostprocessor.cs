using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Patreon.Net
{
    /// <summary>
    /// This class merges the loosely attached "included" data into the main resource and it's relationships and condenses the object structure.
    /// </summary>
    internal static class JsonPostprocessor
    {
        public static bool Process(JObject rootObject, out JToken dataToken)
        {
            if (!rootObject.TryGetValue("data", out JToken rootDataToken))
            {
                dataToken = null;
                return false;
            }

            JArray includedArray = null;
            if (rootObject.TryGetValue("included", out JToken includedToken))
                includedArray = (JArray)includedToken;

            if (rootDataToken.Type == JTokenType.Array)
            {
                var dataArray = (JArray)rootDataToken;
                foreach (var data in dataArray)
                    ProcessDataToken(data, includedArray);

                dataToken = rootObject;
            }
            else
            {
                ProcessDataToken(rootDataToken, includedArray);
                dataToken = rootDataToken;
            }
            return true;
        }

        private static void ProcessDataToken(JToken rootDataToken, JArray includedArray)
        {
            // Move everything in "attributes" up a level, into "data"
            var attributesToken = rootDataToken.SelectToken("attributes");
            if(attributesToken != null)
                MoveTokens(attributesToken, rootDataToken);

            // Included array always exists if there are relationships
            if (includedArray == null)
                return;

            var relationshipsToken = rootDataToken.SelectToken("relationships");
            if (relationshipsToken == null)
                return;

            // Copy included data into "relationships"
            foreach (var relationshipRoot in relationshipsToken)
            {
                var relationship = relationshipRoot.First;
                if (relationship == null)
                    continue;

                // The only child token should ever be "data" but null check just in case
                var relationshipData = relationship.SelectToken("data");
                if (relationshipData == null)
                    continue;

                // Copy included data and move everything in "attributes" as well as type and id up to the relationship root
                switch (relationshipData.Type)
                {
                    case JTokenType.Null: // If "data" has a null value, make the whole object null
                        relationship.Replace(null);
                        break;
                    case JTokenType.Array:
                        {
                            var relationshipDatas = (JArray)relationshipData;
                            foreach (var element in relationshipDatas)
                            {
                                var relationshipId = element.SelectToken("id");
                                if (relationshipId == null) continue;
                                var relationshipType = element.SelectToken("type");
                                if (relationshipType == null) continue;

                                if (includedArray != null)
                                    CopyIncludedAttributes(element, (string)relationshipId, (string)relationshipType, includedArray);

                                var relationshipAttributes = element.SelectToken("attributes");
                                if (relationshipAttributes != null)
                                    MoveTokens(relationshipAttributes, element);
                            }
                            relationshipRoot.First?.Replace(relationshipDatas); // Move the "data" array up a level
                        }
                        break;
                    default:
                        {
                            var relationshipId = relationshipData.SelectToken("id");
                            if (relationshipId == null) continue;
                            var relationshipType = relationshipData.SelectToken("type");
                            if (relationshipType == null) continue;

                            if (includedArray != null)
                                CopyIncludedAttributes(relationshipData, (string)relationshipId, (string)relationshipType, includedArray);

                            var relationshipAttributes = relationshipData.SelectToken("attributes");
                            if (relationshipAttributes != null)
                            {
                                MoveTokens(relationshipAttributes, relationship);
                                MoveTokens(relationshipData, relationship);
                            }
                        }
                        break;
                }
            }
        }

        private static void MoveTokens(JToken sourceToken, JToken destinationToken)
        {
            foreach (var attribute in sourceToken)
            {
                JProperty attributeAsProperty = (JProperty)attribute;
                destinationToken[attributeAsProperty.Name] = attributeAsProperty.Value;
            }
            sourceToken.Parent.Remove();
        }

        private static void CopyIncludedAttributes(JToken relationshipData, string relationshipId, string relationshipType, JArray includedArray)
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
