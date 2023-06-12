using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Project
{
    public int Id { get; set; }

    public string? Theme { get; set; }

    public string? Name { get; set; }

    public string? Problem { get; set; }

    public string? Context { get; set; }

    public string? Actors { get; set; }

    public string? Requirements { get; set; }

    public int SubjectId { get; set; }

    public int? LecturerId { get; set; }

    public sbyte IsDisable { get; set; }

    public virtual Subject Subject { get; set; } = null!;
}
