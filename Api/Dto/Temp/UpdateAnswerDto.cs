using System.ComponentModel.DataAnnotations;

namespace Api.Dto.Temp
{
    public record UpdateAnswerDto
    {
        [Required]
        [StringLength(1000)]
        public string? Content { get; set; }
    }
}