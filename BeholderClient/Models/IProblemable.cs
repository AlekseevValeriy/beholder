using System;
using System.Net;

namespace Beholder.Models;

public interface IProblemable
{
    public HttpStatusCode? HttpError { get; }
    public Exception? Exception { get; }
}
