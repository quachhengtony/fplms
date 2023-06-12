using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Lecturer
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string? Name { get; set; }

    public string? ImageUrl { get; set; }

    public sbyte IsDisable { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual ICollection<Meeting> Meetings { get; set; } = new List<Meeting>();
}
