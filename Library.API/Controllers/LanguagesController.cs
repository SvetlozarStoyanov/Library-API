using Library.Core.Contracts.DbServices;
using Library.Core.Dto.Languages;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/languages")]
    public class LanguagesController : ControllerBase
    {
        private readonly ILanguageService languageService;

        public LanguagesController(ILanguageService languageService)
        {
            this.languageService = languageService;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> All()
        {
            IEnumerable<LanguageListDto> countries = await languageService.GetAllLanguagesAsync();
            return Ok(countries);
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> List()
        {
            IEnumerable<LanguageSelectDto> countries = await languageService.GetAllLanguagesForSelectAsync();
            return Ok(countries);
        }

        //[HttpGet]
        //[Route("create")]
        //public IActionResult Create()
        //{
        //    LanguageCreateDto dto = languageService.CreateLanguageCreateDto();
        //    return Ok(dto);
        //}

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] LanguageCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await languageService.CreateLanguageAsync(dto);
            return Ok();
        }

        [HttpGet]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] long id)
        {
            LanguageEditDto dto = await languageService.CreateLanguageEditDtoAsync(id);
            return Ok(dto);
        }

        [HttpPatch]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] long id, [FromBody] LanguageEditDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await languageService.UpdateLanguageAsync(id, dto);
            return Ok();
        }
    }
}
