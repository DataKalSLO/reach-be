using System;
using HourglassServer.Data;

public class ItemNotFoundException : Exception
{
    public HourglassError errorObj { get; set; }
    public ItemNotFoundException()
    {
    }

    public ItemNotFoundException(string message, string tag)
        : base(message)
    {
        errorObj = new HourglassError(message, tag);
    }

    public ItemNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }
}