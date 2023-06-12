using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class StudentGroup
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public int GroupId { get; set; }

    public int ClassId { get; set; }

    public sbyte? IsLeader { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual Group Group { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
