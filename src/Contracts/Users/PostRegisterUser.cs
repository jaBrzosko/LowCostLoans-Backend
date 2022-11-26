using FastEndpoints;

namespace Contracts.Users;

public class PostRegisterUser
{
    [FromClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")]
    public string UserId { get; set; }

    public PersonalDataDto PersonalData { get; set; }
    
    public static class ErrorCodes
    {
        public const int UserAlreadyRegistered = 1;
        public const int UserIdIsEmpty = 2;
        public const int UserIdIsTooLong = 3;

        public class PersonalDataErrors : PersonalDataDto.ErrorCodes { }
    }
}
