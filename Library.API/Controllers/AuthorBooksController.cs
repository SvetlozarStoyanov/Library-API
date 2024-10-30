using Library.Core.Contracts.DbServices;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/author-books")]
    public class AuthorBooksController : ControllerBase
    {
        private readonly IAuthorBookService authorBookService;
        private readonly IAuthorService authorService;
        private readonly IBookService bookService;

        public AuthorBooksController(IAuthorBookService authorBookService,
            IAuthorService authorService,
            IBookService bookService)
        {
            this.authorBookService = authorBookService;
            this.authorService = authorService;
            this.bookService = bookService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("add-book-to-author/authorId={authorId}&bookId={bookId}")]
        public async Task<IActionResult> LinkAuthorAndBook([FromRoute] long authorId, [FromRoute] long bookId)
        {
            await authorBookService.LinkAuthorAndBookAsync(authorId, bookId);
            return Ok();
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("remove-book-from-author/authorId={authorId}&bookId={bookId}")]
        public async Task<IActionResult> UnlinkAuthorAndBook([FromRoute] long authorId, [FromRoute] long bookId)
        {
            await authorBookService.DeleteAuthorBookAsync(authorId, bookId);
            return Ok();
        }
    }
}
