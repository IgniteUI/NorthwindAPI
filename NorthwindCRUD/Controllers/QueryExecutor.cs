namespace QueryBuilder;

using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

public enum FilterType
{
    And = 0,
    Or = 1,
}

public interface IQuery
{
    [Required]
    public string Entity { get; set; }

    [Required]
    public string[] ReturnFields { get; set; }

    [Required]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "required name")]
    public FilterType Operator { get; set; }

    [Required]
    public IQueryFilter[] FilteringOperands { get; set; }
}

public interface IQueryFilter
{
    [Required]
    public string FieldName { get; set; }

    [Required]
    public bool IgnoreCase { get; set; }

    [Required]
    public string ConditionName { get; set; }

    public object? SearchVal { get; set; }

    public IQuery? SearchTree { get; set; }
}

public class Query : IQuery
{
    public string Entity { get; set; }

    public string[] ReturnFields { get; set; }

    public FilterType Operator { get; set; }

    public IQueryFilter[] FilteringOperands { get; set; }
}

public class QueryFilter : IQueryFilter
{
    public string FieldName { get; set; }

    public bool IgnoreCase { get; set; }

    public string ConditionName { get; set; }

    public object? SearchVal { get; set; }

    public IQuery? SearchTree { get; set; }
}

public static class QueryExecutor
{
    public static TEntity[] Run<TEntity>(this IQueryable<TEntity> source, IQuery query)
    {
        var filterExpression = BuildExpression<TEntity>(query.FilteringOperands, query.Operator);
        var filteredQuery = source.Where(filterExpression);
        if (query.ReturnFields != null && query.ReturnFields.Any())
        {
            // TODO: project required fields
        }

        return filteredQuery.ToArray();
    }

    private static Expression<Func<TEntity, bool>> BuildExpression<TEntity>(IQueryFilter[] filters, FilterType filterType)
    {
        var parameter = Expression.Parameter(typeof(TEntity), "entity");
        var finalExpression = null as Expression;
        foreach (var filter in filters)
        {
            var expression = BuildConditionExpression<TEntity>(filter, parameter);
            if (finalExpression == null)
            {
                finalExpression = expression;
            }
            else
            {
                finalExpression = filterType == FilterType.And
                    ? Expression.AndAlso(finalExpression, expression)
                    : Expression.OrElse(finalExpression, expression);
            }
        }

        return finalExpression is not null
                ? Expression.Lambda<Func<TEntity, bool>>(finalExpression, parameter)
                : (TEntity _) => true;
    }

    private static Expression BuildConditionExpression<TEntity>(IQueryFilter filter, ParameterExpression parameter)
    {
        var property = typeof(TEntity).GetProperty(filter.FieldName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) ?? throw new InvalidOperationException($"Property '{filter.FieldName}' not found on type '{typeof(TEntity)}'");
        var left = Expression.Property(parameter, property);
        var searchValue = Expression.Constant(Convert.ChangeType(filter.SearchVal, property.PropertyType, CultureInfo.InvariantCulture));
#pragma warning disable CS8604 // Possible null reference argument.
        Expression condition = filter.ConditionName.ToLower(CultureInfo.InvariantCulture) switch
        {
            "equals" => Expression.Equal(left, searchValue),
            "contains" => Expression.Call(left, typeof(string).GetMethod("Contains", new[] { typeof(string) }), searchValue),
            "startswith" => Expression.Call(left, typeof(string).GetMethod("StartsWith", new[] { typeof(string) }), searchValue),
            "endswith" => Expression.Call(left, typeof(string).GetMethod("EndsWith", new[] { typeof(string) }), searchValue),
            _ => throw new NotSupportedException($"Condition '{filter.ConditionName}' is not supported"),
        };
#pragma warning restore CS8604 // Possible null reference argument.
        if (filter.IgnoreCase && left.Type == typeof(string))
        {
            // TODO: handle case sensitivity for string types
            // left = Expression.Call(left, "ToLower", null);
            // searchValue = Expression.Constant(((string)filter.SearchVal).ToLower(CultureInfo.InvariantCulture));
        }

        return condition;
    }
}