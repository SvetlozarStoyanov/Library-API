using Library.Core.Common.Helpers;
using Library.Core.Contracts.DbServices;
using Library.Core.Contracts.HelperServices;
using Library.Core.Dto.Checkouts;
using Library.Core.Dto.ClientCards;
using Library.Core.Dto.ClientCardStatusChanges;
using Library.Infrastructure.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/client-cards")]
    public class ClientCardsController : ControllerBase
    {
        private readonly IClientCardService clientCardService;
        private readonly IClientCardTypeService clientCardTypeService;
        private readonly IClientService clientService;
        private readonly IClientCardStatusChangeService clientCardStatusChangeService;
        private readonly ICheckoutService checkoutService;
        private readonly IEnumService enumService;

        public ClientCardsController(IClientCardService clientCardService,
            IClientCardTypeService clientCardTypeService,
            IClientService clientService,
            IClientCardStatusChangeService clientCardStatusChangeService,
            ICheckoutService checkoutService,
            IEnumService enumService)
        {
            this.clientCardService = clientCardService;
            this.clientCardTypeService = clientCardTypeService;
            this.clientService = clientService;
            this.clientCardStatusChangeService = clientCardStatusChangeService;
            this.checkoutService = checkoutService;
            this.enumService = enumService;
        }

        [HttpGet]
        [Route("statuses")]
        public IActionResult GetStatuses()
        {
            IEnumerable<KeyValue> statuses = enumService.GetEnumValues<ClientCardStatus>();
            return Ok(statuses);
        }


        [HttpGet]
        [Route("{id}/checkouts")]
        public async Task<IActionResult> Checkouts([FromRoute] long id,
            [FromQuery] int page = 1,
            [FromQuery] int itemsPerPage = 6)
        {
            IEnumerable<CheckoutListDto> checkouts = await checkoutService.GetClientCardCheckoutsAsync(id, page, itemsPerPage);
            return Ok(checkouts);
        }

        [HttpGet]
        [Route("{id}/status-changes")]
        public async Task<IActionResult> StatusChanges([FromRoute] long id)
        {
            IEnumerable<ClientCardStatusChangeListDto> statusChanges = await clientCardStatusChangeService.GetClientCardStatusChangesAsync(id);
            return Ok(statusChanges);
        }

        [HttpPost]
        [Route("create/{clientId}")]
        public async Task<IActionResult> Create([FromRoute] long clientId, [FromBody] ClientCardCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await clientCardService.ClientHasSameTypeOfCardAsync(clientId, dto.ClientCardTypeId))
            {
                return BadRequest("Client has same type of card already!");
            }

            await clientCardService.CreateCreditClientCardAsync(clientId, dto);

            return Ok();
        }

        [HttpGet]
        [Route("reactivate/{id}")]
        public async Task<IActionResult> Reactivate([FromRoute] long id)
        {
            ClientCardReactivateDto dto = await clientCardService.CreateClientCardReactivateDtoAsync(id);

            if (!(await clientCardService.CanReactivateClientCardAsync(id)))
            {
                return BadRequest("Cannot reactivate client card!");
            }

            return Ok(dto);
        }

        [HttpPatch]
        [Route("reactivate/{id}")]
        public async Task<IActionResult> Reactivate([FromRoute] long id, [FromBody] ClientCardReactivateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await clientCardService.ReactivateClientCardAsync(id);

            return Ok();
        }

        [HttpPatch]
        [Route("deactivate/{id}")]
        public async Task<IActionResult> Deactivate([FromRoute] long id)
        {
            if (await clientCardService.ClientCardHasUnfinalizedCheckoutsAsync(id))
            {
                return BadRequest("Cannot deactivate a card which has unfinalized checkouts!");
            }
            await clientCardService.DeactivateClientCardAsync(id);
            return Ok();
        }

        [HttpPatch]
        [Route("renew/{id}")]
        public async Task<IActionResult> Renew([FromRoute] long id)
        {
            await clientCardService.RenewClientCardAsync(id);
            return Ok();
        }

        [HttpPatch]
        [Route("lose/{id}")]
        public async Task<IActionResult> Lose([FromRoute] long id)
        {
            await clientCardService.LoseClientCardAsync(id);
            return Ok();
        }

        [HttpGet]
        [Route("recover/{id}")]
        public async Task<IActionResult> Recover([FromRoute] long id)
        {
            ClientCardStatusChangeRecoveryDto dto = await clientCardStatusChangeService.CreateClientCardStatusChangeRecoveryDtoAsync(id);
            return Ok(dto);
        }

        [HttpPatch]
        [Route("recover/{id}")]
        public async Task<IActionResult> Recover([FromRoute] long id, [FromBody] ClientCardStatusChangeRecoveryDto dto)
        {
            await clientCardService.RecoverClientCardAsync(id, dto);
            return Ok();
        }

        [HttpPatch]
        [Route("suspend/{id}")]
        public async Task<IActionResult> Suspend([FromRoute] long id)
        {
            await clientCardService.SuspendClientCardAsync(id);
            return Ok();
        }

        [HttpPatch]
        [Route("unsuspend/{id}")]
        public async Task<IActionResult> Unsuspend([FromRoute] long id)
        {
            await clientCardService.UnsuspendClientCardAsync(id);
            return Ok();
        }
    }
}