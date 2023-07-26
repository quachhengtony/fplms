using System;

namespace Repositories.Dto.Temp
{
    public class GetStudentDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Picture { get; set; }
        public string? Email { get; set; }
        public int Point { get; set; }
    }
}