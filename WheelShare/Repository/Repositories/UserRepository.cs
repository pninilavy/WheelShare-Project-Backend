using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Repository.Interfaces;

namespace Repository.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly IContext context;
        
        public UserRepository(IContext context)
        {
            this.context = context;
        }
        public async Task<User> Add(User item)
        {
            await context.Users.AddAsync(item);
            await context.Save();
            return item;
        }

        public async Task Delete(int id)
        {
            context.Users.Remove(await GetById(id));
            await context.Save();
        }

        public async Task<List<User>> GetAll()
        {
            return await context.Users.ToListAsync();

        }

        public async Task<User> GetById(int id)
        {
            return await context.Users.FirstOrDefaultAsync(user => user.Id == id);
           
        }

        public async Task<User> Update(int id, User item)
        {
            User user=await GetById(id);
            user.FirstName=item.FirstName;
            user.LastName=item.LastName;
            user.Email=item.Email;
            user.PhoneNumber=item.PhoneNumber;
            user.IdNumber=item.IdNumber;
            user.Gender=item.Gender;
            await context.Save();
            return user;
        }
        

      
    }
}
