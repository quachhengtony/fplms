
namespace FPLMS.Api.Dto;

public record ResponseDto<T>
{

    public int code { get; set; }
    public string message { get; set; }
    public T? data { get; set; }
    public ResponseDto() { }
    public ResponseDto(int code, string message, T data)
    {
        code = code;
        message = message;
        data = data;
    }
    public ResponseDto(int code, string message)
    {
        code = code;
        message = message;
    }
}