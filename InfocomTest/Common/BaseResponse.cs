namespace InfocomTest.Common;

public class BaseResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }

    public BaseResponse(bool success, string message, T data = default)
    {
        Success = success;
        Message = message;
        Data = data;
    }
}