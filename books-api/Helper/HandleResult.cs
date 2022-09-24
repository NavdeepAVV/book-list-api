using Newtonsoft.Json;

namespace books_api.Helper
{
    public class HandleResult<T>
    {
        public enum RequestStatus
        {
            Success,
            Failure
        }

        [JsonIgnore]
        public RequestStatus Status { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }

        [JsonProperty("message")]
        public string ErrorMessage { get; set; }

        public static HandleResult<T> Success(T item)
        {
            return new HandleResult<T>
            {
                Status = RequestStatus.Success,
                Data = item
            };
        }

        public static HandleResult<T> Failure(string errorMessage)
        {
            return new HandleResult<T>
            {
                Status = RequestStatus.Failure,
                ErrorMessage = errorMessage
            };
        }
    }
}
