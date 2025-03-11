using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IUserService<User>:IService<User>
    {
        Task<User> GetByIdNumberAndEmail(string idNumber,string Email);
        string Generate(User user);
    }
}
