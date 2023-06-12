using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Subject
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}
