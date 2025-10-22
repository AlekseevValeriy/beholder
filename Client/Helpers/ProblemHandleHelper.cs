using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace Beholder.Helpers;
public class ProblemHandleHelper
{
    public static Problem ProblemHandle<T>(ApiResponse<T>? responseData)
    {
        if (responseData is not null)
        {
            if (responseData.HasProblem)
            {
                if (responseData.HttpError is not null)
                {
                    return responseData.HttpError switch
                    {
                        System.Net.HttpStatusCode.NotFound => Problem.NotFound,
                        System.Net.HttpStatusCode.BadGateway or System.Net.HttpStatusCode.ServiceUnavailable => Problem.NoConnect,
                        _ => Problem.InternalError
                    };
                }
                else if (responseData.Exception is not null)
                {
                    if  (responseData.Exception is AuthenticationException)
                    {
                        return Problem.NoAuth;
                    }
                }
            }
        }

        return Problem.ApiError; 
    }
}
