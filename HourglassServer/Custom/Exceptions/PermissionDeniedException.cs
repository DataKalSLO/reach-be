using System;
using HourglassServer.Data;

public class PermissionDeniedException : Exception
{
    public HourglassError errorObj { get; set; }
    public PermissionDeniedException()
    {
    }

    public PermissionDeniedException(string message, string tag)
        : base(message)
    {
        errorObj = new HourglassError(message, tag);
    }

    public PermissionDeniedException(string message, Exception inner)
        : base(message, inner)
    {
    }
}