using System;

namespace Api.Dto.Temp
{
    public class GetStudentDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Picture { get; set; }
        public string? ImageUrl { get; set; }
        public string? Email { get; set; }
        public int Point { get; set; }
    }
}