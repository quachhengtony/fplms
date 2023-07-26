using System.ComponentModel.DataAnnotations;

namespace Repositories.Dto.Temp
{
    public record UpdateAnswerDto
    {
        [Required]
        [StringLength(1000)]
        public string? Content { get; set; }
    }
}