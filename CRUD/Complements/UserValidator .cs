using System.Text.RegularExpressions;
using System;

public class UserValidator : IUserValidator
{
    public bool IsValidPassword(string password)
    {
        var passwordRegex = new Regex(@"^(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{8,}$");
        return passwordRegex.IsMatch(password);
    }

    public bool IsValidStatus(string status)
    {
        return status == "Active" || status == "Inactive";
    }

    public bool IsValidName(string name)
    {
        return !string.IsNullOrWhiteSpace(name) && name.Length >= 2;
    }

    public bool IsAdult(DateTime birthDate)
    {
        var today = DateTime.Today;
        var age = today.Year - birthDate.Year;
        if (birthDate > today.AddYears(-age)) age--;
        return age >= 18;
    }
}
