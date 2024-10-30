using Library.Core.Contracts.DbServices;
using Library.Core.Dto.Addresses;
using Library.Core.Dto.ClientCards;
using Library.Core.Dto.Clients;
using Library.Core.Dto.Emails;
using Library.Core.Dto.Fines;
using Library.Core.Dto.PhoneNumbers;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/clients")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService clientService;
        private readonly IClientCardService clientCardService;
        private readonly IAddressService addressService;
        private readonly IEmailService emailService;
        private readonly IPhoneNumberService phoneNumberService;
        private readonly IFineService fineService;

        public ClientsController(IClientService clientService,
            IClientCardService clientCardService,
            IAddressService addressService,
            IEmailService emailService,
            IPhoneNumberService phoneNumberService,
            IFineService fineService)
        {
            this.clientService = clientService;
            this.clientCardService = clientCardService;
            this.addressService = addressService;
            this.emailService = emailService;
            this.phoneNumberService = phoneNumberService;
            this.fineService = fineService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("all")]
        public async Task<IActionResult> All()
        {
            IEnumerable<ClientListDto> clients = await clientService.GetAllClientsAsync();
            return Ok(clients);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("details/{id}")]
        public async Task<IActionResult> Details(long id)
        {
            ClientDetailsDto dto = await clientService.GetClientByIdAsync(id);
            return Ok(dto);
        }

        [HttpGet]
        [Route("{id}/client-cards")]
        public async Task<IActionResult> ListClientCardsForClient([FromRoute] long id)
        {
            IEnumerable<ClientCardListDto> dto = await clientCardService.GetClientCardsByClientIdAsync(id);
            return Ok(dto);
        }

        //[HttpGet]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[Route("create")]
        //public IActionResult Create()
        //{
        //    ClientCreateDto dto = clientService.CreateClientCreateDtoAsync();
        //    return Ok(dto);
        //}

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] ClientCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await clientService.CreateClientAsync(dto);

            return Created();
        }

        [HttpGet]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] long id)
        {
            ClientEditDto dto = await clientService.CreateClientEditDtoAsync(id);
            return Ok(dto);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] long id, [FromBody] ClientEditDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await clientService.UpdateClientAsync(id, dto);
            return Created();
        }

        [HttpGet]
        [Route("{id}/addresses")]
        public async Task<IActionResult> Addresses([FromRoute] long id)
        {
            IEnumerable<AddressListDto> addresses = await addressService.GetClientAddressesAsync(id);
            return Ok(addresses);
        }

        [HttpGet]
        [Route("{id}/addresses/edit")]
        public async Task<IActionResult> EditAddresses([FromRoute] long id)
        {
            ClientAddressesEditDto dto = await addressService.CreateClientAddressesEditDtoAsync(id);
            return Ok(dto);
        }

        [HttpPatch]
        [Route("{id}/addresses/edit")]
        public async Task<IActionResult> EditAddresses([FromRoute] long id,
            [FromBody] ClientAddressesEditDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await addressService.UpdateClientAddressesAlternateAsync(id, dto);
            return Ok();
        }

        [HttpGet]
        [Route("{id}/emails")]
        public async Task<IActionResult> Emails([FromRoute] long id)
        {
            IEnumerable<EmailListDto> emails = await emailService.GetClientEmailsAsync(id);
            return Ok(emails);
        }

        [HttpGet]
        [Route("{id}/emails/edit")]
        public async Task<IActionResult> EditEmails([FromRoute] long id)
        {
            ClientEmailsEditDto dto = await emailService.CreateClientEmailsEditDtoAsync(id);
            return Ok(dto);
        }

        [HttpPatch]
        [Route("{id}/emails/edit")]
        public async Task<IActionResult> EditEmails([FromRoute] long id,
            [FromBody] ClientEmailsEditDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await emailService.UpdateClientEmailsAsync(id, dto);
            return Ok();
        }

        [HttpGet]
        [Route("{id}/phone-numbers")]
        public async Task<IActionResult> PhoneNumbers([FromRoute] long id)
        {
            IEnumerable<PhoneNumberListDto> phoneNumbers = await phoneNumberService.GetClientPhoneNumbersAsync(id);
            return Ok(phoneNumbers);
        }

        [HttpGet]
        [Route("{id}/phone-numbers/edit")]
        public async Task<IActionResult> EditPhoneNumbers([FromRoute] long id)
        {
            ClientPhoneNumbersEditDto dto = await phoneNumberService.CreateClientEditPhoneNumbersDtoAsync(id);
            return Ok(dto);
        }

        [HttpPatch]
        [Route("{id}/phone-numbers/edit")]
        public async Task<IActionResult> EditPhoneNumbers([FromRoute] long id, [FromBody] ClientPhoneNumbersEditDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await phoneNumberService.UpdateClientPhoneNumbersAsync(id, dto);
            return Ok();
        }

        [HttpGet]
        [Route("{id}/fines")]
        public async Task<IActionResult> Fines([FromRoute] long id)
        {
            IEnumerable<FineListDto> dto = await fineService.GetClientFinesAsync(id);
            return Ok(dto);
        }

        //[HttpDelete]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[Route("delete/{id}")]
        //public async Task<IActionResult> Delete([FromRoute] long id)
        //{
        //    await clientService.DeleteClientByIdAsync(id);
        //    return Ok();
        //}
    }
}
