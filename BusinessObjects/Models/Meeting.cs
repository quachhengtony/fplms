using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Meeting
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Link { get; set; }

    public string? Feedback { get; set; }

    public DateTime? ScheduleTime { get; set; }

    public int LecturerId { get; set; }

    public int GroupId { get; set; }

    public virtual Group Group { get; set; } = null!;

    public virtual Lecturer Lecturer { get; set; } = null!;
}
