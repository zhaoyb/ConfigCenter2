namespace ConfigCenter.Admin
{
    public class ResponseResult
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public ResponseResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}