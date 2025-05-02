namespace API_BASE.Application.Common;

public class PaginatedResponse<T>
{
    public int Page { get; set; }
    public int Limit { get; set; }
    public int TotalRecords { get; set; }
    public IEnumerable<T> Data { get; set; } = new List<T>();

    public static PaginatedResponse<T> Create(IEnumerable<T> data, int total, int page, int limit)
    {
        return new PaginatedResponse<T>
        {
            Data = data,
            TotalRecords = total,
            Page = page,
            Limit = limit
        };
    }
}
