using System;

namespace Api.Dto.Temp
{
    public class GetLecturerDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Picture { get; set; }
        public string? Email { get; set; }
    }
}