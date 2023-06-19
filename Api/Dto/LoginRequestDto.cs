using System.ComponentModel.DataAnnotations;

namespace FPLMS.Api.Dto;

public record LoginRequestDto
{
    [Required]
    public string Provider { get; set; }
    [Required]
    public string IdToken { get; set; }
}