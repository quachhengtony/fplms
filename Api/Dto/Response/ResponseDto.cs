namespace FPLMS.Api.Dto;

public record ResponseDto<T>
{
    public int code { get; set; }
    public string message { get; set; }
    public T data { get; set; }
}