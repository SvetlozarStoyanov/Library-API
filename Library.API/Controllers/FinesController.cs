using Library.Core.Common.Helpers;
using Library.Core.Contracts.DbServices;
using Library.Core.Contracts.HelperServices;
using Library.Core.Dto.Fines;
using Library.Infrastructure.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/fines")]
    public class FinesController : ControllerBase
    {
        private readonly IFineService fineService;
        private readonly ICheckoutService checkoutService;
        private readonly IEnumService enumService;

        public FinesController(IFineService fineService,
            ICheckoutService checkoutService,
            IEnumService enumService)
        {
            this.fineService = fineService;
            this.checkoutService = checkoutService;
            this.enumService = enumService;
        }

        [HttpGet]
        [Route("reasons")]
        public IActionResult GetReasons()
        {
            IEnumerable<KeyValue> reasons = enumService.GetEnumValues<FineReason>();
            return Ok(reasons);
        }

        [HttpGet]
        [Route("statuses")]
        public IActionResult GetStatuses()
        {
            IEnumerable<KeyValue> reasons = enumService.GetEnumValues<FineStatus>();
            return Ok(reasons);
        }

        [HttpGet]
        [Route("history/{code}")]
        public async Task<IActionResult> History([FromRoute] string code)
        {
            IEnumerable<FineListDto> fines = await fineService.GetFineHistoryByCodeAsync(code);

            return Ok(fines);
        }

        //[HttpGet]
        //[Route("create/{checkoutId}")]
        //public async Task<IActionResult> Create([FromRoute] long checkoutId)
        //{
        //    if (await fineService.CheckoutHasUnpaidFineAsync(checkoutId))
        //    {
        //        return BadRequest("Checkout already has an unpaid fine!");
        //    }

        //    FineCreateDto? dto = await fineService.CreateFineCreateDtoAsync(checkoutId);
        //    return Ok(dto);
        //}

        [HttpPost]
        [Route("create/{checkoutId}")]
        public async Task<IActionResult> Create([FromRoute] long checkoutId, [FromBody] FineCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await fineService.CreateFineAsync(dto);
            return Created();
        }

        [HttpGet]
        [Route("pay/{id}")]
        public async Task<IActionResult> Pay([FromRoute] long id)
        {
            FinePaymentDto? dto = await fineService.CreateFinePaymentDtoAsync(id);
            return Ok(dto);
        }

        [HttpPost]
        [Route("pay/{id}")]
        public async Task<IActionResult> Pay([FromRoute] long id, [FromBody] FinePaymentDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await fineService.PayFineAsync(id, dto);
            return Ok();
        }

        [HttpGet]
        [Route("adjust/{id}")]
        public async Task<IActionResult> Adjust([FromRoute] long id)
        {
            FineAdjustmentDto dto = await fineService.CreateFineAdjustmentDtoAsync(id);
            return Ok(dto);
        }

        [HttpPatch]
        [Route("adjust/{id}")]
        public async Task<IActionResult> Adjust([FromRoute] long id, [FromBody] FineAdjustmentDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await fineService.AdjustFineAsync(id, dto);
            return Ok();
        }

        [HttpGet]
        [Route("waive/{id}")]
        public async Task<IActionResult> Waive([FromRoute] long id)
        {
            FineWaiverDto? dto = await fineService.CreateFineWaiveDtoAsync(id);
            return Ok(dto);
        }

        [HttpPatch]
        [Route("waive/{id}")]
        public async Task<IActionResult> Waive([FromRoute] long id, [FromBody] FineWaiverDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await fineService.WaiveFineAsync(id, dto);
            return Ok();
        }
    }
}
