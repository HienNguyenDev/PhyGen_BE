namespace PhyGen.Application.Abstraction.Query;

public interface ISortableQuery
{
    public string? SortBy { get; init; }
    public SortOrder? SortOrder { get; init; }
}