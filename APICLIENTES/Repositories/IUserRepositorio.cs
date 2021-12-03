using APICLIENTES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICLIENTES.Repositories
{
    public interface IUserRepositorio
    {
        Task<string> Register(User user,string password);
        Task<string> Login(string userName,string password);
        Task<bool> IsRegister(string userName);
    }
}
