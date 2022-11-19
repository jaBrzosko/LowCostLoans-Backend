namespace Services.Endpoints.Users.ValidationHelpers;

public class PeselValidationHelper
{
    private const int PeselLenght = 11;

    private readonly string pesel;
    
    private PeselValidationHelper(string pesel)
    {
        this.pesel = pesel;
    }
    
    public static bool IsValid(string pesel)
    {
        var validator = new PeselValidationHelper(pesel);
        return validator.IsValid();
    }

    private bool IsValid()
    {
        return IsLengthValid() &&
               AreAllCharactersDigits() &&
               IsControlDigitValid();
    }

    private bool IsLengthValid()
    {
        return pesel.Length == PeselLenght;
    }

    private bool AreAllCharactersDigits()
    {
        foreach (var c in pesel)
        {
            if (!char.IsDigit(c))
            {
                return false;
            }
        }

        return true;
    }

    private bool IsControlDigitValid()
    {
        return GetExpectedControlDigit() == pesel[^1];
    }

    private char GetExpectedControlDigit()
    {
        var weights = new ReadOnlySpan<int>(new int[] { 1, 3, 7, 9 });
        int resultAsInt = 0;
        for (int i = 0; i < PeselLenght - 1; i++)
        {
            resultAsInt += Char2int(pesel[i]) * weights[i % weights.Length];
        }

        resultAsInt = (10 - resultAsInt % 10) % 10;

        return Int2char(resultAsInt);
    }

    private char Int2char(int a)
    {
        return (char)(a + '0');
    }

    private int Char2int(char a)
    {
        return a - '0';
    }
}
