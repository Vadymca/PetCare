namespace PetCare.Api.Serialization;

using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// A flexible JSON converter factory for enums.
/// Serializes enums as strings (camelCase),
/// and deserializes both strings and integers.
/// </summary>
public sealed class FlexibleStringEnumConverterFactory : JsonConverterFactory
{
    private readonly JsonNamingPolicy? _namingPolicy;
    private readonly bool _allowIntegerValues;

    public FlexibleStringEnumConverterFactory(JsonNamingPolicy? namingPolicy = null, bool allowIntegerValues = true)
    {
        _namingPolicy = namingPolicy;
        _allowIntegerValues = allowIntegerValues;
    }

    public override bool CanConvert(Type typeToConvert)
    {
        var enumType = Nullable.GetUnderlyingType(typeToConvert) ?? typeToConvert;
        return enumType.IsEnum;
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var underlying = Nullable.GetUnderlyingType(typeToConvert);
        if (underlying != null)
        {
            var converter = (JsonConverter)Activator.CreateInstance(
                typeof(FlexibleNullableStringEnumConverter<>).MakeGenericType(underlying),
                _namingPolicy, _allowIntegerValues)!;
            return converter;
        }
        else
        {
            var converter = (JsonConverter)Activator.CreateInstance(
                typeof(FlexibleStringEnumConverter<>).MakeGenericType(typeToConvert),
                _namingPolicy, _allowIntegerValues)!;
            return converter;
        }
    }

    private sealed class FlexibleStringEnumConverter<T> : JsonConverter<T> where T : struct, Enum
    {
        private readonly JsonNamingPolicy? _namingPolicy;
        private readonly bool _allowIntegerValues;
        private readonly Type _enumType;

        public FlexibleStringEnumConverter(JsonNamingPolicy? namingPolicy, bool allowIntegerValues)
        {
            _namingPolicy = namingPolicy;
            _allowIntegerValues = allowIntegerValues;
            _enumType = typeof(T);
        }

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var s = reader.GetString()!;
                if (Enum.TryParse<T>(s, ignoreCase: true, out var parsed))
                    return parsed;

                if (_namingPolicy != null)
                {
                    var pascal = ToPascalCase(s);
                    if (Enum.TryParse<T>(pascal, ignoreCase: true, out parsed))
                        return parsed;
                }

                throw new JsonException($"Cannot convert '{s}' to enum {_enumType.Name}.");
            }

            if (reader.TokenType == JsonTokenType.Number)
            {
                if (!_allowIntegerValues)
                    throw new JsonException($"Integer value not allowed for enum {_enumType.Name}.");

                if (reader.TryGetInt32(out var intValue))
                {
                    return (T)Enum.ToObject(typeof(T), intValue);
                }

                throw new JsonException($"Unexpected numeric value when parsing enum {_enumType.Name}.");
            }

            throw new JsonException($"Unexpected token {reader.TokenType} when parsing enum {_enumType.Name}.");
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            var name = Enum.GetName(_enumType, value)!;
            var outName = _namingPolicy != null ? _namingPolicy.ConvertName(name) : name;
            writer.WriteStringValue(outName);
        }

        private static string ToPascalCase(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            return char.ToUpperInvariant(s[0]) + s.Substring(1);
        }
    }

    private sealed class FlexibleNullableStringEnumConverter<T> : JsonConverter<T?> where T : struct, Enum
    {
        private readonly FlexibleStringEnumConverter<T> _inner;
        public FlexibleNullableStringEnumConverter(JsonNamingPolicy? namingPolicy, bool allowIntegerValues)
        {
            _inner = new FlexibleStringEnumConverter<T>(namingPolicy, allowIntegerValues);
        }

        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null) return null;
            return _inner.Read(ref reader, typeof(T), options);
        }

        public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
        {
            if (value.HasValue) _inner.Write(writer, value.Value, options);
            else writer.WriteNullValue();
        }
    }
}
