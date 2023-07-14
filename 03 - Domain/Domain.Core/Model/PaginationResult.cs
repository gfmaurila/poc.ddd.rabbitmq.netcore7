namespace Domain.Core.Model;

public class PaginationResult<T>
{
    public List<T> Data { get; set; }
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int QtdPge { get; set; }
}