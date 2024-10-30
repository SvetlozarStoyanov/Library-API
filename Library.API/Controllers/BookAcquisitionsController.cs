using Library.Core.Contracts.DbServices;
using Library.Core.Dto.BookAcquisitions;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/book-acquisitions")]
    public class BookAcquisitionsController : ControllerBase
    {
        private readonly IBookService bookService;
        private readonly IBookAcquisitionService bookAcquisitionService;

        public BookAcquisitionsController(IBookService bookService,
            IBookAcquisitionService bookDeliveryService)
        {
            this.bookService = bookService;
            this.bookAcquisitionService = bookDeliveryService;
        }

        [HttpPost]
        [Route("create/{bookId}")]
        public async Task<IActionResult> Create([FromRoute] long bookId, BookAcquisitionRestockDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await bookAcquisitionService.RestockBookAsync(bookId, dto);
            return Ok();
        }
    }
}
