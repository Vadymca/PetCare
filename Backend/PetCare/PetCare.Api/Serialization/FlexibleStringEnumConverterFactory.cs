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
    private readonly JsonNamingPolicy? namingPolicy;
    private readonly bool allowIntegerValues;

    /// <summary>
    /// Initializes a new instance of the <see cref="FlexibleStringEnumConverterFactory"/> class with the specified naming policy and.
    /// integer value handling.
    /// </summary>
    /// <param name="namingPolicy">An optional naming policy to apply to enum member names during serialization and deserialization. If null, the
    /// default naming policy is used.</param>
    /// <param name="allowIntegerValues">true to allow integer values when converting enums; otherwise, false to restrict conversion to string values
    /// only.</param>
    public FlexibleStringEnumConverterFactory(JsonNamingPolicy? namingPolicy = null, bool allowIntegerValues = true)
    {
        this.namingPolicy = namingPolicy;
        this.allowIntegerValues = allowIntegerValues;
    }

    /// <summary>
    /// Determines whether the specified type is an enumeration or a nullable enumeration type that can be converted by
    /// this converter.
    /// </summary>
    /// <param name="typeToConvert">The type to check for convertibility. This can be a non-nullable or nullable enumeration type.</param>
    /// <returns>true if the specified type is an enumeration or a nullable enumeration; otherwise, false.</returns>
    public override bool CanConvert(Type typeToConvert)
    {
        var enumType = Nullable.GetUnderlyingType(typeToConvert) ?? typeToConvert;
        return enumType.IsEnum;
    }

    /// <summary>
    /// Creates a custom JSON converter for the specified type, supporting flexible string and nullable enum
    /// deserialization.
    /// </summary>
    /// <remarks>This method enables deserialization of enums from both string and integer JSON values, and
    /// supports nullable enum types. The returned converter respects the naming policy and integer value handling
    /// specified in the serializer options.</remarks>
    /// <param name="typeToConvert">The type of object to convert. Must represent an enum type or a nullable enum type.</param>
    /// <param name="options">The serialization options to use for the converter. Provides context such as naming policies and other settings.</param>
    /// <returns>A JsonConverter instance capable of serializing and deserializing the specified enum type, including support for
    /// nullable enums and flexible string representations.</returns>
    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var underlying = Nullable.GetUnderlyingType(typeToConvert);
        if (underlying != null)
        {
            var converter = (JsonConverter)Activator.CreateInstance(
                typeof(FlexibleNullableStringEnumConverter<>).MakeGenericType(underlying),
                this.namingPolicy,
                this.allowIntegerValues)!;
            return converter;
        }
        else
        {
            var converter = (JsonConverter)Activator.CreateInstance(
                typeof(FlexibleStringEnumConverter<>).MakeGenericType(typeToConvert),
                this.namingPolicy,
                this.allowIntegerValues)!;
            return converter;
        }
    }

    /// <summary>
    /// Converts enumeration values of type T to and from their string representations in JSON, supporting flexible
    /// case-insensitive matching and optional integer values.
    /// </summary>
    /// <remarks>This converter allows deserialization of enum values from JSON strings in a case-insensitive
    /// manner and optionally from integer values if enabled. It also supports applying a custom naming policy when
    /// serializing enum values to JSON. This converter is intended for use with System.Text.Json serialization and
    /// deserialization scenarios where flexible enum handling is required.</remarks>
    /// <typeparam name="T">The enumeration type to convert. Must be a value type that derives from Enum.</typeparam>
    private sealed class FlexibleStringEnumConverter<T> : JsonConverter<T>
        where T : struct, Enum
    {
        private readonly JsonNamingPolicy? namingPolicy;
        private readonly bool allowIntegerValues;
        private readonly Type enumType;

        public FlexibleStringEnumConverter(JsonNamingPolicy? namingPolicy, bool allowIntegerValues)
        {
            this.namingPolicy = namingPolicy;
            this.allowIntegerValues = allowIntegerValues;
            this.enumType = typeof(T);
        }

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var s = reader.GetString()!;
                if (Enum.TryParse<T>(s, ignoreCase: true, out var parsed))
                {
                    return parsed;
                }

                if (this.namingPolicy != null)
                {
                    var pascal = ToPascalCase(s);
                    if (Enum.TryParse<T>(pascal, ignoreCase: true, out parsed))
                    {
                        return parsed;
                    }
                }

                throw new JsonException($"Cannot convert '{s}' to enum {this.enumType.Name}.");
            }

            if (reader.TokenType == JsonTokenType.Number)
            {
                if (!this.allowIntegerValues)
                {
                    throw new JsonException($"Integer value not allowed for enum {this.enumType.Name}.");
                }

                if (reader.TryGetInt32(out var intValue))
                {
                    return (T)Enum.ToObject(typeof(T), intValue);
                }

                throw new JsonException($"Unexpected numeric value when parsing enum {this.enumType.Name}.");
            }

            throw new JsonException($"Unexpected token {reader.TokenType} when parsing enum {this.enumType.Name}.");
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            var name = Enum.GetName(this.enumType, value)!;
            var outName = this.namingPolicy != null ? this.namingPolicy.ConvertName(name) : name;
            writer.WriteStringValue(outName);
        }

        private static string ToPascalCase(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            return char.ToUpperInvariant(s[0]) + s.Substring(1);
        }
    }

    private sealed class FlexibleNullableStringEnumConverter<T> : JsonConverter<T?>
        where T : struct, Enum
    {
        private readonly FlexibleStringEnumConverter<T> inner;

        public FlexibleNullableStringEnumConverter(JsonNamingPolicy? namingPolicy, bool allowIntegerValues)
        {
            this.inner = new FlexibleStringEnumConverter<T>(namingPolicy, allowIntegerValues);
        }

        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            return this.inner.Read(ref reader, typeof(T), options);
        }

        public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                this.inner.Write(writer, value.Value, options);
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}
