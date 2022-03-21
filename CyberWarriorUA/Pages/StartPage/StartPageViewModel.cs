using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using CyberWarriorUA.Models;
using CyberWarriorUA.MVVM;
using CyberWarriorUA.Pages.AddNewDDOSPage;
using CyberWarriorUA.Services;
using Prism.Navigation;
using Prism.Services;
using Realms;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CyberWarriorUA.Pages.StartPage
{
    public class StartPageViewModel : BaseViewModel
    {
        private readonly IPageDialogService _pageDialogService;
        private readonly IMapper _mapper;

        private ICommand? _openAddNewDDOSPPageCommand;
        public ICommand OpenAddNewDDOSPPageCommand => _openAddNewDDOSPPageCommand
            ??= new AsyncCommand(OpenAddNewDDOSPPage);

        private ICommand _switchChangedCommand;
        public ICommand SwitchChangedCommand => _switchChangedCommand
            ??= new Command<AttackInfo>(SwitchChanged);

        private ICommand _startCommand;
        public ICommand StartCommand => _startCommand
            ??= new Command(Start);

        private ICommand _editCommand;
        public ICommand EditCommand => _editCommand ??=
            new AsyncCommand<AttackInfo>(Edit);

        private ICommand _deleteCommand;
        public ICommand DeleteCommand => _deleteCommand ??=
            new AsyncCommand<AttackInfo>(Delete);

        private void Start(object obj)
        {
            if (!IsRunning)
            {
                _backgroundDDoSer.StartDDoSer();
            }
            else
            {
                _backgroundDDoSer.StopDDoSer();
            }
        }

        public ObservableRangeCollection<AttackInfo> AttackModels { get; set; }

        public bool IsRunning { get; set; }

        private readonly IBackgroundDDoSer _backgroundDDoSer;

        public StartPageViewModel(INavigationService navigationService,
            IBackgroundDDoSer backgroundDDoSer, IPageDialogService pageDialogService,
            IMapper mapper) : base(navigationService)
        {
            _pageDialogService = pageDialogService;
            AttackModels = new();
            _backgroundDDoSer = backgroundDDoSer;
            _mapper = mapper;
            _backgroundDDoSer.OnRunningChanged += backgroundDDoSer_OnRunningChanged;
        }

        private void backgroundDDoSer_OnRunningChanged(object sender, bool e)
        {
            MainThread.BeginInvokeOnMainThread(() => IsRunning = e);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            var realm = Realm.GetInstance();
            var itemsLocal = realm.All<AttackModel>().ToList();

            var items = _mapper.Map<List<AttackInfo>>(itemsLocal);

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

        private void SwitchChanged(AttackInfo obj)
        {
            var realm = Realm.GetInstance();
            var realmItem = realm.All<AttackModel>().ToList().FirstOrDefault(x => x.URL == obj.URL && x.Port == obj.Port);

            if (realmItem is not null)
            {

                realm.Write(() =>
                {

                    realmItem.IsActive = !realmItem.IsActive;

                    realm.Add(realmItem, true);

                });
            }

        }

        private async Task Delete(AttackInfo? arg)
        {
            var shouldDelete = await _pageDialogService.DisplayAlertAsync("Item delete", $"Are you sure to delete {arg.Title}?", "Yes", "No");

            if (shouldDelete)
            {
                var realm = Realm.GetInstance();

                var realmItem = realm.All<AttackModel>().ToList().FirstOrDefault(x => x.URL == arg.URL && x.Port == arg.Port);
                

                realm.Write(() =>
                {
                    realm.Remove(realmItem);
                });
            }
        }

        private Task Edit(AttackInfo? arg)
        {
            var navParams = new NavigationParameters();

            navParams.Add(AddNewDDOSViewModel.NavigationParam, arg);

            return NavigationService.NavigateAsync(nameof(AddNewDDOSPage.AddNewDDOSPage), navParams, true, true);

        }
    }
}
