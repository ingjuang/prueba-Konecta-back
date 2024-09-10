using Business.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utils.Responses;

namespace WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet, Route("GetUser")]
        public async Task<ActionResult> GetUser(int id)
        {
            PetitionResponse res = await _userService.GetUser(id);
            if (res.success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res);
            }
        }

        [HttpGet, Route("GetUsers")]
        public async Task<ActionResult> GetUsers()
        {
            PetitionResponse res = await _userService.GetUsers();
            if (res.success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser([FromBody] User user)
        {
            PetitionResponse res = await _userService.CreateUser(user);
            if (res.success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res);
            }
        }
        [HttpPut]
        public async Task<ActionResult> UpdateUser([FromBody] User user)
        {
            PetitionResponse res = await _userService.UpdateUser(user);
            if (res.success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res);
            }
        }

    }
}
