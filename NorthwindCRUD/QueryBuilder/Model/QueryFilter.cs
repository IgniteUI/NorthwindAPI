namespace QueryBuilder;

public class QueryFilter
{
    // Basic condition
    public string? FieldName { get; set; }

    public bool? IgnoreCase { get; set; }

    public QueryFilterCondition? Condition { get; set; }

    public object? SearchVal { get; set; }

    public Query? SearchTree { get; set; }

    // And/Or
    public FilterType? Operator { get; set; }

    public QueryFilter[] FilteringOperands { get; set; }
}
