namespace Services.ValidationExtensions;

public class ValidationErrors
{
    public int StatusCode { get; set; }
    public List<Error> Errors { get; set; }
}

public class Error
{
    public string ErrorMessage { get; set; }
    public string ErrorCode { get; set; }
}
