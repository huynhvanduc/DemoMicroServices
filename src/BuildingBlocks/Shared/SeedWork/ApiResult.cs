namespace Shared.SeedWork;

public class ApiResult<T>
{
    public ApiResult()
    {
    }

    public ApiResult(bool isSuccessed, string message = null)
    {
        Message = message;
        IsSuccessed = isSuccessed;
    }

    public ApiResult(bool isSuccessed, T data, string message = null)
    {
        Message = message;
        IsSuccessed = isSuccessed;
        Data = data;
    }

    public bool IsSuccessed { get; init; }

    public string Message { get; init; }
    public T Data { get; }
}
