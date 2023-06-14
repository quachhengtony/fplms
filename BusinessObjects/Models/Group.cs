using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Group
{
    public int Id { get; set; }

    public int? Number { get; set; }

    public int? MemberQuantity { get; set; }

    public DateTime? EnrollTime { get; set; }

    public int? ProjectId { get; set; }

    public int ClassId { get; set; }

    public sbyte? IsDisable { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual ICollection<CycleReport> CycleReports { get; set; } = new List<CycleReport>();

    public virtual ICollection<Meeting> Meetings { get; set; } = new List<Meeting>();

    public virtual ICollection<ProgressReport> ProgressReports { get; set; } = new List<ProgressReport>();

    public virtual Project? Project { get; set; }

    public virtual ICollection<StudentGroup> StudentGroups { get; set; } = new List<StudentGroup>();
}
