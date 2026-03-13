namespace ThreadedComments.WebApi.Contracts.Errors;


public sealed class ErrorResponse
{
    public int Status { get; init; }
    public string Error { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public string? TraceId { get; init; } 
}