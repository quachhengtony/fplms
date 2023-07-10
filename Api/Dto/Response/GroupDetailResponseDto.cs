using Api.Dto.Shared;
using System;
using System.Collections.Generic;

namespace Api.Dto.Response
{
    public class GroupDetailResponseDto
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int MemberQuantity { get; set; }
        public int CurrentNumber { get; set; }
        public DateTime? EnrollTime { get; set; }
        public ProjectDto? ProjectDTO { get; set; }
        public int LeaderId { get; set; }
        public bool IsDisable { get; set; }
        public HashSet<StudentDto>? StudentDtoSet { get; set; }
    }
}
