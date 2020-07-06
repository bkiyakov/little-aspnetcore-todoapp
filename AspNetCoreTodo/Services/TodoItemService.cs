using System.Threading.Tasks;
using AspNetCoreTodo.Data;
using AspNetCoreTodo.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreTodo.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ApplicationDbContext _dbcontext;

        public TodoItemService(ApplicationDbContext dbContext)
        {
            _dbcontext = dbContext;
        }
        public async Task<TodoItem[]> GetIncompleteItemsAsync()
        {
            return await _dbcontext.Items.Where(x => x.IsDone == false).ToArrayAsync();
        }
    }
}