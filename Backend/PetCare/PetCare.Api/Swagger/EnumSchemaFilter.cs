namespace PetCare.Api.Swagger;

using System.Text.Json;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

/// <summary>
/// Modifies OpenAPI schema generation to represent enum types as string values, optionally applying a naming policy to
/// enum member names.
/// </summary>
/// <remarks>This schema filter is typically used with Swagger/OpenAPI generators to ensure that enums are
/// described as strings in the generated documentation, rather than as integers. If a naming policy is provided, it is
/// applied to each enum member name before adding it to the schema. This can be useful for conforming to specific
/// naming conventions in API documentation.</remarks>
public class EnumSchemaFilter : ISchemaFilter
{
    private readonly JsonNamingPolicy? namingPolicy;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnumSchemaFilter"/> class with an optional naming policy for enum values.
    /// </summary>
    /// <param name="namingPolicy">An optional naming policy to apply to enum value names. If null, the default naming is used.</param>
    public EnumSchemaFilter(JsonNamingPolicy? namingPolicy = null)
    {
        this.namingPolicy = namingPolicy;
    }

    /// <summary>
    /// Modifies the OpenAPI schema for an enumeration type to represent its values as strings according to the
    /// specified naming policy.
    /// </summary>
    /// <remarks>This method only applies changes if the provided type is an enumeration. It clears any
    /// existing enum values in the schema and replaces them with string representations of the enum names, optionally
    /// transformed by a naming policy. The schema's type is set to "string" and its format is cleared.</remarks>
    /// <param name="schema">The OpenApiSchema instance to modify. Represents the schema definition for the enumeration type.</param>
    /// <param name="context">The context containing metadata about the type being processed, including the enumeration type information.</param>
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (!context.Type.IsEnum)
        {
            return;
        }

        schema.Enum.Clear();
        schema.Type = "string";
        schema.Format = null;

        foreach (var name in Enum.GetNames(context.Type))
        {
            var convertedName = this.namingPolicy?.ConvertName(name) ?? name;
            schema.Enum.Add(new OpenApiString(convertedName));
        }
    }
}
