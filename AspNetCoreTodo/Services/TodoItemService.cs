using System.Threading.Tasks;
using AspNetCoreTodo.Data;
using AspNetCoreTodo.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

namespace AspNetCoreTodo.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ApplicationDbContext _dbcontext;

        public TodoItemService(ApplicationDbContext dbContext)
        {
            _dbcontext = dbContext;
        }

        public async Task<bool> AddItemAsync(TodoItem newTodoItem)
        {
            newTodoItem.Id = Guid.NewGuid();
            newTodoItem.IsDone = false;
            //newTodoItem.DueAt = DateTimeOffset.Now.AddDays(3);

            _dbcontext.Items.Add(newTodoItem);

            var saveResult = await _dbcontext.SaveChangesAsync();

            return saveResult == 1;
        }

        public async Task<TodoItem[]> GetIncompleteItemsAsync()
        {
            return await _dbcontext.Items.Where(x => x.IsDone == false).ToArrayAsync();
        }

        public async Task<bool> MarkDoneAsync(Guid id)
        {
            var item = await _dbcontext.Items.Where(i => i.Id == id).FirstOrDefaultAsync();

            if (item == null) return false;

            item.IsDone = true;

            var saveResult = await _dbcontext.SaveChangesAsync();

            return saveResult == 1;
        }
    }
}