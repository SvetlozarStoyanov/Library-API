using Library.Core.Contracts.DbServices;
using Library.Core.Dto.Series;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/series")]
    public class SeriesController : ControllerBase
    {
        private readonly ISeriesService seriesService;

        public SeriesController(ISeriesService seriesService)
        {
            this.seriesService = seriesService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("all")]
        public async Task<IActionResult> All()
        {
            IEnumerable<SeriesListDto> series = await seriesService.GetAllSeriesAsync();
            return Ok(series);
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> List()
        {
            IEnumerable<SeriesSelectDto> series = await seriesService.GetAllSeriesForSelectAsync();
            return Ok(series);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("details/{id}")]
        public async Task<IActionResult> Details([FromRoute] long id)
        {
            SeriesDetailsDto dto = await seriesService.GetSeriesByIdAsync(id);
            return Ok(dto);
        }

        //[HttpGet]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[Route("create")]
        //public async Task<IActionResult> Create()
        //{
        //    SeriesCreateDto dto = seriesService.CreateSeriesCreateDtoAsync();
        //    return Ok(dto);
        //}

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] SeriesCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await seriesService.CreateSeriesAsync(dto);
            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] long id)
        {
            SeriesEditDto dto = await seriesService.CreateSeriesEditDtoAsync(id);
            return Ok(dto);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] long id, [FromBody] SeriesEditDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await seriesService.UpdateSeriesAsync(id, dto);
            return Ok();
        }

        //[HttpDelete]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[Route("delete/{id}")]
        //public async Task<IActionResult> Delete([FromRoute] long id)
        //{
        //    if (!(await seriesService.SeriesExistsAsync(id)))
        //    {
        //        return NotFound("Series was not found!");
        //    }

        //    await seriesService.DeleteSeriesByIdAsync(id);
        //    return Ok();
        //}
    }
}
