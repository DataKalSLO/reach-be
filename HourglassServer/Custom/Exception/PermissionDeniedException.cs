using System;
using HourglassServer.Data;

public class PermissionDeniedException : Exception
{
    public HourglassException errorObj { get; set; }
    public PermissionDeniedException()
    {
    }

    public PermissionDeniedException(string message, string tag)
        : base(message)
    {
        errorObj = new HourglassException(message, tag);
    }

    public PermissionDeniedException(string message, Exception inner)
        : base(message, inner)
    {
    }
}