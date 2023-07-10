using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class ProgressReport
{
    private int reportId;

    public ProgressReport(int reportId)
    {
        this.reportId = reportId;
    }

    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public DateTime ReportTime { get; set; }

    public int StudentId { get; set; }

    public int GroupId { get; set; }

    public virtual Group Group { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
