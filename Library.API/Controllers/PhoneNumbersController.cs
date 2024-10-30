using Library.Core.Common.Helpers;
using Library.Core.Contracts.DbServices;
using Library.Core.Contracts.HelperServices;
using Library.Core.Dto.PhoneNumbers;
using Library.Infrastructure.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [Route("api/phone-numbers")]
    [ApiController]
    public class PhoneNumbersController : ControllerBase
    {
        private readonly IPhoneNumberService phoneNumberService;
        private readonly IEnumService enumService;
        private readonly IClientService clientService;
        private readonly ICountryService countryService;

        public PhoneNumbersController(IPhoneNumberService phoneNumberService,
            IEnumService enumService,
            IClientService clientService,
            ICountryService countryService)
        {
            this.phoneNumberService = phoneNumberService;
            this.enumService = enumService;
            this.clientService = clientService;
            this.countryService = countryService;
        }

        [HttpGet]
        [Route("types")]
        public IActionResult GetTypes()
        {
            IEnumerable<KeyValue> types = enumService.GetEnumValues<PhoneNumberType>();
            return Ok(types);
        }

        //[HttpGet]
        //[Route("create")]
        //public IActionResult Create()
        //{
        //    PhoneNumberCreateDto dto = phoneNumberService.CreatePhoneNumberCreateDto();
        //    return Ok(dto);
        //}

        [HttpPost]
        [Route("create/{clientId}")]
        public async Task<IActionResult> Create([FromRoute] long clientId, [FromBody] PhoneNumberCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await phoneNumberService.CreatePhoneNumberAsync(clientId, dto);
            return Ok();
        }

        [HttpGet]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] long id)
        {
            PhoneNumberEditDto dto = await phoneNumberService.CreatePhoneNumberEditDtoAsync(id);
            return Ok(dto);
        }

        [HttpPut]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] long id, PhoneNumberEditDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await phoneNumberService.UpdatePhoneNumberAsync(id, dto);
            return Ok();
        }

        [HttpPatch]
        [Route("change-main/clientId={clientId}&phoneNumberId={phoneNumberId}")]
        public async Task<IActionResult> ChangeMain([FromRoute] long clientId, long phoneNumberId)
        {
            await phoneNumberService.ChangeClientMainPhoneNumberAsync(clientId, phoneNumberId);
            return Ok();
        }

        [HttpPatch]
        [Route("archive/{id}")]
        public async Task<IActionResult> Archive([FromRoute] long id)
        {
            await phoneNumberService.ArchivePhoneNumberAsync(id);
            return NoContent();
        }
    }
}
