using books_api.Helper;
using books_api.Modals;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace books_api.Handler
{
    public interface IBooksHandler
    {
        Task<HandleResult<BookList>> GetBookList();
    }
    public class BooksHandler : IBooksHandler
    {
        private readonly string _getUrl;
        private readonly HttpClient _httpClient;
        IConfiguration _configuration;

        public BooksHandler(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _getUrl = _configuration.GetSection("BooksGetUrl").Value;
        }

        public async Task<HandleResult<BookList>> GetBookList()
        {
            try
            {
                HttpMethod method = new HttpMethod("GET");
                var request = new HttpRequestMessage(method, _getUrl);
                var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var stringContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var results = JsonConvert.DeserializeObject<BookList>(stringContent);
                    return HandleResult<BookList>.Success(results);
                }
                else
                {
                    return HandleResult<BookList>.Failure("Internal Server Error");
                }
            }
            catch (Exception ex)
            {
                //Log ex
                return HandleResult<BookList>.Failure("Internal Server Error");
            }

        }
    }
}
