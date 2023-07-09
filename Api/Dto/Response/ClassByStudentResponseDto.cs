using Api.Dto.Shared;

namespace Api.Dto.Response
{
    public class ClassByStudentResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SemesterCode { get; set; }
        public bool HasEnrollKey { get; set; }
        public int SubjectId { get; set; }
        public LecturerDto LecturerDto { get; set; }
        public bool IsJoin { get; set; }
    }
}
