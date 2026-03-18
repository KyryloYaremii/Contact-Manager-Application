namespace ContactManagerApp.Shared.Exceptions;

public class UnsupportedFileTypeException : Exception
{
    public UnsupportedFileTypeException(string message) : base(message) { }
}
