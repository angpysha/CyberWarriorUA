using System;
using System.Collections.ObjectModel;
using CyberWarriorUA.MVVM;
using CyberWarriorUA.Services.ConsoleLogService;
using Prism.Navigation;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace CyberWarriorUA.Pages.ConsoleLogPage
{
    public class ConsoleLogViewModel : BaseViewModel
    {
        private readonly IConsoleLogService _consoleLogService;

        public ObservableRangeCollection<string> Console { get; set; }

        private ReadOnlyObservableCollection<string> _items;
        public ReadOnlyObservableCollection<string> Items => _items;

        public ConsoleLogViewModel(INavigationService navigationService,
            IConsoleLogService consoleLogService) : base(navigationService)
        {
            _consoleLogService = consoleLogService;

            _consoleLogService.Connect(out _items);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }
    }
}

