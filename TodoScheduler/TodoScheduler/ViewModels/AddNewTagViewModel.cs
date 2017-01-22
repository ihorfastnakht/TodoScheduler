using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using TodoScheduler.Data;
using TodoScheduler.Infrastructure.Base;
using TodoScheduler.Infrastructure.Services.DataServices;
using TodoScheduler.Infrastructure.Services.UserDialogServices;
using TodoScheduler.Models;
using Xamarin.Forms;

namespace TodoScheduler.ViewModels
{
    public class AddNewTagViewModel : ViewModelBase
    {
        #region members

        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;

        #endregion

        #region fields & properties

        bool _isValid = false;
        public bool IsValid {
            get { return _isValid; }
            set { SetProperty(ref _isValid, value); }
        }

        string _title;
        public string Title {
            get { return _title; }
            set
            {
                SetProperty(ref _title, value);
                IsValid = !string.IsNullOrWhiteSpace(Title);
            }
        }

        string _tagColor;
        public string TagColor {
            get { return _tagColor; }
            set { SetProperty(ref _tagColor, value); }
        }

        IEnumerable<HexColor> _hexColors;
        public IEnumerable<HexColor> HexColors {
            get { return _hexColors; }
            set { SetProperty(ref _hexColors, value); }
        }

        HexColor _selectedColor;
        public HexColor SelectedColor {
            get { return _selectedColor; }
            set
            {
                if (SetProperty(ref _selectedColor, value))
                    TagColor = SelectedColor.HexValue;
            }
        }
        #endregion

        #region constrcutor

        public AddNewTagViewModel(IDataService dataService, IDialogService dialogService)
        {
            _dataService = dataService;
            _dialogService = dialogService;
        }

        #endregion

        #region commands

        ICommand _saveCommand;
        public ICommand SaveCommand {
            get { return _saveCommand ?? new Command(SaveCommandExecute); }
            set { SetProperty(ref _saveCommand, value); }
        }

        ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get { return _cancelCommand ?? new Command(CancelCommandExecute); }
            set { SetProperty(ref _cancelCommand, value); }
        }

        #endregion

        #region private

        private async void CancelCommandExecute()
        {
            await Navigation.CloseAsync(animation: true);
        }

        private async void SaveCommandExecute()
        {
            try
            {
                var tagItem = new TagItem(){
                    Title = this.Title,
                    TagColor = this.TagColor
                };

                await _dataService.CreateTagItemAsync(tagItem);
                await _dialogService.ShowPopup($"Tag ({tagItem.Title}) has been created");
                //go back
                CancelCommandExecute();
            }
            catch (Exception ex)
            {
                await _dialogService.ShowErrorMessage("Oops", ex.Message, "OK");
            }
        }

        #endregion

        #region override

        public override void Init(Dictionary<string, object> parameters = null)
        {
            base.Init(parameters);

            HexColors = ColorFactory.Colors;
            SelectedColor = HexColors.ToList()[0];
        }

        #endregion
    }
}
