using Contracts.Common;

namespace Contracts.Inquires;

public class GetInquireByUser: GetPaginatedList
{
    public string UserId { get; set; }
}