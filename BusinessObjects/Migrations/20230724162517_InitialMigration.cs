using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "lecturer",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    email = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    name = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true),
                    imageurl = table.Column<string>(name: "image_url", type: "text", nullable: true),
                    isdisable = table.Column<sbyte>(name: "is_disable", type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "semester",
                columns: table => new
                {
                    code = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    startdate = table.Column<DateTime>(name: "start_date", type: "date", nullable: true),
                    enddate = table.Column<DateTime>(name: "end_date", type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.code);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "student",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    code = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    email = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    name = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true),
                    imageurl = table.Column<string>(name: "image_url", type: "text", nullable: true),
                    isdisable = table.Column<sbyte>(name: "is_disable", type: "tinyint", nullable: false),
                    Point = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "subject",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    isdisable = table.Column<sbyte>(name: "is_disable", type: "tinyint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "class",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true),
                    enrollkey = table.Column<string>(name: "enroll_key", type: "varchar(45)", maxLength: 45, nullable: true),
                    cycleduration = table.Column<int>(name: "cycle_duration", type: "int", nullable: true),
                    isdisable = table.Column<sbyte>(name: "is_disable", type: "tinyint", nullable: true, defaultValueSql: "'0'"),
                    SUBJECTid = table.Column<int>(name: "SUBJECT_id", type: "int", nullable: false),
                    LECTURERid = table.Column<int>(name: "LECTURER_id", type: "int", nullable: false),
                    SEMESTERcode = table.Column<string>(name: "SEMESTER_code", type: "varchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "fk_CLASS_LECTURER1",
                        column: x => x.LECTURERid,
                        principalTable: "lecturer",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_CLASS_SUBJECT1",
                        column: x => x.SUBJECTid,
                        principalTable: "subject",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_class_SEMESTER1",
                        column: x => x.SEMESTERcode,
                        principalTable: "semester",
                        principalColumn: "code");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "project",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    theme = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    problem = table.Column<string>(type: "text", nullable: true),
                    context = table.Column<string>(type: "text", nullable: true),
                    actors = table.Column<string>(type: "text", nullable: true),
                    requirements = table.Column<string>(type: "text", nullable: true),
                    SUBJECTid = table.Column<int>(name: "SUBJECT_id", type: "int", nullable: true),
                    isdisable = table.Column<sbyte>(name: "is_disable", type: "tinyint", nullable: false),
                    LECTURERid = table.Column<int>(name: "LECTURER_id", type: "int", nullable: false),
                    SEMESTERcode = table.Column<string>(name: "SEMESTER_code", type: "varchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "fk_PROJECT_SUBJECT1",
                        column: x => x.SUBJECTid,
                        principalTable: "subject",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_project_lecturer1",
                        column: x => x.LECTURERid,
                        principalTable: "lecturer",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_project_semester1",
                        column: x => x.SEMESTERcode,
                        principalTable: "semester",
                        principalColumn: "code");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "question",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Title = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    Content = table.Column<string>(type: "longtext", maxLength: 20000, nullable: false),
                    Solved = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    Removed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    RemovedBy = table.Column<string>(type: "longtext", nullable: true),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_question", x => x.Id);
                    table.ForeignKey(
                        name: "FK_question_student_StudentId",
                        column: x => x.StudentId,
                        principalTable: "student",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_question_subject_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "subject",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "student_class",
                columns: table => new
                {
                    STUDENTid = table.Column<int>(name: "STUDENT_id", type: "int", nullable: false),
                    CLASSid = table.Column<int>(name: "CLASS_id", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.STUDENTid, x.CLASSid });
                    table.ForeignKey(
                        name: "fk_STUDENT_has_CLASS_CLASS1",
                        column: x => x.CLASSid,
                        principalTable: "class",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_STUDENT_has_CLASS_STUDENT1",
                        column: x => x.STUDENTid,
                        principalTable: "student",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "group",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    number = table.Column<int>(type: "int", nullable: true),
                    memberquantity = table.Column<int>(name: "member_quantity", type: "int", nullable: true),
                    enrolltime = table.Column<DateTime>(name: "enroll_time", type: "timestamp", nullable: true),
                    PROJECTid = table.Column<int>(name: "PROJECT_id", type: "int", nullable: true),
                    CLASSid = table.Column<int>(name: "CLASS_id", type: "int", nullable: false),
                    isdisable = table.Column<sbyte>(name: "is_disable", type: "tinyint", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "fk_GROUP_CLASS1",
                        column: x => x.CLASSid,
                        principalTable: "class",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_GROUP_PROJECT1",
                        column: x => x.PROJECTid,
                        principalTable: "project",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "answer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Content = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    Accepted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Removed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    RemovedBy = table.Column<string>(type: "longtext", nullable: true),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_answer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_answer_question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_answer_student_StudentId",
                        column: x => x.StudentId,
                        principalTable: "student",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "student_upvote",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_upvote", x => new { x.StudentId, x.QuestionId });
                    table.ForeignKey(
                        name: "FK_student_upvote_question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_student_upvote_student_StudentId",
                        column: x => x.StudentId,
                        principalTable: "student",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cycle_report",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(type: "text", nullable: true),
                    content = table.Column<string>(type: "text", nullable: true),
                    cyclenumber = table.Column<int>(name: "cycle_number", type: "int", nullable: false),
                    resourcelink = table.Column<string>(name: "resource_link", type: "text", nullable: true),
                    feedback = table.Column<string>(type: "text", nullable: true),
                    mark = table.Column<float>(type: "float", nullable: true),
                    GROUPid = table.Column<int>(name: "GROUP_id", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "fk_PROGRESS_REPORT_copy1_GROUP1",
                        column: x => x.GROUPid,
                        principalTable: "group",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "meeting",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    link = table.Column<string>(type: "text", nullable: true),
                    feedback = table.Column<string>(type: "text", nullable: true),
                    scheduletime = table.Column<DateTime>(name: "schedule_time", type: "timestamp", nullable: true),
                    LECTURERid = table.Column<int>(name: "LECTURER_id", type: "int", nullable: false),
                    GROUPid = table.Column<int>(name: "GROUP_id", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "fk_MEETING_GROUP1",
                        column: x => x.GROUPid,
                        principalTable: "group",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_MEETING_LECTURER1",
                        column: x => x.LECTURERid,
                        principalTable: "lecturer",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "progress_report",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(type: "text", nullable: true),
                    content = table.Column<string>(type: "text", nullable: true),
                    reporttime = table.Column<DateTime>(name: "report_time", type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    STUDENTid = table.Column<int>(name: "STUDENT_id", type: "int", nullable: false),
                    GROUPid = table.Column<int>(name: "GROUP_id", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "fk_PROGRESS_REPORT_GROUP1",
                        column: x => x.GROUPid,
                        principalTable: "group",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_PROGRESS_REPORT_STUDENT",
                        column: x => x.STUDENTid,
                        principalTable: "student",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "student_group",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    STUDENTid = table.Column<int>(name: "STUDENT_id", type: "int", nullable: false),
                    GROUPid = table.Column<int>(name: "GROUP_id", type: "int", nullable: false),
                    CLASSid = table.Column<int>(name: "CLASS_id", type: "int", nullable: false),
                    isleader = table.Column<sbyte>(name: "is_leader", type: "tinyint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "fk_STUDENT_GROUP_CLASS1",
                        column: x => x.CLASSid,
                        principalTable: "class",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_STUDENT_has_GROUP_GROUP1",
                        column: x => x.GROUPid,
                        principalTable: "group",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_STUDENT_has_GROUP_STUDENT1",
                        column: x => x.STUDENTid,
                        principalTable: "student",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "student_answer_upvote",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    AnswerId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_answer_upvote", x => new { x.StudentId, x.AnswerId });
                    table.ForeignKey(
                        name: "FK_student_answer_upvote_answer_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "answer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_student_answer_upvote_student_StudentId",
                        column: x => x.StudentId,
                        principalTable: "student",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_answer_QuestionId",
                table: "answer",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_answer_StudentId",
                table: "answer",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "fk_CLASS_LECTURER1_idx",
                table: "class",
                column: "LECTURER_id");

            migrationBuilder.CreateIndex(
                name: "fk_class_SEMESTER1_idx",
                table: "class",
                column: "SEMESTER_code");

            migrationBuilder.CreateIndex(
                name: "fk_CLASS_SUBJECT1_idx",
                table: "class",
                column: "SUBJECT_id");

            migrationBuilder.CreateIndex(
                name: "fk_PROGRESS_REPORT_copy1_GROUP1_idx",
                table: "cycle_report",
                column: "GROUP_id");

            migrationBuilder.CreateIndex(
                name: "fk_GROUP_CLASS1_idx",
                table: "group",
                column: "CLASS_id");

            migrationBuilder.CreateIndex(
                name: "fk_GROUP_PROJECT1_idx",
                table: "group",
                column: "PROJECT_id");

            migrationBuilder.CreateIndex(
                name: "email_UNIQUE",
                table: "lecturer",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_MEETING_GROUP1_idx",
                table: "meeting",
                column: "GROUP_id");

            migrationBuilder.CreateIndex(
                name: "fk_MEETING_LECTURER1_idx",
                table: "meeting",
                column: "LECTURER_id");

            migrationBuilder.CreateIndex(
                name: "id_UNIQUE",
                table: "meeting",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_PROGRESS_REPORT_GROUP1_idx",
                table: "progress_report",
                column: "GROUP_id");

            migrationBuilder.CreateIndex(
                name: "fk_PROGRESS_REPORT_STUDENT_idx",
                table: "progress_report",
                column: "STUDENT_id");

            migrationBuilder.CreateIndex(
                name: "fk_project_lecturer1_idx",
                table: "project",
                column: "LECTURER_id");

            migrationBuilder.CreateIndex(
                name: "fk_project_semester1_idx",
                table: "project",
                column: "SEMESTER_code");

            migrationBuilder.CreateIndex(
                name: "fk_PROJECT_SUBJECT1_idx",
                table: "project",
                column: "SUBJECT_id");

            migrationBuilder.CreateIndex(
                name: "IX_question_StudentId",
                table: "question",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_question_SubjectId",
                table: "question",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "email_UNIQUE1",
                table: "student",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_student_answer_upvote_AnswerId",
                table: "student_answer_upvote",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "fk_STUDENT_has_CLASS_CLASS1_idx",
                table: "student_class",
                column: "CLASS_id");

            migrationBuilder.CreateIndex(
                name: "fk_STUDENT_has_CLASS_STUDENT1_idx",
                table: "student_class",
                column: "STUDENT_id");

            migrationBuilder.CreateIndex(
                name: "fk_STUDENT_GROUP_CLASS1_idx",
                table: "student_group",
                column: "CLASS_id");

            migrationBuilder.CreateIndex(
                name: "fk_STUDENT_has_GROUP_GROUP1_idx",
                table: "student_group",
                column: "GROUP_id");

            migrationBuilder.CreateIndex(
                name: "fk_STUDENT_has_GROUP_STUDENT1_idx",
                table: "student_group",
                column: "STUDENT_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_upvote_QuestionId",
                table: "student_upvote",
                column: "QuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cycle_report");

            migrationBuilder.DropTable(
                name: "meeting");

            migrationBuilder.DropTable(
                name: "progress_report");

            migrationBuilder.DropTable(
                name: "student_answer_upvote");

            migrationBuilder.DropTable(
                name: "student_class");

            migrationBuilder.DropTable(
                name: "student_group");

            migrationBuilder.DropTable(
                name: "student_upvote");

            migrationBuilder.DropTable(
                name: "answer");

            migrationBuilder.DropTable(
                name: "group");

            migrationBuilder.DropTable(
                name: "question");

            migrationBuilder.DropTable(
                name: "class");

            migrationBuilder.DropTable(
                name: "project");

            migrationBuilder.DropTable(
                name: "student");

            migrationBuilder.DropTable(
                name: "subject");

            migrationBuilder.DropTable(
                name: "lecturer");

            migrationBuilder.DropTable(
                name: "semester");
        }
    }
}
