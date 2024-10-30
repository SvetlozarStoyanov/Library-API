using Library.Core.Contracts.DbServices;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/genre-books")]
    public class GenreBooksController : ControllerBase
    {
        private readonly IGenreBookService genreBookService;
        private readonly IGenreService genreService;
        private readonly IBookService bookService;

        public GenreBooksController(IGenreBookService genreBookService,
            IGenreService genreService,
            IBookService bookService)
        {
            this.genreBookService = genreBookService;
            this.genreService = genreService;
            this.bookService = bookService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("add-book-to-genre/genreId={genreId}&bookId={bookId}")]
        public async Task<IActionResult> LinkGenreAndBook([FromRoute] long genreId, [FromRoute] long bookId)
        {
            await genreBookService.LinkGenreAndBookAsync(genreId, bookId);
            return Ok();
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("remove-book-from-genre/genreId={genreId}&bookId={bookId}")]
        public async Task<IActionResult> UnlinkGenreAndBook([FromRoute] long genreId, [FromRoute] long bookId)
        {
            await genreBookService.DeleteGenreBookAsync(genreId, bookId);
            return Ok();
        }
    }
}
