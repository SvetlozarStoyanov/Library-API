using Library.Core.Common.Helpers;
using Library.Core.Contracts.DbServices;
using Library.Core.Contracts.HelperServices;
using Library.Core.Dto.Addresses;
using Library.Infrastructure.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/addresses")]
    public class AddressesController : ControllerBase
    {
        private readonly IAddressService addressService;
        private readonly IClientService clientService;
        private readonly IEnumService enumService;

        public AddressesController(IAddressService addressService,
            IClientService clientService,
            IEnumService enumService)
        {
            this.addressService = addressService;
            this.clientService = clientService;
            this.enumService = enumService;
        }

        [HttpGet]
        [Route("types")]
        public IActionResult GetAddressTypes()
        {
            IEnumerable<KeyValue> types = enumService.GetEnumValues<AddressType>();
            return Ok(types);
        }

        [HttpPost]
        [Route("create/{clientId}")]
        public async Task<IActionResult> Create([FromRoute] long clientId, [FromBody] AddressCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            await addressService.CreateAddressAsync(clientId, dto);
            return Created();
        }

        [HttpGet]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] long id)
        {
            AddressEditDto dto = await addressService.CreateAddressEditDtoAsync(id);
            return Ok(dto);
        }

        [HttpPut]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] long id, AddressEditDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await addressService.UpdateAddressAsync(id, dto);
            return Created();
        }

        [HttpPatch]
        [Route("change-residency-address/clientId={clientId}&addressId={addressId}")]
        public async Task<IActionResult> ChangeResidencyAddress([FromRoute] long clientId,
            [FromRoute] long addressId)
        {
            if (!(await clientService.ClientExistsAsync(clientId)))
            {
                return NotFound("Client was not found!");
            }

            await addressService.ChangeClientResidencyAddressAsync(clientId, addressId);
            return Ok();
        }

        [HttpPatch]
        [Route("archive/{id}")]
        public async Task<IActionResult> Archive([FromRoute] long id)
        {
            await addressService.ArchiveAddressAsync(id);
            return NoContent();
        }

        //[HttpDelete]
        //[Route("delete/{id}")]
        //public async Task<IActionResult> Delete([FromRoute] long id)
        //{
        //    await addressService.DeleteAddressAsync(id);
        //    return Ok();
        //}
    }
}
