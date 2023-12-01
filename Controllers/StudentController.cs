using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiUniversity.Models;

namespace ApiTodo.Controllers;

[ApiController]
[Route("api/student")]
public class StudentController : ControllerBase
{
    private readonly UniversityContext _context;

    public StudentController(UniversityContext context)
    {
        _context = context;
    }

    // GET: api/student
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudents()
    {
        // Get students
        var students = _context.Students.Select(s => new StudentDTO(s));
        return await students.ToListAsync();
    }

    // GET: api/student/2
    [HttpGet("{id}")]
    public async Task<ActionResult<StudentDTO>> GetStudent(int id)
    {
        // Find todo and related list
        // SingleAsync() throws an exception if no todo is found (which is possible, depending on id)
        // SingleOrDefaultAsync() is a safer choice here
        var students = _context.Students.Select(s => new StudentDTO(s));
        var student = await students.SingleOrDefaultAsync(t => t.Id == id);
        if (student == null)
            return NotFound();

        return student;
    }

    // POST: api/student
    [HttpPost]
    public async Task<ActionResult<StudentDTO>> PostStudent(StudentDTO studentDTO)
    {
        var student = new Student(studentDTO);
        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
    }

    // PUT: api/student/2
    [HttpPut("{id}")]
    public async Task<IActionResult> PutStudent(int id, StudentDTO studentDTO)
    {
        var student = new Student(studentDTO);
        if (id != student.Id)
            return BadRequest();

        _context.Entry(student).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Students.Any(m => m.Id == id))
                return NotFound();
            else
                throw;
        }

        return NoContent();
    }

    // DELETE: api/student/2
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudentItem(int id)
    {
        var student = await _context.Students.FindAsync(id);

        if (student == null)
            return NotFound();

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}