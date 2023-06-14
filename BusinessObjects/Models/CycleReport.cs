using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class CycleReport
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public int CycleNumber { get; set; }

    public string? ResourceLink { get; set; }

    public string? Feedback { get; set; }

    public float? Mark { get; set; }

    public int GroupId { get; set; }

    public virtual Group Group { get; set; } = null!;
}
