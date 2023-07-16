namespace Api.Dto.Shared
{
    public class ProjectDTO
    {
        public int Id { get; set; }
        public string Theme { get; set; }
        public string Name { get; set; }
        public string Problem { get; set; }
        public string Context { get; set; }
        public string Actors { get; set; }
        public string Requirements { get; set; }
        public int SubjectId { get; set; }
        public string SemesterCode { get; set; }

        public override string ToString()
        {
            return $"ProjectDTO [Id={Id}, Name={Name}]";
        }
    }
}
