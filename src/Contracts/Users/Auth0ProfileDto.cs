namespace Contracts.Users;

public class Auth0ProfileDto
{
    public string Id { get; set; } = null!;
    public string Nickname { get; set; } = null!;
    public string Name { get; set; } = null!;
    public Uri PictureUrl { get; set; } = null!;
    public DateTime UpdateTime { get; set; }
    public string Mail { get; set; } = null!;
    public bool IsMailVerified { get; set; }
}
