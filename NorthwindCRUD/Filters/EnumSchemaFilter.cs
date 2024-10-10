﻿using Microsoft.OpenApi.Any;
﻿using Microsoft.OpenApi.Models;
﻿using Swashbuckle.AspNetCore.SwaggerGen;

﻿namespace NorthwindCRUD.Filters
{
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                schema.Type = "string";
                schema.Enum = context.Type
                    .GetEnumNames()
                    .Select(name => new OpenApiString(name))
                    .ToList<IOpenApiAny>();
            }
        }
    }
}