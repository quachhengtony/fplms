using Api.Dto.Shared;

namespace Api.Dto.Response
{
    public class StudentInClassResponseDto : StudentDto
    {
        public int GroupId { get; set; }
        public int GroupNumber { get; set; }
        public bool IsLeader { get; set; }
    }
}

