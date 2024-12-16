using GymAppReal.Data;
using GymAppReal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymAppReal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExercisesController : ControllerBase
    {
        private readonly GymContext _context;

        public ExercisesController(GymContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExerciseDTO>> GetExercise(int id)
        {
            var exercise = await _context.Exercises.FindAsync(id);

            if (exercise == null)
            {
                return NotFound();
            }

            var exerciseDTO = new ExerciseDTO
            {
                Id = exercise.Id,
                ExerciseName = exercise.ExerciseName,
                Sets = exercise.Sets,
                Reps = exercise.Reps,
                Weight = exercise.Weight,
                Date = exercise.Date,
                UserId = exercise.UserId
            };

            return exerciseDTO;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<ExerciseDTO>>> GetExercisesByUser(int userId)
        {
            var exercises = await _context.Exercises.Where(e => e.UserId == userId).ToListAsync();

            var exerciseDTOs = exercises.Select(e => new ExerciseDTO
            {
                Id = e.Id,
                ExerciseName = e.ExerciseName,
                Sets = e.Sets,
                Reps = e.Reps,
                Weight = e.Weight,
                Date = e.Date,
                UserId = e.UserId
            }).ToList();

            return exerciseDTOs;
        }

        [HttpPost]
        public async Task<ActionResult<ExerciseDTO>> PostExercise(ExerciseDTO exerciseDTO)
        {
            var user = await _context.Users.FindAsync(exerciseDTO.UserId);
            if (user == null)
            {
                return NotFound();
            }

            var exercise = new Exercise
            {
                ExerciseName = exerciseDTO.ExerciseName,
                Sets = exerciseDTO.Sets,
                Reps = exerciseDTO.Reps,
                Weight = exerciseDTO.Weight,
                Date = exerciseDTO.Date,
                UserId = exerciseDTO.UserId,
                User = user
            };

            _context.Exercises.Add(exercise);
            await _context.SaveChangesAsync();

            exerciseDTO.Id = exercise.Id;

            return CreatedAtAction(nameof(GetExercise), new { id = exercise.Id }, exerciseDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutExercise(int id, ExerciseDTO exerciseDTO)
        {
            if (id != exerciseDTO.Id)
            {
                return BadRequest();
            }

            var exercise = await _context.Exercises.FindAsync(id);
            if (exercise == null)
            {
                return NotFound();
            }

            exercise.ExerciseName = exerciseDTO.ExerciseName;
            exercise.Sets = exerciseDTO.Sets;
            exercise.Reps = exerciseDTO.Reps;
            exercise.Weight = exerciseDTO.Weight;
            exercise.Date = exerciseDTO.Date;
            exercise.UserId = exerciseDTO.UserId;

            _context.Entry(exercise).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExerciseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExercise(int id)
        {
            var exercise = await _context.Exercises.FindAsync(id);
            if (exercise == null)
            {
                return NotFound();
            }

            _context.Exercises.Remove(exercise);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExerciseExists(int id)
        {
            return _context.Exercises.Any(e => e.Id == id);
        }
    }
}
