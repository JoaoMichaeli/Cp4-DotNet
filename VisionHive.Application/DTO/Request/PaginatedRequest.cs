

namespace VisionHive.Application.DTO.Request;


public sealed class PaginatedRequest
{
	public int PageNumber { get; set; }
	public int PageSize { get; set; }
	public string Subject { get; set; }
}
