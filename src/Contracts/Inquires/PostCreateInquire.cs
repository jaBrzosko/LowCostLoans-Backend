using Contracts.Users;

namespace Contracts.Inquires;

public class PostCreateInquire
{
    public int MoneyInSmallestUnit { get; set; }
    public int NumberOfInstallments { get; set; }
    public string UserId { get; set; }

    public static class ErrorCodes
    {
        public const int MoneyHasToBePositive = 1;
        public const int NumberOfInstallmentsHasToBePositive = 2;
    }
}