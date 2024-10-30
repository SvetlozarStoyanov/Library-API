using Library.Core.Contracts.DbServices;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/seeding")]
    public class SeedingController : ControllerBase
    {
        private readonly ISeedingService seedingService;

        public SeedingController(ISeedingService seedingService)
        {
            this.seedingService = seedingService;
        }

        [HttpPost]
        [Route("seed")]
        public async Task<IActionResult> SeedDatabase()
        {
            if (await seedingService.CheckDatabaseIsSeededAsync())
            {
                return BadRequest("Database is already seeded!");
            }
            await seedingService.SeedDatabaseAsync();
            return Ok();
        }
    }
}
