using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using TodoScheduler.Base;
using TodoScheduler.Data;
using TodoScheduler.Enums;
using TodoScheduler.Models;
using TodoScheduler.Services.DataServices;
using TodoScheduler.Services.DialogServices;
using Xamarin.Forms;

namespace TodoScheduler.ViewModels
{
    public class CreateTagViewModel : ViewModelBase
    {
        #region members

        readonly IDataService _dataService;
        readonly IDialogService _dialogService;

        #endregion

        #region fields & properties

        bool _isValid = false;
        public bool IsValid {
            get { return _isValid; }
            private set
            {
                if (SetProperty(ref _isValid, value))
                {
                    ((Command)SaveCommand).CanExecute(value);
                }
            }
        }

        string _title;
        public string Title {
            get { return _title; }
            set
            {
                if (SetProperty(ref _title, value))
                    IsValid = !string.IsNullOrWhiteSpace(Title);
            }
        }

        HexColor _selectedColor;
        public HexColor SelectedColor {
            get { return _selectedColor; }
            set { SetProperty(ref _selectedColor, value); }
        }

        IList<HexColor> _hexColors;
        public IList<HexColor> HexColors
        {
            get { return _hexColors; }
            private set { SetProperty(ref _hexColors, value); }
        }

        #endregion

        #region constructor

        public CreateTagViewModel(IDataService dataService, IDialogService dialogService)
        {
            _dataService = dataService;
            _dialogService = dialogService;
        }

        #endregion

        #region commands

        ICommand _saveCommand;
        public ICommand SaveCommand {
            get { return _saveCommand ?? new Command(() => SaveCommandExecute(), () => IsValid); }
            set { SetProperty(ref _saveCommand, value); }
        }

        ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get { return _cancelCommand ?? new Command(CancelCommandExecute); }
            set { SetProperty(ref _cancelCommand, value); }
        }

        private async void CancelCommandExecute() => await Navigation.CloseAsync(animation: true);

        private async void SaveCommandExecute()
        {
            if (State == VmState.Busy) return;
            try
            {
                State = VmState.Busy;

                var tag = new TagItem() {
                    Title = this.Title, HexColor = this.SelectedColor.HexValue
                };

                await _dataService.CreateTagItemAsync(tag);
                await _dialogService.ShowToastMessageAsync($"Tag ({tag.Title}) has been created", TimeSpan.FromSeconds(2));
                await Navigation.CloseAsync(animation: true);
               
                //Send message for refresh tag items after add
                MessagingCenter.Send(this, "refresh");
            }
            catch (Exception ex)
            {
                await _dialogService.ShowErrorMessageAsync("Oops.", ex.Message);
            }
            finally
            {
                State = VmState.Normal;
            }
        }

        #endregion

        #region override

        public override void Init(Dictionary<string, object> parameters = null)
        {
            base.Init(parameters);
            HexColors = ColorFactory.Colors.ToList();
            SelectedColor = HexColors[0];
        }

        #endregion
    }
}
