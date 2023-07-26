using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.Models;

[Table("student_upvote")]
public partial class StudentUpvote
{
    public int StudentId { get; set; }
    public Student Student { get; set; }

    public Guid QuestionId { get; set; }
    public Question Question { get; set; }
}
