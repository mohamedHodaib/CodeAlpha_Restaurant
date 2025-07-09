using Microsoft.AspNetCore.Mvc;
using Restaurant.Business;

namespace Restaurant.Api
{
    public static class ControllerExtensions
    {
        public static IActionResult HandleFailureResult<T>(this ControllerBase controller,
            BusinessResult<T> result)
        {

            switch (result.Error)
            {
                case BusinessResult<T>.enError.BadRequest:
                    return controller.BadRequest(result.Message);

                case BusinessResult<T>.enError.Conflict:
                    return controller.Conflict(result.Message);

                case BusinessResult<T>.enError.NotFound:
                    return controller.NotFound(result.Message);

                case BusinessResult<T>.enError.Unauthorized:
                    return controller.Unauthorized(result.Message);

                default:
                    return controller.StatusCode(StatusCodes.Status500InternalServerError,
                        result.Message);

            }
        }
    }
}
