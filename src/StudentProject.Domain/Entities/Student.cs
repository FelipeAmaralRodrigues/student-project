namespace StudentProject.Domain.Entities
{
    public class Student
    {
        public long Id { get; set; }
        public Guid UId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public Guid? ThirdPartyUId { get; set; }
    }

    public static class StudentFactory
    {
        public static Student CreateStudent(string firstName, string lastName, DateTime birthDate, string email)
        {
            return new Student
            {
                UId = Guid.NewGuid(),
                FirstName = firstName.Trim(),
                LastName = lastName.Trim(),
                BirthDate = birthDate,
                Email = email.Trim().ToLower()
            };
        }

        public static Student UpdateThirPartyUId(Student student, Guid thirdPartyUId)
        {
            student.ThirdPartyUId = thirdPartyUId;
            return student;
        }
    }
}
