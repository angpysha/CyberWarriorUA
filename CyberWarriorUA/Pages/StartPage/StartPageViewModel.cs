using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CyberWarriorUA.Models;
using CyberWarriorUA.MVVM;
using Prism.Navigation;
using Realms;
using Xamarin.CommunityToolkit.ObjectModel;

namespace CyberWarriorUA.Pages.StartPage
{
    public class StartPageViewModel : BaseViewModel
    {
        private ICommand? _openAddNewDDOSPPageCommand;
        public ICommand OpenAddNewDDOSPPageCommand => _openAddNewDDOSPPageCommand
            ??= new AsyncCommand(OpenAddNewDDOSPPage);

        public ObservableRangeCollection<AttackModel> AttackModels { get; set; }

        public StartPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            AttackModels = new();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            var realm = Realm.GetInstance();
            var items = realm.All<AttackModel>().ToList();

            if (items?.Any() == true)
            {
                AttackModels.Clear();
                AttackModels.AddRange(items);
            }
        }

        private Task OpenAddNewDDOSPPage()
        {
            return NavigationService.NavigateAsync(nameof(AddNewDDOSPage.AddNewDDOSPage), null, true, true);
        }
    }
}
