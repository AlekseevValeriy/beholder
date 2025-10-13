using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beholder.Helpers;
public class ProblemHandleHelper
{
    async public static Task<Problem> ProblemHandle<T>(ApiResponse<T>? responseData, ContentPage? page)
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
                        System.Net.HttpStatusCode.BadGateway => Problem.NoConnect,
                        _ => Problem.InternalError
                    };
                }
                else if (responseData.Exception is not null && page is not null)
                {
                    //await page.DisplayAlert("Проблема: исключение", responseData.Exception.Message, "ОК");
                }
            }
        }

        return Problem.ApiError; 
    }
}
