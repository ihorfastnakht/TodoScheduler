using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoScheduler.Models;

namespace TodoScheduler.Infrastructure.Services.DataServices
{
    public interface IDataService
    {
        Task CreateTagItemAsync(TagItem tagItem);
        Task RemoveTagItemAsync(TagItem tagItem);
        Task UpdateTagItemAsync(TagItem todoItem);
        Task<IEnumerable<TagItem>> GetTagItemsAsync();

        Task CreateTodoItemAsync(TodoItem todoItem);
        Task RemoveTodoItemAsync(TodoItem todoItem);
        Task UpdateTodoItemAsync(TodoItem todoItem);

        Task<IEnumerable<TodoItem>> GetTodoItemsAsync();
        Task<IEnumerable<TodoItem>> GetTodoItemsAsync(DateTime date);
        Task<IEnumerable<TodoItem>> GetTodoItemsAsync(TagItem tagItem);
    }
}
