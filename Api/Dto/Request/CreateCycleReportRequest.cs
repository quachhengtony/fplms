namespace Api.Dto.Request
{
    public class CreateCycleReportRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string ResourceLink { get; set; }
        public int GroupId { get; set; }

        public override string ToString()
        {
            return $"CreateCycleReportRequest [Title={Title}]";
        }
    }
}
