using Microsoft.AspNetCore.Mvc;
using Salon.DataContext;
using Salon.WebApi.Repositories;
using System.Runtime.CompilerServices;

namespace Salon.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServiceRepository repo;

        public ServicesController(IServiceRepository repo)
        {
            this.repo = repo;
        }


        //get api/services
        //get api/services/?title=[ServiceName]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PriceList>))]
        public async Task<IEnumerable<PriceList>> GetServices(string? title)
        {
            if (title is null)
            {
                return await repo.RetrieveAllAsync();
            }
            else
            {
                return (await repo.RetrieveAllAsync()).Where(p=>p.Service.Contains(title));
            }
        }

        //get api/services/[id]
        [HttpGet("{id}", Name = nameof(GetService))]
        [ProducesResponseType(200, Type = typeof(PriceList))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetService(short id)
        {
            PriceList? c = await repo.RetrieveAsync(id);
            if (c is null) 
            {
                return NotFound();
            }
            else
            return Ok(c);
        }

        //post api/services
        //body Service(JSON)
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(PriceList))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] PriceList priceList)
        {
            if (priceList is null)
            {
                return BadRequest();
            }
            PriceList? itemtoadd = new()
            {
                ServiceId = (short)((await repo.GetMaxIdAsync()).GetValueOrDefault()+1),
                Service = priceList.Service,
                Price = priceList.Price
            };
            PriceList? serviceadded = await repo.CreateAsync(itemtoadd);

            if (serviceadded is null)
            {
                return BadRequest();
            }
            else
            {
                return CreatedAtRoute(
                    nameof(GetService),
                    new { id = serviceadded.ServiceId },
                    serviceadded);
            }
        }

        //put api/services/[id]
        //body: Service (JSON)
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Update(
            short id, [FromBody] PriceList priceList)
        {
            if (priceList is null || priceList.ServiceId != id)
            {
                return BadRequest(); //400
            }
            PriceList? existing = await repo.RetrieveAsync(id);
            if (existing is null)
            {
                return NotFound(); //404
            }
            await repo.UpdateAsync(id, priceList);
            return new NoContentResult(); //204
        }
        //Delete: api/workers/[id]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(short id)
        {
            PriceList? existing = await repo.RetrieveAsync(id);
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
                return BadRequest("Service was failed to delete"); //400
            }
        }

    }
}
