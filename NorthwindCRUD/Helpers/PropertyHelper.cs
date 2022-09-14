namespace NorthwindCRUD.Helpers
{
    using System.Reflection;

    public static class PropertyHelper<T>
    {
        public static void MakePropertiesEmptyIfNull(T model)
        {
            if (model != null)
            {
                foreach (PropertyInfo prop in model?.GetType()?.GetProperties())
                {
                    var value = prop.GetValue(model);
                    var type = prop.PropertyType;
                    if (value == null && type == typeof(string))
                    {
                        prop.SetValue(model, "");
                    }
                }
            }
        }
    }
}
