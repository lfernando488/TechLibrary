using Microsoft.AspNetCore.Mvc;
using TechLibrary.Api.UseCases.Users.Register;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;

namespace TechLibrary.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
        public IActionResult Create(RequestUserJson requestUserJson)
        {
            var useCase = new RegisterUserUseCase();
            var response = useCase.Execute(requestUserJson);

            return Created(String.Empty, response);
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Created();
        }

    }
}
