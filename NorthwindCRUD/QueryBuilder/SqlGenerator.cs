namespace QueryBuilder;

using System.Diagnostics.CodeAnalysis;

[SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:Code should not contain multiple whitespace in a row", Justification = "...")]
public static class SqlGenerator
{
    public static string GenerateSql(Query query)
    {
        var selectClause = BuildSelectClause(query);
        var whereClause  = BuildWhereClause(query.FilteringOperands, query.Operator);
        return $"{selectClause} {whereClause};";
    }

    private static string BuildSelectClause(Query query)
    {
        var fields = query.ReturnFields != null && query.ReturnFields.Any()
            ? string.Join(", ", query.ReturnFields)
            : "*";
        return $"SELECT {fields} FROM {query.Entity}";
    }

    private static string BuildWhereClause(QueryFilter[] filters, FilterType filterType)
    {
        if (filters == null || !filters.Any())
        {
            return string.Empty;
        }

        var conditions  = filters.Select(BuildCondition).ToArray();
        var conjunction = filterType == FilterType.And ? " AND " : " OR ";
        return $"WHERE {string.Join(conjunction, conditions)}";
    }

    private static string BuildCondition(QueryFilter filter)
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