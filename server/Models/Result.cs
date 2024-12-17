namespace server.Models
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public static Result<T> Success(T data, string message)
        {
            return new Result<T>
            {
                IsSuccess = true,
                Data = data,
                Message = message
            };
        }

        public static Result<T> Error(int statusCode,string ErrorMessage)
        {
            return new Result<T>
            {
                IsSuccess = false,
                ErrorCode = statusCode,
                ErrorMessage = ErrorMessage
            };
        }

   
    }

}
