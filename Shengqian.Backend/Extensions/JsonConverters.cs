using SmartCtor.Monadics;

namespace Shengqian.Backend.Extensions;

using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

public sealed class OptionalJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) =>
        typeToConvert.IsGenericType &&
        typeToConvert.GetGenericTypeDefinition() == typeof(Option<>);

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        Type innerType = typeToConvert.GetGenericArguments()[0];
        Type converterType = typeof(OptionalJsonConverter<>).MakeGenericType(innerType);

        return (JsonConverter)Activator.CreateInstance(converterType, options)!;
    }


    private sealed class OptionalJsonConverter<T>(JsonSerializerOptions options) : JsonConverter<Option<T>> where T : notnull
    {
        private readonly JsonConverter<T> _innerConverter = (JsonConverter<T>)options.GetConverter(typeof(T));
        private readonly bool _isNestedOption = typeof(T).IsGenericType &&
                              typeof(T).GetGenericTypeDefinition() == typeof(Option<>);

        public override bool HandleNull => true;

        public override Option<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null && !_isNestedOption)
            {
                return new None();
            }

            T? value = _innerConverter.Read(ref reader, typeof(T), options);

            return value is not null ? new Option<T>(value) : new None();
        }

        public override void Write(Utf8JsonWriter writer, Option<T> value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case T innerValue:
                    _innerConverter.Write(writer, innerValue, options);
                    break;
                case None:
                default:
                    writer.WriteNullValue();
                    break;
            }
        }
    }
}
