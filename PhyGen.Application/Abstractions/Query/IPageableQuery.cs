namespace PhyGen.Application.Abstraction.Query;

public interface IPageableQuery
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
}