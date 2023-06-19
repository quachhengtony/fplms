using System.ComponentModel.DataAnnotations;

namespace FPLMS.Api.Dto;

public record LoginRequestDto
{
    public string? Provider { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string? IdToken { get; set; }
}