using System;

namespace Api.Dto.Request
{
    public class CreateGroupRequestDto
    {
        public int MemberQuantity { get; set; }
        public DateTime EnrollTime { get; set; }
        public int GroupQuantity { get; set; }
        public int ClassId { get; set; }
    }
}
