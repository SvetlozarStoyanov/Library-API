using Library.Core.Contracts.DbServices;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/series-books")]
    public class SeriesBooksController : ControllerBase
    {
        private readonly ISeriesBookService seriesBookService;
        private readonly ISeriesService seriesService;
        private readonly IBookService bookService;

        public SeriesBooksController(ISeriesBookService seriesBookService,
            ISeriesService seriesService,
            IBookService bookService)
        {
            this.seriesBookService = seriesBookService;
            this.seriesService = seriesService;
            this.bookService = bookService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("add-book-to-series/seriesId={seriesId}&bookId={bookId}")]
        public async Task<IActionResult> LinkSeriesAndBook([FromRoute] long seriesId, [FromRoute] long bookId)
        {
            await seriesBookService.LinkSeriesAndBookAsync(seriesId, bookId);

            return Ok();
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("remove-book-from-series/seriesId={seriesId}&bookId={bookId}")]
        public async Task<IActionResult> UnlinkSeriesAndBook([FromRoute] long seriesId, [FromRoute] long bookId)
        {
            await seriesBookService.DeleteSeriesBookAsync(seriesId, bookId);

            return Ok();
        }
    }
}
