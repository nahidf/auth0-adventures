using Microsoft.AspNetCore.Authentication;
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

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<UserModel>> Get()
        {
            return new List<UserModel>
            {
                new UserModel()
                {
                    Name = "Nahid"
                },
                new UserModel()
                {
                    Name = "Dora"
                },
                new UserModel()
                {
                    Name = "Foo"
                }
            };
        }

        [HttpGet]
        [Route("verify")]
        public string Verify()
        {
            return "API is up!";
        }
    }
}
