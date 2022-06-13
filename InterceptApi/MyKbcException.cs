using Microsoft.VisualBasic;

namespace InterceptApi;

public class MyKbcException : Exception
{
    public MyKbcException(string? msg, Exception innerException, string uri) : base(msg,innerException)
    {
        Uri = uri;
    }
    /// <summary>
    /// unique error ID
    /// </summary>
    public string Uri { get; set; }
}