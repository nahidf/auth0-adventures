using Api.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("users")]
  
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpGet]
        [Authorize]
        public Task<IEnumerable<UserModel>> GetAsync([FromQuery] Dictionary<string, string> filter)
        {
            return _userService.GetUsersAsync(filter);
        }

        //[HttpGet]
        //[Route("verify")]
        //public string Verify()
        //{
        //    return "API is up!";
        //}

        [HttpGet]
        [Route("verify")]
        public Task<IEnumerable<UserModel>> TestApi([FromQuery] Dictionary<string, string> filter)
        {
            return _userService.GetUsersAsync(filter);
        }
    }
}
