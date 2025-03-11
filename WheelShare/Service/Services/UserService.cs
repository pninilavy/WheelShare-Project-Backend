using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;

namespace Service.Services
{
    public class UserService : IUserService<User>
    {
        private readonly IRepository<User> _repository;
        private readonly IConfiguration config;
        public UserService(IRepository<User> _repository, IConfiguration config)
        {
            this._repository = _repository;   
            this.config = config;
        }
        public Task<User> Add(User item)
        {
            return _repository.Add(item);
        }

        public Task Delete(int id)
        {
            return _repository.Delete(id);
        }

        public Task<List<User>> GetAll()
        {
            return _repository.GetAll();
        }

        public Task<User> GetById(int id)
        {
            return _repository.GetById(id);
        }

        public Task<User> Update(int id, User item)
        {
            return _repository.Update(id,item);
        }

        public async Task<User> GetByIdNumberAndEmail(string idNumber, string Email)
        {
            var users = await _repository.GetAll(); 
            return users.FirstOrDefault(u => u.IdNumber == idNumber && u.Email == Email);
            
        }
        public string Generate(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                
                new Claim(ClaimTypes.NameIdentifier,user.FirstName+" "+user.LastName),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Gender,user.Gender),
                new Claim("IdNumber", user.IdNumber),
                new Claim(ClaimTypes.MobilePhone,user.PhoneNumber),

            };
            var token = new JwtSecurityToken(config["Jwt:Issuer"], config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        
        
    }
}
