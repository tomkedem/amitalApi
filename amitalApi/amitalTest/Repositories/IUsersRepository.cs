using amitalTest.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace amitalTest.Repositories
{
    public interface IUsersRepository
    {
        Task<IEnumerable<Users>> GetAllUsers();
    }
}