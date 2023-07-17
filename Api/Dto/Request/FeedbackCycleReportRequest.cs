namespace Api.Dto.Request
{
    public class FeedbackCycleReportRequest
    {
        public string? Feedback { get; set; }

        public int GroupId { get; set; }

        public int ReportId { get; set; }

        public float? Mark { get; set; }
    }
}
