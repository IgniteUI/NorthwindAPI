namespace QueryBuilder;

public class Query
{
    public string Entity { get; set; }

    public string[] ReturnFields { get; set; }

    public FilterType Operator { get; set; }

    public QueryFilter[] FilteringOperands { get; set; }
}
