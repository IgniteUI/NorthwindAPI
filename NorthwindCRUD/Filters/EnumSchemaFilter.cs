using System.Reflection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NorthwindCRUD.Filters
{
    public class EnumSchemaFilter : ISchemaFilter
    {
        private static readonly Assembly CurrentAssembly = typeof(EnumSchemaFilter).Assembly;

        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.IsEnum && context.Type.Assembly == CurrentAssembly)
            {
                schema.Type = "string";
                schema.Enum = context.Type
                    .GetEnumNames()
                    .Select(name => new OpenApiString(name))
                    .ToList<IOpenApiAny>();
                schema.Format = null;
            }
        }
    }
}
