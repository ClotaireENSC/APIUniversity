namespace ApiUniversity.Models;

// Data Transfer Object class, used to bypass navigation properties validation during API calls
public class StudentDTO
{
    public int Id { get; set; }
    public string LastName { get; set; } = null!;
    public string FirstName { get; set; } = null!;

    public StudentDTO() { }

    public StudentDTO(Student student)
    {
        Id = student.Id;
        LastName = student.LastName;
        FirstName = student.FirstName;
    }
}