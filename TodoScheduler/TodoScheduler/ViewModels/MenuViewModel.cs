using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using TodoScheduler.Infrastructure.Base;
using Xamarin.Forms;
using m = TodoScheduler.PageModels;

namespace TodoScheduler.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        #region members & properties

        IEnumerable<Grouping<m.MenuGroup, m.MenuItem>> _menuGroups;
        public IEnumerable<Grouping<m.MenuGroup, m.MenuItem>> MenuGroups {
            get { return _menuGroups; }
            set { SetProperty(ref _menuGroups, value); }
        }

        #endregion

        #region constructor

        public MenuViewModel()
        {
            InitMenuGroups();
        }

        #endregion

        #region commands

        private ICommand _selectMenuCommand;
        public ICommand SelectMenuCommand {
            get { return _selectMenuCommand ?? new Command<MenuItem>(SelectMenuCommandExecute); }
            set { SetProperty(ref _selectMenuCommand, value); }
        }

        #endregion

        #region private

        private void InitMenuGroups()
        {
            //menu groups
            var tagGroup = new m.MenuGroup() { Id = 1, Icon = "tags.png", Title = "Tag items" };
            var todoGroup = new m.MenuGroup() { Id = 2, Icon = "todos.png", Title = "Todo items" };
            var configGroup = new m.MenuGroup() { Id = 3, Icon = "settings.png", Title = "Settings" };

            var menuItems = new List<m.MenuItem>()
            {
                //tag
                new m.MenuItem() { MenuGroup = tagGroup, Icon = "", Title = "" },
                //todo
                new m.MenuItem() { MenuGroup = todoGroup, Icon = "today.png", Title = "Today" },
                new m.MenuItem() { MenuGroup = todoGroup, Icon = "tomorrow.png", Title = "Tomorrow" },
                new m.MenuItem() { MenuGroup = todoGroup, Icon = "schedule.png", Title = "Schedule" },
                //configuration
                new m.MenuItem() { MenuGroup = configGroup, Icon = "config.png", Title = "Configuration" },
                new m.MenuItem() { MenuGroup = configGroup, Icon = "about.png", Title = "About" },
            };

            //grouping menu by menu header
            var groupedMenu = from menu in menuItems
                              orderby menu.MenuGroup.Id
                              group menu by menu.MenuGroup into grouped
                              select new Grouping<m.MenuGroup, m.MenuItem>(grouped.Key, grouped);

            MenuGroups = new ObservableCollection<Grouping<m.MenuGroup, m.MenuItem>>(groupedMenu);
        }

        private void SelectMenuCommandExecute(MenuItem menuItem)
        {
            throw new NotImplementedException();
            //if (menuItem == null) return;
            //navigate to detail 
        }

        #endregion
    }
}
