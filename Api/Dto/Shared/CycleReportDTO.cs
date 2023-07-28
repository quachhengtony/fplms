namespace Api.Dto.Shared
{
    public class CycleReportDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public int? CycleNumber { get; set; }
        public string? Feedback { get; set; }
        public string? ResourceLink { get; set; }
        public float? Mark { get; set; }
        public int? GroupId { get; set; }

        public override string ToString()
        {
            return $"CycleReportDTO [Id={Id}, GroupId={GroupId}]";
        }
    }
}
