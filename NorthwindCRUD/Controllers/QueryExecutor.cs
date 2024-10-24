namespace QueryBuilder;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using AutoMapper.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Newtonsoft.Json;
using NorthwindCRUD;
using Swashbuckle.AspNetCore.SwaggerGen;

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
    // Basic condition
    public string? FieldName { get; set; }

    public bool? IgnoreCase { get; set; }

    public IQueryFilterCondition? Condition { get; set; }

    public object? SearchVal { get; set; }

    public IQuery? SearchTree { get; set; }

    // And/Or
    [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "required name")]
    public FilterType? Operator { get; set; }

    public IQueryFilter[] FilteringOperands { get; set; }
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
    // Basic condition
    public string? FieldName { get; set; }

    public bool? IgnoreCase { get; set; }

    public IQueryFilterCondition? Condition { get; set; }

    public object? SearchVal { get; set; }

    public IQuery? SearchTree { get; set; }

    // And/Or
    public FilterType? Operator { get; set; }

    public IQueryFilter[] FilteringOperands { get; set; }
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
public static class QueryExecutor
{
    public static TEntity[] Run<TEntity>(this IQueryable<TEntity> source, IQuery? query)
    {
        var infrastructure = source as IInfrastructure<IServiceProvider>;
        var serviceProvider = infrastructure!.Instance;
        var currentDbContext = serviceProvider.GetService(typeof(ICurrentDbContext)) as ICurrentDbContext;
        var db = currentDbContext!.Context as DataContext;
        return db is not null ? BuildQuery(db, source, query).ToArray() : Array.Empty<TEntity>();
    }

    private static IQueryable<TEntity> BuildQuery<TEntity>(DataContext db, IQueryable<TEntity> source, IQuery? query)
    {
        if (query is null)
        {
            throw new InvalidOperationException("Null query");
        }

        var filterExpression = BuildExpression(db, source, query.FilteringOperands, query.Operator);
        var filteredQuery = source.Where(filterExpression);
        if (query.ReturnFields != null && query.ReturnFields.Any())
        {
            var projectionExpression = BuildProjectionExpression<TEntity>(query.ReturnFields);
            return filteredQuery.Select(projectionExpression).Cast<TEntity>();
        }
        else
        {
            return filteredQuery;
        }
    }

    private static Expression<Func<TEntity, bool>> BuildExpression<TEntity>(DataContext db, IQueryable<TEntity> source, IQueryFilter[] filters, FilterType filterType)
    {
        var parameter = Expression.Parameter(typeof(TEntity), "entity");
        var finalExpression = null as Expression;
        foreach (var filter in filters)
        {
            var expression = BuildConditionExpression(db, source, filter, parameter);
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

    private static Expression BuildConditionExpression<TEntity>(DataContext db, IQueryable<TEntity> source, IQueryFilter filter, ParameterExpression parameter)
    {
        if (filter.FieldName is not null && filter.IgnoreCase is not null && filter.Condition is not null)
        {
            var property = source.ElementType.GetProperty(filter.FieldName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                                 ?? throw new InvalidOperationException($"Property '{filter.FieldName}' not found on type '{source.ElementType}'");
            var field = Expression.Property(parameter, property);
            var targetType = property.PropertyType;
            var searchValue = GetSearchValue(filter.SearchVal, targetType);
            var emptyValue = GetEmptyValue(targetType);
            Expression condition = filter.Condition.Name switch
            {
                "null" => targetType.IsNullableType() ? Expression.Equal(field, Expression.Constant(targetType.GetDefaultValue())) : Expression.Constant(false),
                "notNull" => targetType.IsNullableType() ? Expression.NotEqual(field, Expression.Constant(targetType.GetDefaultValue())) : Expression.Constant(true),
                "empty" => Expression.Or(Expression.Equal(field, emptyValue), targetType.IsNullableType() ? Expression.Equal(field, Expression.Constant(targetType.GetDefaultValue())) : Expression.Constant(false)),
                "notEmpty" => Expression.And(Expression.NotEqual(field, emptyValue), targetType.IsNullableType() ? Expression.NotEqual(field, Expression.Constant(targetType.GetDefaultValue())) : Expression.Constant(true)),
                "equals" => Expression.Equal(field, searchValue),
                "doesNotEqual" => Expression.NotEqual(field, searchValue),
                "in" => BuildInExpression(db, filter.SearchTree, field),
                "notIn" => Expression.Not(BuildInExpression(db, filter.SearchTree, field)),
                "contains" => Expression.Call(field, typeof(string).GetMethod("Contains", new[] { typeof(string) })!, searchValue),
                "doesNotContain" => Expression.Not(Expression.Call(field, typeof(string).GetMethod("Contains", new[] { typeof(string) })!, searchValue)),
                "startsWith" => Expression.Call(field, typeof(string).GetMethod("StartsWith", new[] { typeof(string) })!, searchValue),
                "endsWith" => Expression.Call(field, typeof(string).GetMethod("EndsWith", new[] { typeof(string) })!, searchValue),
                "greaterThan" => Expression.GreaterThan(field, searchValue),
                "lessThan" => Expression.LessThan(field, searchValue),
                "greaterThanOrEqualTo" => Expression.GreaterThanOrEqual(field, searchValue),
                "lessThanOrEqualTo" => Expression.LessThanOrEqual(field, searchValue),
                "all" => throw new NotImplementedException("Not implemented"),
                "true" => Expression.IsTrue(field),
                "false" => Expression.IsFalse(field),
                _ => throw new NotImplementedException("Not implemented"),
            };
            if (filter.IgnoreCase.Value && field.Type == typeof(string))
            {
                // TODO: Implement case-insensitive comparison
            }

            return condition;
        }
        else
        {
            var subexpressions = filter.FilteringOperands?.Select(f => BuildConditionExpression(db, source, f, parameter)).ToArray();
            if (subexpressions == null || !subexpressions.Any())
            {
                return Expression.Constant(true);
            }

            return filter.Operator == FilterType.And
                ? subexpressions.Aggregate(Expression.AndAlso)
                : subexpressions.Aggregate(Expression.OrElse);
        }
    }

    private static Expression BuildInExpression(DataContext db, IQuery? query, MemberExpression field)
    {
        if (field.Type == typeof(string))
        {
            var d = RunSubquery(db, query).Select(x => (string)ProjectField(x, query?.ReturnFields[0] ?? string.Empty)).ToArray();
            return Expression.Call(typeof(Enumerable), "Contains", new[] { typeof(string) }, Expression.Constant(d), field);
        }
        else if (field.Type == typeof(bool) || field.Type == typeof(bool?))
        {
            var d = RunSubquery(db, query).Select(x => (bool?)ProjectField(x, query?.ReturnFields[0] ?? string.Empty)).ToArray();
            return Expression.Call(typeof(Enumerable), "Contains", new[] { typeof(bool?) }, Expression.Constant(d), field);
        }
        else if (field.Type == typeof(int) || field.Type == typeof(int?))
        {
            var d = RunSubquery(db, query).Select(x => (int?)ProjectField(x, query?.ReturnFields[0] ?? string.Empty)).ToArray();
            return Expression.Call(typeof(Enumerable), "Contains", new[] { typeof(int?) }, Expression.Constant(d), field);
        }
        else if (field.Type == typeof(decimal) || field.Type == typeof(decimal?))
        {
            var d = RunSubquery(db, query).Select(x => (decimal?)ProjectField(x, query?.ReturnFields[0] ?? string.Empty)).ToArray();
            return Expression.Call(typeof(Enumerable), "Contains", new[] { typeof(decimal?) }, Expression.Constant(d), field);
        }
        else if (field.Type == typeof(float) || field.Type == typeof(float?))
        {
            var d = RunSubquery(db, query).Select(x => (float?)ProjectField(x, query?.ReturnFields[0] ?? string.Empty)).ToArray();
            return Expression.Call(typeof(Enumerable), "Contains", new[] { typeof(float?) }, Expression.Constant(d), field);
        }
        else if (field.Type == typeof(DateTime) || field.Type == typeof(DateTime?))
        {
            var d = RunSubquery(db, query).Select(x => (DateTime?)ProjectField(x, query?.ReturnFields[0] ?? string.Empty)).ToArray();
            return Expression.Call(typeof(Enumerable), "Contains", new[] { typeof(DateTime) }, Expression.Constant(d), field);
        }
        else
        {
            throw new InvalidOperationException($"Type '{field.Type}' not supported for 'IN' operation");
        }
    }

    private static IEnumerable<dynamic> RunSubquery(DataContext db, IQuery? query)
    {
        // var t = query?.Entity.ToLower(CultureInfo.InvariantCulture);
        // var p = db.GetType().GetProperty(t, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) ?? throw new InvalidOperationException($"Property '{t}' not found on type '{db.GetType()}'");
        // var q = p.GetValue(db) as IQueryable<dynamic>;
        // return q is null ? Array.Empty<dynamic>() : q.Run(query).ToArray();
        var t = query?.Entity.ToLower(CultureInfo.InvariantCulture) ?? string.Empty;
        switch (t)
        {
            case "addresses":
                return db.Suppliers.Run(query).ToArray();
            case "categories":
                return db.Categories.Run(query).ToArray();
            case "products":
                return db.Products.Run(query).ToArray();
            case "regions":
                return db.Regions.Run(query).ToArray();
            case "territories":
                return db.Territories.Run(query).ToArray();
            case "employees":
                return db.Employees.Run(query).ToArray();
            case "customers":
                return db.Customers.Run(query).ToArray();
            case "orders":
                return db.Orders.Run(query).ToArray();
            case "orderdetails":
                return db.OrderDetails.Run(query).ToArray();
            case "shippers":
                return db.Shippers.Run(query).ToArray();
            case "suppliers":
                return db.Suppliers.Run(query).ToArray();
            default:
                return Array.Empty<dynamic>();
        }
    }

    private static dynamic? ProjectField(dynamic? obj, string field)
    {
        var property = obj?.GetType().GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) ?? throw new InvalidOperationException($"Property '{field}' not found on type '{obj?.GetType()}'");
        return property?.GetValue(obj);
    }

    private static Expression GetSearchValue(dynamic? value, Type targetType)
    {
        if (value == null)
        {
            return GetEmptyValue(targetType);
        }

        var nonNullableType = Nullable.GetUnderlyingType(targetType) ?? targetType;
        var convertedValue = Convert.ChangeType(value, nonNullableType, CultureInfo.InvariantCulture);
        return Expression.Constant(convertedValue, targetType);
    }

    private static Expression GetEmptyValue(Type targetType)
    {
        return Expression.Constant(targetType == typeof(string) ? string.Empty : targetType.GetDefaultValue());
    }

    private static Expression<Func<TEntity, dynamic>> BuildProjectionExpression<TEntity>(string[] returnFields)
    {
        var parameter = Expression.Parameter(typeof(TEntity), "entity");
        var bindings = returnFields.Select(field =>
        {
            var property = typeof(TEntity).GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) ?? throw new InvalidOperationException($"Property '{field}' not found on type '{typeof(TEntity)}'");
            var propertyAccess = Expression.Property(parameter, property);
            return Expression.Bind(property, propertyAccess);
        }).ToArray();

        var body = Expression.MemberInit(Expression.New(typeof(TEntity)), bindings);
        return Expression.Lambda<Func<TEntity, dynamic>>(body, parameter);
    }
}

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
        var condition = filter.Condition?.Name;
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
            "all"                  => throw new NotImplementedException("Not implemented"),
            "true"                 => $"{field} = TRUE",
            "false"                => $"{field} = FALSE",
            _                      => throw new NotImplementedException($"Condition '{condition}' is not implemented"),
        };
    }
}