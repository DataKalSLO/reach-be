using System;

public class StaleRequestException : Exception
{
    public StaleRequestException() 
    {
    }

    public StaleRequestException(string message)
        : base(message) 
    {
    }

    public StaleRequestException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
