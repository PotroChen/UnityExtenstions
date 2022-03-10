
using System;

public static class StringExtensions
{
    public static bool IsNullOrEmptyOrWhiteSpace(string value)
    {
        return string.IsNullOrEmpty(value) || value.Trim() == string.Empty;
    }
}
