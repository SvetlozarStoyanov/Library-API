using Library.Core.Contracts.DbServices;
using Library.Core.Dto.Checkouts;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/checkouts")]
    public class CheckoutsController : ControllerBase
    {
        private readonly ICheckoutService checkoutService;
        private readonly IClientCardService clientCardService;
        private readonly IClientService clientService;
        private readonly IBookService bookService;

        public CheckoutsController(ICheckoutService checkoutService,
            IClientCardService clientCardService,
            IClientService clientService,
            IBookService bookService)
        {
            this.checkoutService = checkoutService;
            this.clientCardService = clientCardService;
            this.bookService = bookService;
            this.clientService = clientService;
        }

        [HttpGet]
        [Route("filter")]
        public async Task<IActionResult> Filter([FromQuery] CheckoutsFilterDto dto)
        {
            IEnumerable<CheckoutListDto> checkouts = await checkoutService.GetFilteredCheckoutsAsync(dto);

            return Ok(checkouts);
        }

        [HttpGet]
        [Route("details/{id}")]
        public async Task<IActionResult> Details([FromRoute] long id)
        {
            CheckoutDetailsDto dto = await checkoutService.GetCheckoutByIdAsync(id);
            return Ok(dto);
        }

        //[HttpGet]
        //[Route("create/{clientCardId}")]
        //public async Task<IActionResult> Create([FromRoute] long clientCardId)
        //{
        //    CheckoutCreateDto dto = await checkoutService.CreateCheckoutCreateDtoAsync(clientCardId);

        //    long clientId = await clientCardService.GetClientCardClientIdAsync(clientCardId);
        //    if (await clientService.ClientHasUnpaidFinesAsync(clientId))
        //    {
        //        return BadRequest("Client is not eligible to checkout books!");
        //    }

        //    return Ok(dto);
        //}

        [HttpPost]
        [Route("create/{clientCardId}")]
        public async Task<IActionResult> Create([FromRoute] long clientCardId, [FromBody] CheckoutCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           
            long clientId = await clientCardService.GetClientCardClientIdAsync(clientCardId);
            if (await clientService.ClientHasUnpaidFinesAsync(clientId))
            {
                return BadRequest("Client is not eligible to checkout books!");
            }
            await checkoutService.CreateCheckoutAsync(clientCardId, dto);
            return Ok();
        }

        [HttpGet]
        [Route("finalize/{id}")]
        public async Task<IActionResult> Finalize([FromRoute] long id)
        {
            CheckoutFinalizationDto dto = await checkoutService.CreateCheckoutFinalizationDto(id);
            return Ok(dto);
        }

        [HttpPatch]
        [Route("finalize/{id}")]
        public async Task<IActionResult> Finalize([FromRoute] long id, [FromBody] CheckoutFinalizationDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await checkoutService.FinalizeCheckoutAsync(id, dto);
            return Ok();
        }
    }
}
