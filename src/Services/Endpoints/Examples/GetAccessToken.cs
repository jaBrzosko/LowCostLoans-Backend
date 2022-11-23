using FastEndpoints;

namespace Endpoints.Auxilliary;

public class GetAccessToken
{
    [FromHeader(IsRequired = false)] // Access token will be validated by Auth middleware
    public string Authorization { get; set; } = ""; // Populated from Request Header
}