using System;

public class ColumnNotFoundException : Exception
{
    public ColumnNotFoundException() 
    {
    }

    public ColumnNotFoundException(string message)
        : base(message) 
    {
    }

    public ColumnNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }
}