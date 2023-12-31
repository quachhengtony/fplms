﻿using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Student
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Name { get; set; }

    public string? ImageUrl { get; set; }

    public sbyte IsDisable { get; set; }

    public int Point { get; set; } = 0;

    public virtual ICollection<ProgressReport> ProgressReports { get; set; } = new List<ProgressReport>();

    public virtual ICollection<StudentGroup> StudentGroups { get; set; } = new List<StudentGroup>();

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public ICollection<Question>? Questions { get; set; }
    public ICollection<Answer>? Answers { get; set; }
    public ICollection<StudentUpvote> UpvotedQuestions { get; set; }
    public ICollection<StudentAnswerUpvote> UpvotedAnswers { get; set; }
}
