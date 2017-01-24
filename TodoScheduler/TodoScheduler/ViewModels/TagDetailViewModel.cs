using System;
using System.Collections.Generic;
using System.Windows.Input;
using TodoScheduler.Base;
using TodoScheduler.Enums;
using TodoScheduler.Models;
using TodoScheduler.Services.DataServices;
using TodoScheduler.Services.DialogServices;
using Xamarin.Forms;

namespace TodoScheduler.ViewModels
{
    public class TagDetailViewModel : ViewModelBase
    {
        #region members

        readonly IDataService _dataService;
        readonly IDialogService _dialogService;

        #endregion

        #region fileds & properties

        TagItem _tag;
        public TagItem Tag {
            get { return _tag; }
            set { SetProperty(ref _tag, value); }
        }

        #endregion

        #region constructor

        public TagDetailViewModel(IDataService dataService, IDialogService dialogService)
        {
            _dataService = dataService;
            _dialogService = dialogService;
        }

        #endregion

        #region commands

        ICommand _addTodoCommand;
        public ICommand AddTodoCommand {
            get { return _addTodoCommand ?? new Command(AddTodoCommandExecute); }
            set { SetProperty(ref _addTodoCommand, value); }
        }


        #endregion

        #region private

        private void AddTodoCommandExecute()
        {
            //throw new NotImplementedException();
        }

        

        #endregion

        #region override

        public override void Init(Dictionary<string, object> parameters = null)
        {
            base.Init(parameters);

            if (parameters != null)
            {
                if (!parameters.ContainsKey("tag"))
                    return;

                var tag = (TagItem)parameters["tag"];
                Header = $"{tag.Title} todo items";

                if (!tag.HasItems)
                    State = VmState.NoData;
                else
                {
                    Tag = tag;
                    State = VmState.Normal;
                }
            }
        }

        #endregion
    }
}
