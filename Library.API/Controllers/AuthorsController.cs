using Library.Core.Contracts.DbServices;
using Library.Core.Dto.Authors;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/authors")]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService authorService;

        public AuthorsController(IAuthorService authorService)
        {
            this.authorService = authorService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("all")]
        public async Task<IActionResult> All()
        {
            IEnumerable<AuthorListDto> authors = await authorService.GetAllAuthorsAsync();
            return Ok(authors);
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> List()
        {
            IEnumerable<AuthorSelectDto> authors = await authorService.GetAllAuthorsForSelectAsync();
            return Ok(authors);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("details/{id}")]
        public async Task<IActionResult> Details([FromRoute] long id)
        {
            AuthorDetailsDto? author = await authorService.GetAuthorByIdAsync(id);
            return Ok(author);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] AuthorCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await authorService.CreateAuthorAsync(dto);
            return Created();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(long id)
        {
            AuthorEditDto dto = await authorService.CreateAuthorEditDtoAsync(id);
            return Ok(dto);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] long id, [FromBody] AuthorEditDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await authorService.UpdateAuthorAsync(id, dto);

            return Created();
        }

        //[HttpDelete]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[Route("delete/{id}")]
        //public async Task<IActionResult> Delete([FromRoute] long id)
        //{
        //    await authorService.DeleteAuthorByIdAsync(id);
        //    return Ok();
        //}
    }
}
