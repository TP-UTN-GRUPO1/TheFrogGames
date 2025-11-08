namespace Application.Helpers;

public static class PatchHelper
{
    public static string KeepIfEmpty(string current, string newValue)
        => string.IsNullOrWhiteSpace(newValue) ? current : newValue;
}
