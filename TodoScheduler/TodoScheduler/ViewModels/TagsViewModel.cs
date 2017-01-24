using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using TodoScheduler.Base;
using TodoScheduler.Enums;
using TodoScheduler.Models;
using TodoScheduler.Services.DataServices;
using TodoScheduler.Services.DialogServices;
using Xamarin.Forms;

namespace TodoScheduler.ViewModels
{
    public class TagsViewModel : ViewModelBase
    {
        #region members

        readonly IDataService _dataService;
        readonly IDialogService _dialogService;

        #endregion

        #region fields & properties

        IEnumerable<TagItem> _tagItems;
        public IEnumerable<TagItem> TagItems
        {
            get { return _tagItems; }
            set { SetProperty(ref _tagItems, value); }
        }

        #endregion

        #region constructor

        public TagsViewModel(IDataService dataService, IDialogService dialogService)
        {
            _dataService = dataService;
            _dialogService = dialogService;

            MessagingCenter.Subscribe<CreateTagViewModel>(this, "refresh", (sender) => LoadTagItems());
        }

        #endregion

        #region commands

        ICommand _removeTagCommand;
        public ICommand RemoveTagCommand {
            get { return _removeTagCommand ?? new Command<TagItem>(RemoveTagCommandExecute); }
            set { SetProperty(ref _removeTagCommand, value); }
        }

        ICommand _createTagCommand;
        public ICommand CreateTagCommand
        {
            get { return _createTagCommand ?? new Command(CreateTagCommandExecute); }
            set { SetProperty(ref _createTagCommand, value); }
        }

        ICommand _addTodoCommand;
        public ICommand AddTodoCommand {
            get { return _addTodoCommand ?? new Command<TagItem>(AddTodoCommandExecute); }
            set { SetProperty(ref _addTodoCommand, value); }
        }

        ICommand _detailCommand;
        public ICommand DetailCommand {
            get { return _detailCommand ?? new Command<TagItem>(DetailCommandExecute); }
            set { SetProperty(ref _detailCommand, value); }
        }

        #endregion

        #region private

        private async void LoadTagItems()
        {
            if (State == VmState.Busy)
                return;
            State = VmState.Busy;
            try
            {
                var items = await _dataService.GetTagItemsAsync();

                if (items.Any())
                {
                    State = VmState.Normal;
                    TagItems = items;
                }
                else
                    State = VmState.NoData;
            }
            catch (Exception ex)
            {
                await _dialogService.ShowErrorMessageAsync("Opps.", ex.Message);
            }
        }

        private async void RemoveTagCommandExecute(TagItem tag)
        {
            try
            {
                if (State == VmState.Busy) return;
          
                var result = await _dialogService.ShowConfirmMessageAsync("Confirm", $"Remove tag ({tag.Title}) permanently?");

                if (result)
                {
                    await _dataService.RemoveTagItemAsync(tag);
                    LoadTagItems();
                    await _dialogService.ShowToastMessageAsync($"Tag ({tag.Title}) has been removed", TimeSpan.FromSeconds(2));
                }
            }
            catch (Exception ex)
            {
                await _dialogService.ShowErrorMessageAsync("Oops.", ex.Message);
            }
        }

        private async void CreateTagCommandExecute() => await Navigation.NavigateAsync(typeof(CreateTagViewModel), animation: true);

        private async void AddTodoCommandExecute(TagItem tag)
        {
            if (tag == null)
                return;

            var parameters = new Dictionary<string, object>() { ["tag"] = tag };
            await Navigation.NavigateAsync(typeof(CreateTodoViewModel), parameters, animation: true);
        }

        private async void DetailCommandExecute(TagItem tag)
        {
            if (tag == null)
                return;

            var parameters = new Dictionary<string, object>() { ["tag"] = tag };
            await Navigation.NavigateAsync(typeof(TagDetailViewModel), parameters, animation: true);
        }
        #endregion

        #region override

        public override void Init(Dictionary<string, object> parameters = null)
        {
            base.Init(parameters);
            LoadTagItems();
        }

        #endregion
    }
}