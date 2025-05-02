namespace API_BASE.Application.Common;

public class PaginationRequest
{
    public int Page { get; set; } = 1;
    public int Limit { get; set; } = 10;
}
