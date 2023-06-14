using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Semester
{
    public string Code { get; set; } = null!;

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}
