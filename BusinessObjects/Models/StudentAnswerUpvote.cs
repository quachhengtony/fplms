using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.Models;

[Table("student_answer_upvote")]
public partial class StudentAnswerUpvote
{
    public int StudentId { get; set; }
    public Student Student { get; set; }

    public Guid AnswerId { get; set; }
    public Answer Answer { get; set; }
}
