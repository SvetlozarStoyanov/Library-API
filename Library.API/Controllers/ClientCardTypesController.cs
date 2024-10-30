using Library.Core.Contracts.DbServices;
using Library.Core.Dto.ClientCardTypes;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/client-card-types")]
    public class ClientCardTypesController : ControllerBase
    {
        private readonly IClientCardTypeService clientCardTypeService;

        public ClientCardTypesController(IClientCardTypeService clientCardTypeService)
        {
            this.clientCardTypeService = clientCardTypeService;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> All()
        {
            IEnumerable<ClientCardTypeListDto> clientCardTypes = await clientCardTypeService.GetAllClientCardTypesAsync();
            return Ok(clientCardTypes);
        }
    }
}
