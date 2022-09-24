using books_api.Controllers;
using books_api.Handler;
using books_api.Helper;
using books_api.Modals;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    public class BooksControllerTests
    {
        private Mock<IBooksHandler> _mockBookHandler;

        [SetUp]
        public void Init()
        {
            _mockBookHandler = new Mock<IBooksHandler>();

        }

        [Test]
        public async Task GetBooksData_Returns_Items_Succesfully()
        {
            var books = new BookList
            {
                Items = new List<Items>()
                    {
                        new Items()
                        {
                        VolumeInfo = new VolumeInfo()
                        {
                            Authors = new List<string>() { "Kaplan Test Prep" },
                            PublishedDate = "2020-03-03",
                            Publisher = " Simon and Schuster",
                            Title = "Official Guide to OET"
                        }
                    }
                }
            };

            _mockBookHandler.Setup(handler => handler.GetBookList())
                .Returns(() => Task.FromResult(HandleResult<BookList>.Success(books)));

            var controller = new BooksController(_mockBookHandler.Object);
            var response = await controller.Get();
            Assert.AreEqual((HttpStatusCode)response
                    .GetType()
                    .GetProperty("StatusCode")
                    .GetValue(response, null), HttpStatusCode.OK, "Should return an OK response.");
        }

        [Test]
        public async Task GetBooksData_Returns_BadRequest_OnFailure()
        {
            _mockBookHandler.Setup(handler => handler.GetBookList())
                .Returns(() => Task.FromResult(HandleResult<BookList>.Failure("Internal Server Error")));

            var controller = new BooksController(_mockBookHandler.Object);
            var response = await controller.Get();
            Assert.AreEqual((HttpStatusCode)response
                    .GetType()
                    .GetProperty("StatusCode")
                    .GetValue(response, null), HttpStatusCode.BadRequest, "Should return an OK response.");
        }
    }
}
