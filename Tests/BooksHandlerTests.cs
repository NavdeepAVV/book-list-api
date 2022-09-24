using books_api.Handler;
using books_api.Modals;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    public class BooksHandlerTests
    {
        [Test]
        public async Task BooksHandler_Gets_Records_Succesfully()
        {
            string httpclientContent = "{\"items\":[{\"volumeInfo\":{\"title\":\"Official Guide to OET\",\"authors\":[\"Kaplan Test Prep\"],\"publisher\":\"Simon and Schuster\",\"publishedDate\":\"2020-03-03\"}},{\"volumeInfo\":{\"title\":\"SAT Prep Plus 2021\",\"authors\":[\"Kaplan Test Prep\"],\"publisher\":\"Simon and Schuster\",\"publishedDate\":\"2020-06-02\"}},{\"volumeInfo\":{\"title\":\"Kaplan DAT\",\"authors\":[\"Kaplan, Inc\"],\"publisher\":\"Kaplan\",\"publishedDate\":\"2002\"}},{\"volumeInfo\":{\"title\":\"GMAT Prep Plus 2021\",\"authors\":[\"Kaplan Test Prep\"],\"publisher\":\"Simon and Schuster\",\"publishedDate\":\"2020-07-07\"}},{\"volumeInfo\":{\"title\":\"NCLEX-PN Prep Plus 2018\",\"authors\":[\"Kaplan Nursing\"],\"publisher\":\"Simon and Schuster\",\"publishedDate\":\"2018-02-06\"}},{\"volumeInfo\":{\"title\":\"GRE Prep Plus 2021\",\"authors\":[\"Kaplan Test Prep\"],\"publisher\":\"Simon and Schuster\",\"publishedDate\":\"2020-06-02\"}},{\"volumeInfo\":{\"title\":\"Kaplan LSAT 2002-2003\",\"authors\":[\"Kaplan\"],\"publisher\":null,\"publishedDate\":\"2002-06\"}},{\"volumeInfo\":{\"title\":\"SAT Prep Plus 2020\",\"authors\":[\"Kaplan Test Prep\"],\"publisher\":\"Simon and Schuster\",\"publishedDate\":\"2019-07-02\"}},{\"volumeInfo\":{\"title\":\"GRE Prep Plus 2022 Our 80 Year's Expertise = Your Competitive Advantage\",\"authors\":[\"Kaplan Test Prep\"],\"publisher\":\"Simon and Schuster\",\"publishedDate\":\"2021-05-04\"}},{\"volumeInfo\":{\"title\":\"MCAT Complete 7-Book Subject Review 2021-2022\",\"authors\":[\"Kaplan Test Prep\"],\"publisher\":\"Kaplan Publishing\",\"publishedDate\":\"2020-07-07\"}}]}";
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent(httpclientContent),
               })
               .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = It.IsAny<Uri>()
            };

            var inMemorySettings = new Dictionary<string, string> {
                {"BooksGetUrl", "https://www.googleapis.com/books/v1/volumes?q=kaplan%20test%20prep"}
            };


            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var handler = new BooksHandler(httpClient, configuration);
            var response = await handler.GetBookList().ConfigureAwait(false);
            Assert.AreEqual("Official Guide to OET", response.Data.Items[0].VolumeInfo.Title);

        }

        [Test]
        public async Task BooksHandler_Fails_When_HttpClient_ErrorsOut()
        {
            string httpclientContent = "{\"items\":[{\"volumeInfo\":{\"title\":\"Official Guide to OET\",\"authors\":[\"Kaplan Test Prep\"],\"publisher\":\"Simon and Schuster\",\"publishedDate\":\"2020-03-03\"}},{\"volumeInfo\":{\"title\":\"SAT Prep Plus 2021\",\"authors\":[\"Kaplan Test Prep\"],\"publisher\":\"Simon and Schuster\",\"publishedDate\":\"2020-06-02\"}},{\"volumeInfo\":{\"title\":\"Kaplan DAT\",\"authors\":[\"Kaplan, Inc\"],\"publisher\":\"Kaplan\",\"publishedDate\":\"2002\"}},{\"volumeInfo\":{\"title\":\"GMAT Prep Plus 2021\",\"authors\":[\"Kaplan Test Prep\"],\"publisher\":\"Simon and Schuster\",\"publishedDate\":\"2020-07-07\"}},{\"volumeInfo\":{\"title\":\"NCLEX-PN Prep Plus 2018\",\"authors\":[\"Kaplan Nursing\"],\"publisher\":\"Simon and Schuster\",\"publishedDate\":\"2018-02-06\"}},{\"volumeInfo\":{\"title\":\"GRE Prep Plus 2021\",\"authors\":[\"Kaplan Test Prep\"],\"publisher\":\"Simon and Schuster\",\"publishedDate\":\"2020-06-02\"}},{\"volumeInfo\":{\"title\":\"Kaplan LSAT 2002-2003\",\"authors\":[\"Kaplan\"],\"publisher\":null,\"publishedDate\":\"2002-06\"}},{\"volumeInfo\":{\"title\":\"SAT Prep Plus 2020\",\"authors\":[\"Kaplan Test Prep\"],\"publisher\":\"Simon and Schuster\",\"publishedDate\":\"2019-07-02\"}},{\"volumeInfo\":{\"title\":\"GRE Prep Plus 2022 Our 80 Year's Expertise = Your Competitive Advantage\",\"authors\":[\"Kaplan Test Prep\"],\"publisher\":\"Simon and Schuster\",\"publishedDate\":\"2021-05-04\"}},{\"volumeInfo\":{\"title\":\"MCAT Complete 7-Book Subject Review 2021-2022\",\"authors\":[\"Kaplan Test Prep\"],\"publisher\":\"Kaplan Publishing\",\"publishedDate\":\"2020-07-07\"}}]}";
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.BadRequest,
                   Content = new StringContent(httpclientContent),
               })
               .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = It.IsAny<Uri>()
            };

            var inMemorySettings = new Dictionary<string, string> {
                {"BooksGetUrl", "https://www.googleapis.com/books/v1/volumes?q=kaplan%20test%20prep"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var handler = new BooksHandler(httpClient, configuration);
            var response = await handler.GetBookList().ConfigureAwait(false);
            Assert.AreEqual(null, response.Data);
            Assert.AreEqual("Internal Server Error", response.ErrorMessage);

        }
    }
}
