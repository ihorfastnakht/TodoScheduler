using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoScheduler.Infrastructure.Base;
using TodoScheduler.Infrastructure.Services.DataServices;
using TodoScheduler.Infrastructure.Services.UserDialogServices;
using TodoScheduler.Models;

namespace TodoScheduler.ViewModels
{
    public class TagDetailViewModel : ViewModelBase
    {
        #region members

        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;

        #endregion

        #region fields & properties

        TagItem _selectedTag;
        public TagItem SelectedTagItem {
            get { return _selectedTag; }
            set { SetProperty(ref _selectedTag, value); }
        }

        #endregion

        #region constructor
        #endregion

        #region commands
        #endregion

        #region private
        #endregion

        #region override

        public override void Init(Dictionary<string, object> parameters = null)
        {
            base.Init(parameters);

            if (parameters != null)
            {
                if (parameters.ContainsKey("tag"))
                {
                    SelectedTagItem = (TagItem)parameters["tag"];
                    Header = $"{SelectedTagItem.Title} todos ({SelectedTagItem.TodoItemsCount})";
                }
            }
        }

        #endregion

    }
}
