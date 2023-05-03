using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Salon.DataContext;
using Salon.WebApi.Repositories;

namespace Salon.WebApi.Controllers
{
    // api/workers
    [Route("api/[controller]")]
    [ApiController]
    public class WorkersController : ControllerBase
    {
        private readonly IWorkerRepository repo;

        public WorkersController(IWorkerRepository repo) 
        {
            this.repo= repo;
        }

        //Get api/workers
        //Get api/workers/?name=[FullName]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Worker>))]
        public async Task<IEnumerable<Worker>> GetWorkers(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return await repo.RetrieveAllAsync();
            }
            else
            {
                return (await repo.RetrieveAllAsync())
                    .Where(p => p.FullName.Contains(name));
            }
        }

        //Get api/workers/[id]
        [HttpGet("{id}", Name = nameof(GetWorker))]
        [ProducesResponseType(200, Type = typeof(Worker))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetWorker(string id)
        {
            Worker? c = await repo.RetrieveAsync(id);
            if (c is null)
            {
                return NotFound(); //404
            }
            return Ok(c); //200
        }

        //POST: api/workers
        //BODY: Customer (JSON)
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Worker))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] Worker worker)
        {
            if (worker is null)
            {
                return BadRequest();
            }
            Worker? addedWorker = await repo.CreateAsync(worker);

            if (addedWorker is null)
            {
                return BadRequest("Repository failed to create");
            }
            else
            {
                return CreatedAtRoute( //201 - ресурс создан
                    nameof(GetWorker),
                    new { id = addedWorker.PatentId.ToLower() },
                    addedWorker);
            }
        }

        //Put: api/workers/[id]
        //Body: Worker (JSON)
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(
            string id, [FromBody] Worker worker)
        {
            id = id.ToUpper();
            worker.PatentId = worker.PatentId.ToUpper();
            if (worker == null || worker.PatentId!=id)
            {
                return BadRequest(); //400
            }
            Worker? existing = await repo.RetrieveAsync(id);
            if (existing is null)
            {
                return NotFound(); //404
            }
            await repo.UpdateAsync(id, worker);
            return new NoContentResult(); //204
        }

        //Delete: api/workers/[id]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(string id)
        {
            Worker? existing = await repo.RetrieveAsync(id);
            if (existing is null)
            {
                return NotFound(); //404
            }
            
            bool? deleted = await repo.DeleteAsync(id);

            if (deleted.HasValue && deleted.Value)
            {
                return new NoContentResult(); //204
            }
            else
            {
                return BadRequest("Worker was failed to delete"); //400
            }
        }

    }
}
