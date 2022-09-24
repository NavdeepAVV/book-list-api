using System.Threading.Tasks;
using books_api.Handler;
using Microsoft.AspNetCore.Mvc;
using books_api.Helper;
using books_api.Modals;

namespace books_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBooksHandler _bookHandler;

        public BooksController(IBooksHandler bookHandler)
        {
            _bookHandler = bookHandler;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _bookHandler.GetBookList().ConfigureAwait(false);

            switch (response.Status)
            {
                case HandleResult<BookList>.RequestStatus.Success:
                    return Ok(response.Data);
                case HandleResult<BookList>.RequestStatus.Failure:
                    return BadRequest(response.ErrorMessage);
            }
            return BadRequest(response.ErrorMessage);
        }
    }
}
