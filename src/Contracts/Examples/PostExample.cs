namespace Contracts.Examples;

public class PostExample : IHttpPost
{
    public string Name { get; set; }

    public static class ErrorCodes
    {
        public const int NameIsEmpty = 1;
        public const int NameIsTooLong = 2;
    }
}
