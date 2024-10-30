using Library.Core.Contracts.DbServices;
using Library.Core.Dto.Genres;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/genres")]
    public class GenresController : ControllerBase
    {
        private readonly IGenreService genreService;

        public GenresController(IGenreService genreService)
        {
            this.genreService = genreService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("all")]
        public async Task<IActionResult> All()
        {
            IEnumerable<GenreListDto> genres = await genreService.GetAllGenresAsync();
            return Ok(genres);
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> List()
        {
            IEnumerable<GenreSelectDto> genres = await genreService.GetAllGenresForSelectAsync();
            return Ok(genres);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("details/{id}")]
        public async Task<IActionResult> Details(long id)
        {
            GenreDetailsDto dto = await genreService.GetGenreByIdAsync(id);
            return Ok(dto);
        }

        //[HttpGet]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[Route("create")]
        //public async Task<IActionResult> Create()
        //{
        //    GenreCreateDto dto = genreService.CreateGenreCreateDtoAsync();
        //    return Ok(dto);
        //}

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] GenreCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await genreService.CreateGenreAsync(dto);
            return Created();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] long id)
        {
            GenreEditDto dto = await genreService.CreateGenreEditDtoAsync(id);
            return Ok(dto);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] long id, [FromBody] GenreEditDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await genreService.UpdateGenreAsync(id, dto);
            return Created();
        }

        //[HttpDelete]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[Route("delete/{id}")]
        //public async Task<IActionResult> Delete([FromRoute] long id)
        //{
        //    await genreService.DeleteGenreByIdAsync(id);
        //    return Ok();
        //}
    }
}
