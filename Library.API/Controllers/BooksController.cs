using Library.Core.Contracts.DbServices;
using Library.Core.Dto.BookAcquisitions;
using Library.Core.Dto.Books;
using Library.Core.Dto.Checkouts;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService bookService;
        private readonly IBookAcquisitionService bookAcquisitionService;
        private readonly ICheckoutService checkoutService;

        public BooksController(IBookService bookService,
            IBookAcquisitionService bookAcquisitionService,
            ICheckoutService checkoutService)
        {
            this.bookService = bookService;
            this.bookAcquisitionService = bookAcquisitionService;
            this.checkoutService = checkoutService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("filter")]
        public async Task<IActionResult> Filter([FromQuery] BooksFilterDto dto)
        {
            IEnumerable<BookListDto> books = await bookService.GetFilteredBooksAsync(dto);

            return Ok(books);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("all")]
        public async Task<IActionResult> All()
        {
            IEnumerable<BookListDto> books = await bookService.GetAllBooksAsync();
            return Ok(books);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("{id}/checkouts")]
        public async Task<IActionResult> Checkouts([FromRoute] long id,
            [FromQuery] int page = 1,
            [FromQuery] int itemsPerPage = 6)
        {
            IEnumerable<CheckoutListDto> dto = await checkoutService.GetBookCheckoutsAsync(id, page, itemsPerPage);
            return Ok(dto);
        }

        [HttpGet]
        [Route("{id}/acquisitions")]
        public async Task<IActionResult> Acquisitions([FromRoute] long id)
        {
            IEnumerable<BookAcquisitionListDto> bookAcquisitions = await bookAcquisitionService.GetBookAcquisitionsAsync(id);
            return Ok(bookAcquisitions);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("details/{id}")]
        public async Task<IActionResult> Details([FromRoute] long id)
        {
            BookDetailsDto book = await bookService.GetBookByIdAsync(id);
            return Ok(book);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] BookCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await bookService.CreateBookAsync(dto);
            return Created();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("create-experimental")]
        public IActionResult CreateExperimental()
        {
            BookExperimentalCreateDto dto = bookService.CreateBookExperimentalCreateDto();
            return Ok(dto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("create-experimental")]
        public async Task<IActionResult> CreateExperimental([FromBody] BookExperimentalCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await bookService.ExperimentalCreateBookAsync(dto);
            return Created();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] long id)
        {
            if (!(await bookService.BookExistsAsync(id)))
            {
                return NotFound("Book does not exist!");
            }
            BookEditDto dto = await bookService.CreateBookEditDtoAsync(id);
            return Ok(dto);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] long id, [FromBody] BookEditDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await bookService.UpdateBookAsync(id, dto);
            return Ok();
        }

        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("archive/{id}")]
        public async Task<IActionResult> Archive([FromRoute] long id)
        {
            await bookService.ArchiveBookByIdAsync(id);
            return Ok();
        }
    }
}
