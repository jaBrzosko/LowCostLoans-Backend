using Contracts.Common;

namespace Contracts.Inquires;

public class GetInquireByUser: GetPaginatedList
{
    public Guid UserId { get; set; }
}