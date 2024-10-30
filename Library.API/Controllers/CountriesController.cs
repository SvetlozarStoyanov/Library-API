using Library.Core.Contracts.DbServices;
using Library.Core.Dto.Countries;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/countries")]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService countryService;

        public CountriesController(ICountryService countryService)
        {
            this.countryService = countryService;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> All()
        {
            IEnumerable<CountryListDto> countries = await countryService.GetAllCountriesAsync();
            return Ok(countries);
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> List()
        {
            IEnumerable<CountrySelectDto> countries = await countryService.GetAllCountriesForSelectAsync();
            return Ok(countries);
        }
    }
}
