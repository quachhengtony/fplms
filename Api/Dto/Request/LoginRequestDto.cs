namespace FPLMS.Api.Dto

public record LoginRequestDto
{
    public string Provider { get; set; }
    public string IdToken { get; set; }
}