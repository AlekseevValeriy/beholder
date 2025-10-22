using System.Net;

namespace Beholder.Models;
public class ApiResponseProblem : IProblemable
{
    readonly HttpStatusCode? _error;
    readonly Exception? _exception;

    public Boolean IsSuccess => _error is null && _exception is null;
    public Boolean HasProblem => _error is not null || _exception is not null;
    public HttpStatusCode? HttpError => _error;
    public Exception? Exception => _exception;

    public ApiResponseProblem(HttpStatusCode error)
    {
        _error = error;
    }

    public ApiResponseProblem(Exception exception)
    {
        _exception = exception;
    }    
    
    public ApiResponseProblem(IProblemable problem)
    {
        _exception = problem.Exception is null ? _exception : problem.Exception;
        _error = problem.HttpError is null ? _error : problem.HttpError;
    }
}
