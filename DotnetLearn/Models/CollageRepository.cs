namespace DotnetLearn.Models
{
    public class CollageRepository
    {

        public static List<Student> Students { get; set; } = new List<Student>(){
           new Student
            {
                Id=1,
                StudentName="Student1",
                Email="ashutoshmali@gmail.com",
                Address="Hyd,INDIA"

            },
            new Student {
                Id = 2,
                StudentName = "Student2",
                Email = "ashutoshmali@gmail.com",
                Address = "Hyd,INDIA"
            }
            };
    }
}
