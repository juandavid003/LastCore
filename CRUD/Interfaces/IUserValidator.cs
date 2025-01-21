using System;

public interface IUserValidator
{
    bool IsValidPassword(string password);
    bool IsValidStatus(string status);
    bool IsValidName(string name);
    bool IsAdult(DateTime birthDate);
}
