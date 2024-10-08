using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NorthwindCRUD.Filters
{
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                var enumValues = Enum.GetValues(context.Type)
                    .Cast<object>()
                    .Select(e => new OpenApiString(e.ToString()))
                    .ToList<IOpenApiAny>();

                schema.Enum = enumValues;
            }
        }
    }
}
