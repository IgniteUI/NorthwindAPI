namespace NorthwindCRUD.Helpers
{
    using System.Reflection;

    public static class PropertyHelper<T>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "Need generic type here")]
        public static void MakePropertiesEmptyIfNull(T model)
        {
            if (model != null)
            {
                var properties = model?.GetType()?.GetProperties();
                if (properties != null)
                {
                    foreach (PropertyInfo prop in properties)
                    {
                        var value = prop.GetValue(model);
                        var type = prop.PropertyType;
                        if (value == null && type == typeof(string))
                        {
                            prop.SetValue(model, string.Empty);
                        }
                    }
                }
            }
        }
    }
}
