namespace Api.Dto.Request
{
    public class CreateProgressReportRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int GroupId { get; set; }

        public override string ToString()
        {
            return $"CreateProgressReportRequest [Title={Title}]";
        }
    }
}
