using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TodoScheduler.Infrastructure.Base;
using TodoScheduler.Infrastructure.Enums;
using TodoScheduler.Infrastructure.Services.DataServices;
using TodoScheduler.Infrastructure.Services.UserDialogServices;
using TodoScheduler.Models;
using Xamarin.Forms;

namespace TodoScheduler.ViewModels
{
    public class TagsViewModel : ViewModelBase
    {
        #region members

        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;

        #endregion

        #region fileds & properties

        bool _isRefresh = false;
        public bool IsRefresh {
            get { return _isRefresh; }
            set { SetProperty(ref _isRefresh, value); }
        }

        IEnumerable<TagItem> _tag;
        public IEnumerable<TagItem> Tags {
            get { return _tag; }
            set { SetProperty(ref _tag, value); }
        }

        #endregion

        #region constructor

        public TagsViewModel(IDataService dataService, IDialogService dialogService)
        {
            _dataService = dataService;
            _dialogService = dialogService;
        }

        #endregion

        #region commands

        ICommand _refreshCommand;
        public ICommand RefreshCommand {
            get { return _refreshCommand ?? new Command(RefreshCommandExecute);  }
            set { SetProperty(ref _refreshCommand, value); }
        }

        ICommand _itemTappedCommand;
        public ICommand ItemTappedCommand {
            get { return _itemTappedCommand ?? new Command(ItemTappedCommandExecute); }
            set { SetProperty(ref _itemTappedCommand, value); }
        }

        ICommand _addNewTagCommand;
        public ICommand AddNewTagCommand {
            get { return _addNewTagCommand ?? new Command(AddNewTagCommandExecute); }
            set { SetProperty(ref _addNewTagCommand, value); }
        }

        ICommand _addTodoForTagCommand;
        public ICommand AddTodoForTagCommand {
            get { return _addTodoForTagCommand ?? new Command(AddTodoForTagCommandExecute); }
            set { SetProperty(ref _addTodoForTagCommand, value); }
        }

        ICommand _removeTagCommand;
        public ICommand RemoveTagCommand {
            get { return _removeTagCommand ?? new Command(RemoveTagCommandExecute); }
            set { SetProperty(ref _removeTagCommand, value); }
        }

        ICommand _detailTagCommand;
        public ICommand DetailTagCommand{
            get { return _detailTagCommand ?? new Command(DetailTagCommandExecute); }
            set { SetProperty(ref _detailTagCommand, value); }
        }

        #endregion

        #region private

        private async void LoadTagItems()
        {
            try
            {
                if (State == VmState.Loading)
                    return;

                State = VmState.Loading;

                await Task.Delay(4000); // for test

                var items = await _dataService.GetTagItemsAsync();

                if (items.Any())
                {
                    Tags = items;
                    State = VmState.Normal;
                }
                else
                    State = VmState.NoData;
                
            }
            catch (Exception ex)
            {
                //notify when error
            }
        }

        private void ItemTappedCommandExecute(object item)
        {
            //if (item == null)
            //    return;

            //foreach (var tag in Tags)
            //{
            //    tag.IsSelected = false;
            //    OnPropertyChanged(nameof(tag.IsSelected));
            //}

            //var selectedTag = (SelectableObject<TagItem>)item;
            //selectedTag.IsSelected = true;

            //deselect all tags
            //foreach (var tag in Tags)
            //{
            //    if (tag.Item.Id == selectedTag.Item.Id)
            //        tag.IsSelected = true;
            //    else
            //        tag.IsSelected = false;
            //}

            //OnPropertyChanged(nameof(Tags));
        }

        private async void AddNewTagCommandExecute()
        {
            await Navigation.NavigateAsync(typeof(AddNewTagViewModel), animation: true);
        }

        private async void AddTodoForTagCommandExecute(object obj)
        {
            if (State == VmState.Loading)
                return;

            var tagItem = (TagItem)obj;

            Dictionary<string, object> parameters = new Dictionary<string, object>(){
                ["tag"] = tagItem
            };

            State = VmState.Loading;

            await Navigation.NavigateAsync(typeof(AddNewTodoViewModel), parameters, animation: true);

            State = VmState.Normal;
        }

        private async void RemoveTagCommandExecute(object obj)
        {
            try
            {
                if (State == VmState.Loading)
                    return;

                var tagItem = (TagItem)obj;

                var result = await _dialogService.ShowMessageWithConfirmation("Confirm",
                    $"Remove ({tagItem.Title} with {tagItem.TodoItemsCount} todos) permanently?");

                if (result)
                {
                    await _dataService.RemoveTagItemAsync(tagItem);
                    await _dialogService.ShowPopup($"Tag ({tagItem.Title}) has been removed");

                    //reload tag items
                    LoadTagItems();
                }
            }
            catch (Exception ex)
            {
                await _dialogService.ShowErrorMessage("Oops", ex.Message, "OK");
            }
            finally
            {
                State = VmState.Normal;
            }
        }

        private async void DetailTagCommandExecute(object obj)
        {
            if (State == VmState.Loading)
                return;

            var tagItem = (TagItem) obj;

            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                ["tag"] = tagItem
            };

            State = VmState.Loading;

            await Navigation.NavigateAsync(typeof(TagDetailViewModel), parameters, animation: true);

            State = VmState.Normal;
        }

        private void RefreshCommandExecute()
        {
            IsRefresh = true;

            LoadTagItems();

            IsRefresh = false;
        }

        #endregion

        #region override

        public override void Init(Dictionary<string, object> parameters = null)
        {
            base.Init(parameters);
            LoadTagItems();
        }

        public override void Appearing()
        {
            base.Appearing();
            //LoadTagItems();
        }

        #endregion


    }
}
