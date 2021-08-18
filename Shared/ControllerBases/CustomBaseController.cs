using Microsoft.AspNetCore.Mvc;
using UdemyMicroservices.Shared.Dtos;

namespace UdemyMicroservices.Shared.ControllerBases
{
    public class CustomBaseController : ControllerBase
    {
        public IActionResult CreateActionResultInstance<T>(Response<T> response)
        {
            return new ObjectResult(response) { StatusCode = response.StatusCode };
        }
    }
}
