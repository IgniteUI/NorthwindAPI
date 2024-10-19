namespace QueryBuilder;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json;

public enum FilterType
{
    And = 0,
    Or = 1,
}

public interface IQuery
{
    public string Entity { get; set; }

    public string[] ReturnFields { get; set; }

    [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "required name")]
    public FilterType Operator { get; set; }

    public IQueryFilter[] FilteringOperands { get; set; }
}

public interface IQueryFilter
{
    public string FieldName { get; set; }

    public bool IgnoreCase { get; set; }

    public IQueryFilterCondition Condition { get; set; }

    public object? SearchVal { get; set; }

    public IQuery? SearchTree { get; set; }
}

public interface IQueryFilterCondition
{
    public string Name { get; set; }

    public bool IsUnary { get; set; }

    public string IconName { get; set; }
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

    public IQueryFilterCondition Condition { get; set; }

    public object? SearchVal { get; set; }

    public IQuery? SearchTree { get; set; }
}

public class QueryFilterCondition : IQueryFilterCondition
{
    public string Name { get; set; }

    public bool IsUnary { get; set; }

    public string IconName { get; set; }
}

public class QueryConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) => objectType == typeof(IQuery);

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer) => serializer.Deserialize<Query>(reader);

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) => serializer.Serialize(writer, value);
}

public class QueryFilterConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) => objectType == typeof(IQueryFilter);

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer) => serializer.Deserialize<QueryFilter>(reader);

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) => serializer.Serialize(writer, value);
}

public class QueryFilterConditionConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) => objectType == typeof(IQueryFilterCondition);

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer) => serializer.Deserialize<QueryFilterCondition>(reader);

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) => serializer.Serialize(writer, value);
}

/// <summary>
/// A generic query executor that can be used to execute queries on IQueryable data sources.
/// </summary>
[SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1009:Closing parenthesis should be spaced correctly", Justification = "...")]
[SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:Code should not contain multiple whitespace in a row", Justification = "...")]
public static class QueryExecutor
{
    public static object[] Run(this IQueryable<object> source, IQuery query)
    {
        return BuildQuery(source, query).ToArray();
    }

    private static IQueryable<object> BuildQuery(IQueryable<object> source, IQuery query)
    {
        var filterExpression = BuildExpression<object>(query.FilteringOperands, query.Operator);
        var filteredQuery = source.Where(filterExpression);
        if (query.ReturnFields != null && query.ReturnFields.Any())
        {
            var projectionExpression = BuildProjectionExpression<object>(query.ReturnFields);
            var projectedQuery = filteredQuery.Select(projectionExpression);
            return projectedQuery;
        }
        else
        {
            return filteredQuery;
        }
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
        var property         = typeof(TEntity).GetProperty(filter.FieldName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) ?? throw new InvalidOperationException($"Property '{filter.FieldName}' not found on type '{typeof(TEntity)}'");
        var field            = Expression.Property(parameter, property);
        var targetType       = GetPropertyType(property);
        var searchValue      = GetSearchValue(filter.SearchVal, targetType);
        var searchTree       = BuildSubquery(filter.SearchTree);
        Expression condition = filter.Condition.Name switch
        {
            "null"                 => Expression.Equal(field, Expression.Constant(null, targetType)),
            "notNull"              => Expression.NotEqual(field, Expression.Constant(null, targetType)),
            "empty"                => Expression.Equal(field, Expression.Constant(string.Empty, targetType)), // TODO: Implement empty condition
            "notEmpty"             => Expression.NotEqual(field, Expression.Constant(string.Empty, targetType)), // TODO: Implement not empty condition
            "equals"               => Expression.Equal(field, searchValue),
            "doesNotEqual"         => Expression.NotEqual(field, searchValue),
            "in"                   => Expression.Call(typeof(Enumerable), "Contains", new[] { targetType }, searchTree, field),
            "notIn"                => Expression.Not(Expression.Call(typeof(Enumerable), "Contains", new[] { targetType }, searchTree, field)),
            "contains"             => Expression.Call(field, typeof(string).GetMethod("Contains", new[] { typeof(string) })!, searchValue),
            "doesNotContain"       => Expression.Not(Expression.Call(field, typeof(string).GetMethod("Contains", new[] { typeof(string) })!, searchValue)),
            "startsWith"           => Expression.Call(field, typeof(string).GetMethod("StartsWith", new[] { typeof(string) })!, searchValue),
            "endsWith"             => Expression.Call(field, typeof(string).GetMethod("EndsWith", new[] { typeof(string) })!, searchValue),
            "greaterThan"          => Expression.GreaterThan(field, searchValue),
            "lessThan"             => Expression.LessThan(field, searchValue),
            "greaterThanOrEqualTo" => Expression.GreaterThanOrEqual(field, searchValue),
            "lessThanOrEqualTo"    => Expression.LessThanOrEqual(field, searchValue),
            "all"                  => throw new NotImplementedException("Not implemented"),
            "true"                 => Expression.IsTrue(field),
            "false"                => Expression.IsFalse(field),
            _                      => throw new NotImplementedException("Not implemented"),
        };
        if (filter.IgnoreCase && field.Type == typeof(string))
        {
            // TODO: Implement case-insensitive comparison
        }

        return condition;
    }

    private static Expression BuildSubquery(IQuery? query)
    {
        if (query == null)
        {
            return Expression.Constant(true);
        }

        var parameter = Expression.Parameter(typeof(object), "entity");
        var filterExpression = BuildExpression<object>(query.FilteringOperands, query.Operator);
        var lambda = Expression.Lambda<Func<object, bool>>(filterExpression.Body, parameter);
        return lambda.Body;
    }

    private static Type GetPropertyType(PropertyInfo property)
    {
        var targetType = property.PropertyType;
        if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            return Nullable.GetUnderlyingType(targetType) ?? targetType;
        }
        else
        {
            return targetType;
        }
    }

    private static Expression GetSearchValue(object? value, Type targetType)
    {
        if (value == null)
        {
            return Expression.Constant(null, targetType);
        }

        var nonNullableType = Nullable.GetUnderlyingType(targetType) ?? targetType;
        var convertedValue = Convert.ChangeType(value, nonNullableType, CultureInfo.InvariantCulture);
        return Expression.Constant(convertedValue, targetType);
    }

    private static Expression<Func<TEntity, object>> BuildProjectionExpression<TEntity>(string[] returnFields)
    {
        var parameter = Expression.Parameter(typeof(TEntity), "entity");
        var bindings = returnFields.Select(field =>
        {
            var property = typeof(TEntity).GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) ?? throw new InvalidOperationException($"Property '{field}' not found on type '{typeof(TEntity)}'");
            var propertyAccess = Expression.Property(parameter, property);
            return Expression.Bind(property, propertyAccess);
        }).ToArray();

        var body = Expression.MemberInit(Expression.New(typeof(TEntity)), bindings);
        return Expression.Lambda<Func<TEntity, object>>(body, parameter);
    }
}

[SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1009:Closing parenthesis should be spaced correctly", Justification = "...")]
[SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:Code should not contain multiple whitespace in a row", Justification = "...")]
public static class SqlGenerator
{
    public static string GenerateSql(IQuery query)
    {
        var selectClause = BuildSelectClause(query);
        var whereClause  = BuildWhereClause(query.FilteringOperands, query.Operator);
        return $"{selectClause} {whereClause};";
    }

    private static string BuildSelectClause(IQuery query)
    {
        var fields = query.ReturnFields != null && query.ReturnFields.Any()
            ? string.Join(", ", query.ReturnFields)
            : "*";
        return $"SELECT {fields} FROM {query.Entity}";
    }

    private static string BuildWhereClause(IQueryFilter[] filters, FilterType filterType)
    {
        if (filters == null || !filters.Any())
        {
            return string.Empty;
        }

        var conditions  = filters.Select(BuildCondition).ToArray();
        var conjunction = filterType == FilterType.And ? " AND " : " OR ";
        return $"WHERE {string.Join(conjunction, conditions)}";
    }

    private static string BuildCondition(IQueryFilter filter)
    {
        var field     = filter.FieldName;
        var condition = filter.Condition.Name;
        var value     = filter.SearchVal != null ? $"'{filter.SearchVal}'" : "NULL";
        var subquery  = filter.SearchTree != null ? $"({GenerateSql(filter.SearchTree)})" : string.Empty;
        return condition switch
        {
            "null"                 => $"{field} IS NULL",
            "notNull"              => $"{field} IS NOT NULL",
            "empty"                => $"{field} = ''",
            "notEmpty"             => $"{field} <> ''",
            "equals"               => $"{field} = {value}",
            "doesNotEqual"         => $"{field} <> {value}",
            "in"                   => $"{field} IN ({subquery})",
            "notIn"                => $"{field} NOT IN ({subquery})",
            "contains"             => $"{field} LIKE '%{filter.SearchVal}%'",
            "doesNotContain"       => $"{field} NOT LIKE '%{filter.SearchVal}%'",
            "startsWith"           => $"{field} LIKE '{filter.SearchVal}%'",
            "endsWith"             => $"{field} LIKE '%{filter.SearchVal}'",
            "greaterThan"          => $"{field} > {value}",
            "lessThan"             => $"{field} < {value}",
            "greaterThanOrEqualTo" => $"{field} >= {value}",
            "lessThanOrEqualTo"    => $"{field} <= {value}",
            _                      => throw new NotImplementedException($"Condition '{condition}' is not implemented"),
        };
    }
}