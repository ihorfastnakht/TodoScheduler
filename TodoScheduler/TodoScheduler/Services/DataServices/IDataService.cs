using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoScheduler.Models;

namespace TodoScheduler.Services.DataServices
{
    public interface IDataService
    {
        Task CreateTagItemAsync(TagItem tagItem);
        Task RemoveTagItemAsync(TagItem tagItem);
        Task<IEnumerable<TagItem>> GetTagItemsAsync();

        Task CreateTodoItemAsync(TodoItem todoItem);
        Task UpdateTodoItemAsync(TodoItem todoItem);
        Task RemoveTodoItemAsync(TodoItem todoItem);
        Task<IEnumerable<TodoItem>> GetTodoItemsAsync();
        Task<IEnumerable<TodoItem>> GetTodoItemsAsync(DateTime dueDate);
        Task<IEnumerable<TodoItem>> GetTodoItemsAsync(TagItem tagItem);
    }
}
