using System;

namespace Api.Dto.Shared
{
    public class ProgressReportDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime? ReportTime { get; set; }
        public int GroupId { get; set; }
        public int StudentId { get; set; }

        public override string ToString()
        {
            return $"ProgressReportDTO [Id={Id}, GroupId={GroupId}, StudentId={StudentId}]";
        }
    }
}
