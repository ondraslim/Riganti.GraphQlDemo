using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using StrawberryShake;
using StrawberryShake.Http;
using StrawberryShake.Http.Subscriptions;
using StrawberryShake.Transport;

namespace RigantiGraphQlDemo.Client
{
    public class FarmListResultParser
        : JsonResultParserBase<IFarmList>
    {
        private readonly IValueSerializer _iDSerializer;
        private readonly IValueSerializer _stringSerializer;

        public FarmListResultParser(IValueSerializerResolver serializerResolver)
        {
            if (serializerResolver is null)
            {
                throw new ArgumentNullException(nameof(serializerResolver));
            }
            _iDSerializer = serializerResolver.GetValueSerializer("ID");
            _stringSerializer = serializerResolver.GetValueSerializer("String");
        }

        protected override IFarmList ParserData(JsonElement data)
        {
            return new FarmList
            (
                ParseFarmListFarms(data, "farms")
            );

        }

        private IFarmConnection ParseFarmListFarms(
            JsonElement parent,
            string field)
        {
            if (!parent.TryGetProperty(field, out JsonElement obj))
            {
                return null;
            }

            if (obj.ValueKind == JsonValueKind.Null)
            {
                return null;
            }

            return new FarmConnection
            (
                ParseFarmListFarmsNodes(obj, "nodes")
            );
        }

        private IReadOnlyList<IFarm> ParseFarmListFarmsNodes(
            JsonElement parent,
            string field)
        {
            if (!parent.TryGetProperty(field, out JsonElement obj))
            {
                return null;
            }

            if (obj.ValueKind == JsonValueKind.Null)
            {
                return null;
            }

            int objLength = obj.GetArrayLength();
            var list = new IFarm[objLength];
            for (int objIndex = 0; objIndex < objLength; objIndex++)
            {
                JsonElement element = obj[objIndex];
                list[objIndex] = new Farm
                (
                    DeserializeID(element, "id"),
                    DeserializeNullableString(element, "name")
                );

            }

            return list;
        }

        private string DeserializeID(JsonElement obj, string fieldName)
        {
            JsonElement value = obj.GetProperty(fieldName);
            return (string)_iDSerializer.Deserialize(value.GetString());
        }

        private string DeserializeNullableString(JsonElement obj, string fieldName)
        {
            if (!obj.TryGetProperty(fieldName, out JsonElement value))
            {
                return null;
            }

            if (value.ValueKind == JsonValueKind.Null)
            {
                return null;
            }

            return (string)_stringSerializer.Deserialize(value.GetString());
        }
    }
}
