namespace FPLMS.Api.Models;

using System.Text.Json;

public class ErrorBase
{
    public int StatusCode { get; set; }
    public string Message { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}