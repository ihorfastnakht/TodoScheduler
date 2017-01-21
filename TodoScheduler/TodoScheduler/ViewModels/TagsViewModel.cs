using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TodoScheduler.Infrastructure.Base;
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

        IEnumerable<SelectableObject<TagItem>> _tag;
        public IEnumerable<SelectableObject<TagItem>> Tags {
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
                //Busy
                var items = await _dataService.GetTagItemsAsync();
                
                if (items.Any())
                {
                    var selectableItems = from tag in items
                                          select new SelectableObject<TagItem>() { Item = tag, IsSelected = false };
                    Tags = selectableItems;
                    //TagItems = items;
                    //Normal
                }
                else
                {
                    //NoData
                }
            }
            catch (Exception ex)
            {
                //notify when error
            }
        }


        private void ItemTappedCommandExecute(object item)
        {
            if (item == null)
                return;

            foreach (var tag in Tags)
            {
                tag.IsSelected = false;
                OnPropertyChanged(nameof(tag.IsSelected));
            }

            var selectedTag = (SelectableObject<TagItem>)item;
            selectedTag.IsSelected = true;

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

        private void AddNewTagCommandExecute(object obj)
        {
            throw new NotImplementedException();
        }

        private void AddTodoForTagCommandExecute(object obj)
        {
            throw new NotImplementedException();
        }

        private void RemoveTagCommandExecute(object obj)
        {
            throw new NotImplementedException();
        }

        private void DetailTagCommandExecute(object obj)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region override

        public override void Init(Dictionary<string, object> parameters = null)
        {
            base.Init(parameters);
            LoadTagItems();
            _dialogService.ShowMessage("Test", "Test message");
        }

        #endregion


    }
}
