namespace PhyGen.Domain.Common;

public class Page<T>
{
    public ICollection<T> Items { get; } = [];
    public int? PageNumber { get; set; }
    public int? TotalPages { get; set; }
    public int TotalCount { get; set; }

    protected Page()
    {
    }

    public Page(ICollection<T> items, int count, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
        Items = items;
    }

    public Page(ICollection<T> items, int count)
    {
        PageNumber = null;
        TotalPages = null;
        TotalCount = count;
        Items = items;
    }

    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageNumber < TotalPages;

}