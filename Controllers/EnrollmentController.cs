using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiUniversity.Models;

namespace ApiTodo.Controllers;

[ApiController]
[Route("api/enrollment")]
public class EnrollmentController : ControllerBase
{
    private readonly UniversityContext _context;

    public EnrollmentController(UniversityContext context)
    {
        _context = context;
    }

    // GET: api/enrollment/2
    [HttpGet("{id}")]
    public async Task<ActionResult<DetailedEnrollmentDTO>> GetEnrollment(int id)
    {
        // Find todo and related list
        // SingleAsync() throws an exception if no todo is found (which is possible, depending on id)
        // SingleOrDefaultAsync() is a safer choice here
        // var enrollments = _context.Enrollments.Include(e => e.Student).Include(e => e.Course).Select(s => new DetailedEnrollmentDTO(s));
        // var enrollment = await enrollments.SingleOrDefaultAsync(t => t.Id == id);

        var enrollment = await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .SingleOrDefaultAsync(t => t.Id == id);

        if (enrollment == null)
            return NotFound();

        var detailedEnrollmentDTO = new DetailedEnrollmentDTO(enrollment);

        return detailedEnrollmentDTO;
    }

    // POST: api/enrollment
    [HttpPost]
    public async Task<ActionResult<DetailedEnrollmentDTO>> PostEnrollment(EnrollmentDTO enrollmentDTO)
    {
        var enrollment = new Enrollment(enrollmentDTO);
        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEnrollment), new { id = enrollment.Id }, enrollment);
    }
}