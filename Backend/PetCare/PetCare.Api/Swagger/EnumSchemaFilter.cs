namespace PetCare.Api.Swagger;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json;

public class EnumSchemaFilter : ISchemaFilter
{
    private readonly JsonNamingPolicy? _namingPolicy;

    public EnumSchemaFilter(JsonNamingPolicy? namingPolicy = null)
    {
        _namingPolicy = namingPolicy;
    }

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (!context.Type.IsEnum) return;

        schema.Enum.Clear();
        schema.Type = "string";
        schema.Format = null;

        foreach (var name in Enum.GetNames(context.Type))
        {
            var convertedName = _namingPolicy?.ConvertName(name) ?? name;
            schema.Enum.Add(new OpenApiString(convertedName));
        }
    }
}
