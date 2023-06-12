using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Class
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Semester { get; set; }

    public string? EnrollKey { get; set; }

    public sbyte? IsDisable { get; set; }

    public int SubjectId { get; set; }

    public int LecturerId { get; set; }

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    public virtual Lecturer Lecturer { get; set; } = null!;

    public virtual ICollection<StudentGroup> StudentGroups { get; set; } = new List<StudentGroup>();

    public virtual Subject Subject { get; set; } = null!;

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
