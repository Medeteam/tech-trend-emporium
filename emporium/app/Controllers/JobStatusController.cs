using Data.Entities;
using Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JobStatusController : ControllerBase
    {
        private readonly DBContextTechEmporiumTrend _context;

        public JobStatusController(DBContextTechEmporiumTrend context)
        {
            _context = context;
        }

        [HttpPost("create-default-job-status")]
        public async Task<IActionResult> CreateDefaultJobStatus()
        {
            // Definir los JobStatus predeterminados
            var defaultJobStatuses = new List<string> { "Pending", "Completed", "Accepted" , "Declined"};

            // Verificar cuáles ya existen en la base de datos
            var existingJobStatuses = _context.Jobs
                                        .Where(js => defaultJobStatuses.Contains(js.Job_status))
                                        .Select(js => js.Job_status)
                                        .ToList();

            // Filtrar los JobStatus que aún no existen
            var jobStatusesToCreate = defaultJobStatuses.Except(existingJobStatuses).ToList();

            // Crear y agregar los JobStatus que faltan
            foreach (var jobStatus in jobStatusesToCreate)
            {
                var newJobStatus = new JobStatus { Job_status = jobStatus };
                _context.Jobs.Add(newJobStatus);
            }

            // Guardar los cambios en la base de datos
            if (jobStatusesToCreate.Count > 0)
            {
                await _context.SaveChangesAsync();
                return Ok($"Job statuses created: {string.Join(", ", jobStatusesToCreate)}");
            }
            else
            {
                return Ok("All job statuses already exist.");
            }
        }

        [HttpGet("get-all-job-statuses")]
        public IActionResult GetAllJobStatuses()
        {
            // Obtener todos los JobStatus de la base de datos
            var jobStatuses = _context.Jobs.ToList();

            if (jobStatuses == null || jobStatuses.Count == 0)
            {
                return NotFound("No job statuses found in the database.");
            }

            return Ok(jobStatuses);
        }
    }
}
