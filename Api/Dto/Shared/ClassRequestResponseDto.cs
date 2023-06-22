public record ClassRequestResponseDto
{
    public int id { get; set; }
    public string name { get; set; }
    public int cycleDuration { get; set; }
    public string semesterCode { get; set; }
    public string enrollKey { get; set; }
    public int subjectId { get; set; }
}