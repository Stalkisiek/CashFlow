using System.Text.RegularExpressions;

namespace CashFlow.Services.AuthServices;

public class SyntaxChecker
{
    public static bool IsValidEmail(string email)
    {
        // Defining a regular expression pattern for a valid email address
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        // Using Regex.IsMatch to check if the email matches the pattern
        return Regex.IsMatch(email, pattern);
    }
    public static bool IsValidName(string name)
    {
        // Defining a regular expression pattern for a valid name (letters only, minimum 2 characters)
        string pattern = @"^[A-Za-z]{2,}$";

        // Using Regex.IsMatch to check if the name matches the pattern
        return Regex.IsMatch(name, pattern);
    }
}