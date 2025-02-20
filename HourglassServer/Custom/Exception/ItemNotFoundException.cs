using System;
using HourglassServer.Data;

public class ItemNotFoundException : Exception
{
    public HourglassException errorObj { get; set; }
    public ItemNotFoundException()
    {
    }

    public ItemNotFoundException(string message, string tag)
        : base(message)
    {
        errorObj = new HourglassException(message, tag);
    }

    public ItemNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }
}