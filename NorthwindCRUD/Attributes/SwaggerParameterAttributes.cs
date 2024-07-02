using Swashbuckle.AspNetCore.Annotations;

namespace NorthwindCRUD.Attributes
{
        public class SwaggerPageParameterAttribute : SwaggerParameterAttribute
        {
            public SwaggerPageParameterAttribute()
                : base("The page index of records to fetch. If this parameter is not provided, fetching starts from the beginning (page 0).")
            {
            }
        }

        public class SwaggerSizeParameterAttribute : SwaggerParameterAttribute
        {
            public SwaggerSizeParameterAttribute()
                : base("The maximum number of records to fetch per page. If this parameter is not provided, all records are fetched.")
            {
            }
        }

        public class SwaggerOrderByParameterAttribute : SwaggerParameterAttribute
        {
            public SwaggerOrderByParameterAttribute()
                : base("A comma-separated list of fields to order the records by, along with the sort direction (e.g., 'field1 asc, field2 desc').")
            {
            }
        }

        public class SwaggerSkipParameterAttribute : SwaggerParameterAttribute
        {
            public SwaggerSkipParameterAttribute()
                : base("The number of records to skip before starting to fetch them. If this parameter is not provided, fetching starts from the beginning.")
            {
            }
        }

        public class SwaggerTopParameterAttribute : SwaggerParameterAttribute
        {
            public SwaggerTopParameterAttribute()
                : base("The maximum number of records to fetch. If this parameter is not provided, all records are fetched.")
            {
            }
        }
}
