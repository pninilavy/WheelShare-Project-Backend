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

        // POST api/<UserController>
        //[HttpPost]
        //public Task<User> Post([FromBody] User item)
        //{
        //    return _userService.Add(item);

        //}
        [HttpPost("SignIn")]
        public async Task<IActionResult> Post([FromBody] UserLogin userLogin)
        {
            var user = await _userService.GetByIdNumberAndEmail(userLogin.IdNumber,userLogin.Email);
            if (user!=null)
            {
                var token = _userService.Generate(user);
                return Ok(token);
            }
            return BadRequest("user not found");

        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> Post([FromBody] User user)
        {
                   
                _userService.Add(user);
                var token = _userService.Generate(user);
                TokenAndName tokenAndName = new TokenAndName(token,user.FirstName,user.LastName);
                return Ok(tokenAndName);         
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
