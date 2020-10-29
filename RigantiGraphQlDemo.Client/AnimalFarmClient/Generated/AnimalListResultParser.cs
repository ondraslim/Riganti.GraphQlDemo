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
    public class AnimalListResultParser
        : JsonResultParserBase<IAnimalList>
    {
        private readonly IValueSerializer _iDSerializer;
        private readonly IValueSerializer _stringSerializer;

        public AnimalListResultParser(IValueSerializerResolver serializerResolver)
        {
            if (serializerResolver is null)
            {
                throw new ArgumentNullException(nameof(serializerResolver));
            }
            _iDSerializer = serializerResolver.GetValueSerializer("ID");
            _stringSerializer = serializerResolver.GetValueSerializer("String");
        }

        protected override IAnimalList ParserData(JsonElement data)
        {
            return new AnimalList
            (
                ParseAnimalListAnimals(data, "animals")
            );

        }

        private IAnimalConnection ParseAnimalListAnimals(
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

            return new AnimalConnection
            (
                ParseAnimalListAnimalsNodes(obj, "nodes")
            );
        }

        private IReadOnlyList<IAnimal> ParseAnimalListAnimalsNodes(
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
            var list = new IAnimal[objLength];
            for (int objIndex = 0; objIndex < objLength; objIndex++)
            {
                JsonElement element = obj[objIndex];
                list[objIndex] = new Animal
                (
                    DeserializeID(element, "id"),
                    DeserializeNullableString(element, "name"),
                    DeserializeNullableString(element, "species"),
                    DeserializeID(element, "farmId")
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
