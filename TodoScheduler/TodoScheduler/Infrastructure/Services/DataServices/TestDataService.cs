using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoScheduler.Models;

namespace TodoScheduler.Infrastructure.Services.DataServices
{
    public class TestDataService : IDataService
    {
        #region members

        private IList<TagItem> tags;
        private IList<TodoItem> todoItems;

        #endregion


        #region constructor

        public TestDataService()
        {
            Init();
        }

        #endregion

        #region private

        private void Init()
        {
            tags = new List<TagItem>() {
                new TagItem() {
                    Id = 0, Title = "Default", TagColor = "#7635EB"
                },
                new TagItem() {
                    Id = 1, Title = "Sports", TagColor = "#2d2d30"
                },
                new TagItem() {
                    Id = 2, Title = "Works", TagColor = "#8000FF"
                }
            };

            todoItems = new List<TodoItem>();
        }

        private bool IsTagExists(TagItem tagItem)
        {
            var exist = tags.Where(t => t.Title == tagItem.Title).FirstOrDefault();
            return exist == null ? false : true;
        }

        private bool IsTodoItemExists(TodoItem todoItem)
        {
            var exist = todoItems.Where(t => t.Id == todoItem.Id)
                                 .FirstOrDefault();
            return exist == null ? false : true;
        }

        #endregion

        #region IDataService implementation

        public async Task CreateTagItemAsync(TagItem tagItem)
        {
            await Task.Factory.StartNew(() =>
            {
                if (tagItem == null)
                    throw new ArgumentNullException("TagItem is null");
                if (IsTagExists(tagItem))
                    throw new Exception($"Tag ({tagItem.Title}) is already existed");

                tags.Add(tagItem);
            });
        }

        public async Task CreateTodoItemAsync(TodoItem todoItem)
        {
            await Task.Factory.StartNew(() =>
            {
                if (todoItem == null)
                    throw new ArgumentNullException("Todo item is null");
                
                todoItems.Add(todoItem);
            });
        }

        public async Task<IEnumerable<TagItem>> GetTagItemsAsync()
        {
            return await Task.Factory.StartNew(() =>
            {
                var tagItems = tags.ToList();

                foreach (var tag in tagItems)
                {
                    tag.TodoItems = todoItems.Where(t => t.TagId == tag.Id && t.Status == TodoStatus.InProcess)
                                             .OrderByDescending(t => t.Priority)
                                             .OrderBy(t => t.DueTime).ToList();
                }

                return tagItems;
            }); 
        }

        public async Task<IEnumerable<TodoItem>> GetTodoItemsAsync()
        {
            return await Task.Factory.StartNew(() =>
            {
                return todoItems.Where(t => t.Status == TodoStatus.InProcess)
                                .OrderByDescending(t => t.Priority)
                                .OrderBy(t => t.DueTime).ToList();
            });
        }

        public async Task<IEnumerable<TodoItem>> GetTodoItemsAsync(TagItem tagItem)
        {
            return await Task.Factory.StartNew(() =>
            {
                return todoItems.Where(t => t.TagId == tagItem.Id && t.Status == TodoStatus.InProcess)
                                .OrderByDescending(t => t.Priority)
                                .OrderBy(t => t.DueTime).ToList();
            });
        }

        public async Task<IEnumerable<TodoItem>> GetTodoItemsAsync(DateTime date)
        {
            return await Task.Factory.StartNew(() =>
            {
                return todoItems.Where(t => t.DueTime.Value.Date == date.Date && 
                                        t.Status == TodoStatus.InProcess)
                                .OrderByDescending(t => t.Priority)
                                .OrderBy(t => t.DueTime).ToList();
            });
        }

        public async Task RemoveTagItemAsync(TagItem tagItem)
        {
            await Task.Factory.StartNew(() =>
            {
                if (tagItem == null) throw new ArgumentNullException("Tag item is null");
                if (!IsTagExists(tagItem))
                    throw new Exception($"Tag {tagItem.Title} is not existed");

                tags.Remove(tagItem);
            });
        }

        public async Task RemoveTodoItemAsync(TodoItem todoItem)
        {
            await Task.Factory.StartNew(() =>
            {
                if (todoItem == null)
                    throw new ArgumentNullException("Todo item is null");
                if (!IsTodoItemExists(todoItem))
                    throw new Exception($"Tag {todoItem.Id} is not existed");

                todoItems.Remove(todoItem);
            });
        }


        public Task UpdateTagItemAsync(TagItem todoItem)
        {
            throw new NotImplementedException();
        }

        public Task UpdateTodoItemAsync(TodoItem todoItem)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
