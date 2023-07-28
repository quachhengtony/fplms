using System;

namespace Api.Dto.Shared
{
    public class GroupDto
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int MemberQuantity { get; set; }
        public bool IsJoin { get; set; }
        public DateTime EnrollTime { get; set; }
        public bool IsDisable { get; set; }
        public ProjectDto? ProjectDto { get; set; }

    }
}
