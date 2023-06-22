public record StudentInClassResponseDto
{
    public int id { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public string code { get; set; }
    public int groupId { get; set; }
    public int groupNumber { get; set; }
    public bool isLeader { get; set; }
}