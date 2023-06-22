namespace FPLMS.Api.Dto;

public record LoginResponseDto
{
    public bool IsAuthSuccessful { get; set; }
    public string ErrorMessage { get; set; }
    public string Token { get; set; }
}