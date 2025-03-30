using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Mock;
using Repository.Entities;
using Service.Interfaces;
using WheelShare.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WheelShare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService<User> _userService;
        public UserController(IUserService<User> _userService)
        {
            this._userService = _userService;

        }
        // GET: api/<UserController>
        [HttpGet]
        public Task<List<User>> Get()
        {
            return _userService.GetAll();

        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public Task<User> Get(int id)
        {
            return _userService.GetById(id);

        }


        [HttpPost("SignIn")]
        public async Task<IActionResult> Post([FromBody] UserLogin userLogin)
        {
            
            var user = await _userService.GetByIdNumberAndEmail(userLogin.IdNumber, userLogin.Email);
            if (user != null)
            {
                var token = _userService.Generate(user);
                TokenAndName tokenAndName = new(token, user.FirstName, user.LastName);
                return Ok(tokenAndName);
            }
            return BadRequest("משתמש לא רשום");

        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            User user1 = await _userService.GetByIdNumberAndEmail(user.IdNumber, user.Email);
            if (user1 != null)
            {
                return BadRequest(new { message = "משתמש כבר קיים במערכת" });
            }
            var newUser = await _userService.Add(user);
            return Ok(new { message = "ההרשמה בוצעה בהצלחה", user = newUser });
        }


        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public Task<User> Put(int id, [FromBody] User item)
        {
            return _userService.Update(id, item);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public Task Delete(int id)
        {
            return _userService.Delete(id);

        }



    }
}
