using System;

namespace Api.Dto.Shared
{
    public class MeetingDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string Feedback { get; set; }
        public DateTime ScheduleTime { get; set; }
        public int LecturerId { get; set; }
        public int GroupId { get; set; }
    }
}
