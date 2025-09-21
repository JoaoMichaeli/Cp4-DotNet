namespace VisionHive.Domain.Pagination;

public class PageResult<T>
{
    public IEnumerable<T> Items { get; set; } =  new List<T>();
    
    public int Total {get; set;}
    
    public bool HasMore {get; set;}
    
    public int Page {get; set;}
    
    public int PageSize {get; set;}
}