using amitalTest.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace amitalTest.Repositories
{
    public interface ITodoRepository
    {
        Task<IEnumerable<ItemTodo>> AddTask(ItemTodo request);
        Task<int> GetSumOfTaskByUserId(int userId);
        Task<IEnumerable<ItemTodo>> GetTaskList();
    }
}