using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        private IEnumerable<TagItem> _originalTags;

        IEnumerable<TagItem> _tagItems;
        public IEnumerable<TagItem> TagItems
        {
            get { return _tagItems; }
            set { SetProperty(ref _tagItems, value); }
        }

        string _searchedText;
        public string SearchedText {
            get { return _searchedText; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    TagItems = _originalTags;

                if (SetProperty(ref _searchedText, value)) {
                    TagItems = _originalTags.Where(t => t.Title.ToLower()
                                            .StartsWith(SearchedText.ToLower()));

                }
            }
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

        ICommand _refreshCommand;
        public ICommand RefreshCommand
        {
            get { return _refreshCommand ?? new Command(() => LoadTagItems()); }
            set { SetProperty(ref _refreshCommand, value); }
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
                TagItems = null;
#if DEBUG
                await Task.Delay(500);
#endif
                var items = await _dataService.GetTagItemsAsync();

                if (items.Any())
                {
                    TagItems = items;
                    _originalTags = items;

                    State = VmState.Normal;
                }
                else
                    State = VmState.NoData;
            }
            catch (Exception ex)
            {
                await _dialogService.ShowErrorMessageAsync("Oops", ex.Message);
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
                await _dialogService.ShowErrorMessageAsync("Oops", ex.Message);
            }
        }

        private async void CreateTagCommandExecute() => await Navigation.NavigateAsync(typeof(CreateTagViewModel));

        private async void AddTodoCommandExecute(TagItem tag)
        {
            if (tag == null)
                return;

            var parameters = new Dictionary<string, object>() { ["tag"] = tag };
            await Navigation.NavigateAsync(typeof(CreateTodoViewModel), parameters);
        }

        private async void DetailCommandExecute(TagItem tag)
        {
            if (tag == null)
                return;

            var parameters = new Dictionary<string, object>() { ["tag"] = tag };
            await Navigation.NavigateAsync(typeof(TagDetailViewModel), parameters);
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
            LoadTagItems();
        }

        public override void Disappearing()
        {
            base.Disappearing();
            SearchedText = string.Empty;
            TagItems = null;
        }

        #endregion
    }
}