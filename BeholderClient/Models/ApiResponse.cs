using System.Net;

namespace Beholder.Models
{
    public class ApiResponse<T> : IProblemable
    {
        T? _content;
        HttpStatusCode? _error; 
        Exception? _exception;

        public T? Content => _content;
        public Boolean IsSuccess => _error is null && _exception is null;
        public Boolean HasProblem => _error is not null || _exception is not null;
        public HttpStatusCode? HttpError => _error;
        public Exception? Exception => _exception;

        public ApiResponse(T content)
        {
            _content = content;
        }
        public ApiResponse(HttpStatusCode error)
        {
            _error = error;
        }
        public ApiResponse(Exception exception)
        {
            _exception = exception;
        }
        public ApiResponse(IProblemable problem)
        {
            _exception = problem.Exception is null ? _exception : problem.Exception;
            _error = problem.HttpError is null ? _error : problem.HttpError;
        }

        public void ProblemProcessed()
        {
            _error = null;
            _exception = null;
        }
    }

    public static class ServerResponseExtensions
    {
        public static Boolean IsEmpty<T>(this ApiResponse<T> response)
        {
            return response.Content is null && response.Exception is null && response.HttpError is null;
        }

        public static T GetContentOrThrow<T>(this ApiResponse<T> response)
        {
            if (response.HasProblem)
            {
                if (response.Exception is null) throw new InvalidOperationException($"Server response has error: {response.HttpError}");
                else throw new InvalidOperationException($"Api client handler has exception: {response.Exception}");
            }

            if (response.Content is null)
                throw new InvalidOperationException("Server response content is null");

            return response.Content;
        }

        public static T GetContentOrDefault<T>(this ApiResponse<T> response, T defaultValue = default!)
        {
            return response.IsSuccess && response.Content is not null ? response.Content : defaultValue;
        }

        public static Boolean TryGetContent<T>(this ApiResponse<T> response, out T? content)
        {
            content = response.IsSuccess ? response.Content : default;
            return response.IsSuccess && response.Content is not null;
        }
    }
}
