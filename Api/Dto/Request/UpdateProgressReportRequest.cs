using System.ComponentModel.DataAnnotations;

namespace Api.Dto.Request
{
    public class UpdateProgressReportRequest : CreateProgressReportRequest
    {
        [Required]
        public int Id { get; set; }
    }
}
