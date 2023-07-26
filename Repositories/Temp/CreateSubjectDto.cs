using System.ComponentModel.DataAnnotations;

namespace Repositories.Dto.Temp
{
    public class CreateSubjectDto
    {
        [Required]
        [StringLength(7)]
        public string? Name { get; set; }
    }
}