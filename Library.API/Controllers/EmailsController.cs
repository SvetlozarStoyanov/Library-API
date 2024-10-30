using Library.Core.Common.Helpers;
using Library.Core.Contracts.DbServices;
using Library.Core.Contracts.HelperServices;
using Library.Core.Dto.Emails;
using Library.Infrastructure.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/emails")]
    public class EmailsController : ControllerBase
    {
        private readonly IEmailService emailService;
        private readonly IEnumService enumService;
        private readonly IClientService clientService;

        public EmailsController(IEmailService emailService,
            IEnumService enumService,
            IClientService clientService)
        {
            this.emailService = emailService;
            this.enumService = enumService;
            this.clientService = clientService;
        }

        [HttpGet]
        [Route("types")]
        public IActionResult GetEmailTypes()
        {
            IEnumerable<KeyValue> types = enumService.GetEnumValues<EmailType>();
            return Ok(types);
        }

        //[HttpGet]
        //[Route("create")]
        //public IActionResult Create()
        //{
        //    EmailCreateDto dto = emailService.CreateEmailCreateDto();
        //    return Ok(dto);
        //}

        [HttpPost]
        [Route("create/{clientId}")]
        public async Task<IActionResult> Create([FromRoute] long clientId, [FromBody] EmailCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await emailService.CreateEmailAsync(clientId, dto);
            return Created();
        }

        [HttpGet]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] long id)
        {
            EmailEditDto dto = await emailService.CreateEmailEditDtoAsync(id);
            return Ok(dto);
        }

        [HttpPut]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] long id, [FromBody] EmailEditDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await emailService.UpdateEmailAsync(id, dto);
            return Created();
        }

        [HttpPatch]
        [Route("change-main/clientId={clientId}&emailId={emailId}")]
        public async Task<IActionResult> ChangeMain([FromRoute] long clientId, [FromRoute] long emailId)
        {
            await emailService.ChangeClientMainEmailAsync(clientId, emailId);
            return NoContent();
        }

        [HttpPatch]
        [Route("archive/{id}")]
        public async Task<IActionResult> Archive([FromRoute] long id)
        {
            await emailService.ArchiveEmailAsync(id);
            return NoContent();
        }
    }
}
