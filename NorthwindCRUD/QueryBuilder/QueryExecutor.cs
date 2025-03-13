namespace QueryBuilder;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using AutoMapper.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NorthwindCRUD;
using Swashbuckle.AspNetCore.SwaggerGen;

/// <summary>
/// A generic query executor that can be used to execute queries on IQueryable data sources.
/// </summary>
[SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:Code should not contain multiple whitespace in a row", Justification = "...")]
public static class QueryExecutor
{
    public static TEntity[] Run<TEntity>(this IQueryable<TEntity> source, Query? query)
    {
        var infrastructure = source as IInfrastructure<IServiceProvider>;
        var serviceProvider = infrastructure!.Instance;
        var currentDbContext = serviceProvider.GetService(typeof(ICurrentDbContext)) as ICurrentDbContext;
        var db = currentDbContext!.Context as DataContext;
        return db is not null ? BuildQuery(db, source, query).ToArray() : Array.Empty<TEntity>();
    }

    private static IQueryable<TEntity> BuildQuery<TEntity>(DataContext db, IQueryable<TEntity> source, Query? query)
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

    private static Expression<Func<TEntity, bool>> BuildExpression<TEntity>(DataContext db, IQueryable<TEntity> source, QueryFilter[] filters, FilterType filterType)
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

    private static Expression BuildConditionExpression<TEntity>(DataContext db, IQueryable<TEntity> source, QueryFilter filter, ParameterExpression parameter)
    {
        if (filter.FieldName is not null && filter.IgnoreCase is not null && filter.Condition is not null)
        {
            var property = source.ElementType.GetProperty(filter.FieldName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                                 ?? throw new InvalidOperationException($"Property '{filter.FieldName}' not found on type '{source.ElementType}'");
            var field = Expression.Property(parameter, property);
            var targetType = property.PropertyType;
            var searchValue = GetSearchValue(filter.SearchVal, targetType);
            var emptyValue = GetEmptyValue(targetType);
            var now = DateTime.Now.Date;
            Expression condition = filter.Condition.Name switch
            {
                "null"                  => targetType.IsNullableType() ? Expression.Equal(field, Expression.Constant(targetType.GetDefaultValue())) : Expression.Constant(false),
                "notNull"               => targetType.IsNullableType() ? Expression.NotEqual(field, Expression.Constant(targetType.GetDefaultValue())) : Expression.Constant(true),
                "empty"                 => Expression.Or(Expression.Equal(field, emptyValue), targetType.IsNullableType() ? Expression.Equal(field, Expression.Constant(targetType.GetDefaultValue())) : Expression.Constant(false)),
                "notEmpty"              => Expression.And(Expression.NotEqual(field, emptyValue), targetType.IsNullableType() ? Expression.NotEqual(field, Expression.Constant(targetType.GetDefaultValue())) : Expression.Constant(true)),
                "equals"                => Expression.Equal(field, searchValue),
                "doesNotEqual"          => Expression.NotEqual(field, searchValue),
                "inQuery"               => BuildInExpression(db, filter.SearchTree, field),
                "notInQuery"            => Expression.Not(BuildInExpression(db, filter.SearchTree, field)),
                "contains"              => CallContains(field, searchValue),
                "doesNotContain"        => Expression.Not(CallContains(field, searchValue)),
                "startsWith"            => CallStartsWith(field, searchValue),
                "endsWith"              => CallEndsWith(field, searchValue),
                "greaterThan"           => Expression.GreaterThan(field, searchValue),
                "lessThan"              => Expression.LessThan(field, searchValue),
                "greaterThanOrEqualTo"  => Expression.GreaterThanOrEqual(field, searchValue),
                "lessThanOrEqualTo"     => Expression.LessThanOrEqual(field, searchValue),
                "before"                => Expression.LessThan(CallCompare(field, searchValue), Expression.Constant(0)),
                "after"                 => Expression.GreaterThan(CallCompare(field, searchValue), Expression.Constant(0)),
                "today"                 => CallStartsWith(field, now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)),
                "yesterday"             => CallStartsWith(field, now.AddDays(-1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)),
                "thisMonth"             => CallStartsWith(field, now.ToString("yyyy-MM", CultureInfo.InvariantCulture)),
                "lastMonth"             => CallStartsWith(field, now.AddMonths(-1).ToString("yyyy-MM", CultureInfo.InvariantCulture)),
                "nextMonth"             => CallStartsWith(field, now.AddMonths(1).ToString("yyyy-MM", CultureInfo.InvariantCulture)),
                "thisYear"              => CallStartsWith(field, now.ToString("yyyy", CultureInfo.InvariantCulture)),
                "lastYear"              => CallStartsWith(field, now.AddYears(-1).ToString("yyyy", CultureInfo.InvariantCulture)),
                "nextYear"              => CallStartsWith(field, now.AddYears(1).ToString("yyyy", CultureInfo.InvariantCulture)),
                "at"                    => Expression.Equal(field, searchValue),
                "not_at"                => Expression.NotEqual(field, searchValue),
                "at_before"             => Expression.LessThan(CallCompare(field, searchValue), Expression.Constant(0)),
                "at_after"              => Expression.GreaterThan(CallCompare(field, searchValue), Expression.Constant(0)),
                "all"                   => Expression.Constant(true),
                "true"                  => Expression.IsTrue(field),
                "false"                 => Expression.IsFalse(field),
                _                       => throw new NotImplementedException("Not implemented"),
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

    private static Expression CallContains(Expression field, Expression searchValue)
    {
        var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        return Expression.Call(field, containsMethod!, searchValue);
    }

    private static Expression CallStartsWith(Expression field, Expression searchValue)
    {
        var startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
        return Expression.Call(field, startsWithMethod!, searchValue);
    }

    private static Expression CallStartsWith(Expression field, string dateLiteral)
    {
        return CallStartsWith(field, Expression.Constant(dateLiteral));
    }

    private static Expression CallEndsWith(Expression field, Expression searchValue)
    {
        var endsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
        return Expression.Call(field, endsWithMethod!, searchValue);
    }

    private static Expression CallEndsWith(Expression field, string dateLiteral)
    {
        return CallEndsWith(field, Expression.Constant(dateLiteral));
    }

    private static Expression CallCompare(Expression field, Expression searchValue)
    {
        var compareMethod = typeof(string).GetMethod("Compare", new[] { typeof(string), typeof(string) });
        return Expression.Call(compareMethod!, field, searchValue);
    }

    private static Expression BuildInExpression(DataContext db, Query? query, MemberExpression field)
    {
        return field.Type switch
        {
            { } t when t == typeof(string)      => BuildInExpression<string>(db, query, field),
            { } t when t == typeof(bool)        => BuildInExpression<bool>(db, query, field),
            { } t when t == typeof(bool?)       => BuildInExpression<bool?>(db, query, field),
            { } t when t == typeof(int)         => BuildInExpression<int>(db, query, field),
            { } t when t == typeof(int?)        => BuildInExpression<int?>(db, query, field),
            { } t when t == typeof(decimal)     => BuildInExpression<decimal>(db, query, field),
            { } t when t == typeof(decimal?)    => BuildInExpression<decimal?>(db, query, field),
            { } t when t == typeof(float)       => BuildInExpression<float>(db, query, field),
            { } t when t == typeof(float?)      => BuildInExpression<float?>(db, query, field),
            { } t when t == typeof(DateTime)    => BuildInExpression<DateTime>(db, query, field),
            { } t when t == typeof(DateTime?)   => BuildInExpression<DateTime?>(db, query, field),
            _ => throw new InvalidOperationException($"Type '{field.Type}' not supported for 'IN' operation"),
        };
    }

    private static Expression BuildInExpression<T>(DataContext db, Query? query, MemberExpression field)
    {
        var d = RunSubquery(db, query).Select(x => (T)ProjectField(x, query?.ReturnFields[0] ?? string.Empty)).ToArray();
        var m = typeof(Enumerable).GetMethods()
            .FirstOrDefault(method => method.Name == nameof(Enumerable.Contains) && method.GetParameters().Length == 2)
            ?.MakeGenericMethod(typeof(T)) ?? throw new InvalidOperationException("Missing method");
        return Expression.Call(m, Expression.Constant(d), field);
    }

    private static IEnumerable<dynamic> RunSubquery(DataContext db, Query? query)
    {
        // var t = query?.Entity.ToLower(CultureInfo.InvariantCulture);
        // var p = db.GetType().GetProperty(t, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) ?? throw new InvalidOperationException($"Property '{t}' not found on type '{db.GetType()}'");
        // var q = p.GetValue(db) as IQueryable<dynamic>;
        // return q is null ? Array.Empty<dynamic>() : q.Run(query).ToArray();
        var t = query?.Entity.ToLower(CultureInfo.InvariantCulture) ?? string.Empty;
        return t switch
        {
            "addresses"     => db.Suppliers.Run(query).ToArray(),
            "categories"    => db.Categories.Run(query).ToArray(),
            "products"      => db.Products.Run(query).ToArray(),
            "regions"       => db.Regions.Run(query).ToArray(),
            "territories"   => db.Territories.Run(query).ToArray(),
            "employees"     => db.Employees.Run(query).ToArray(),
            "customers"     => db.Customers.Run(query).ToArray(),
            "orders"        => db.Orders.Run(query).ToArray(),
            "orderdetails"  => db.OrderDetails.Run(query).ToArray(),
            "shippers"      => db.Shippers.Run(query).ToArray(),
            "suppliers"     => db.Suppliers.Run(query).ToArray(),
            _               => Array.Empty<dynamic>(),
        };
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
