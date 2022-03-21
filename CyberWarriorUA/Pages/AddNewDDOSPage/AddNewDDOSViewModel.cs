using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using CyberWarriorUA.Models;
using CyberWarriorUA.MVVM;
using Prism.Navigation;
using Realms;
using Xamarin.CommunityToolkit.ObjectModel;

namespace CyberWarriorUA.Pages.AddNewDDOSPage
{
    public class AddNewDDOSViewModel : BaseViewModel
    {
        private IMapper _mapper;

        internal static string NavigationParam = "data";

        private ICommand? _closePageCommand;
        public ICommand ClosePageCommand => _closePageCommand
            ??= new AsyncCommand(ClosePage);

        private ICommand _addNewDDOSCommand;
        public ICommand AddNewDDOSCommand => _addNewDDOSCommand
            ??= new AsyncCommand(AddNewDDOS, CanAddNew);

        public ObservableCollection<string> Protocols { get; set; }

        public AttackInfo AttackModel { get; set; }

        public string Protocol { get; set; }

        public ObservableCollection<string> HttpMethods { get; set; }
        public string HttpMethod { get; set; }

        public bool IsUpdate { get; set; }

        public AddNewDDOSViewModel(INavigationService navigationService,
            IMapper mapper) : base(navigationService)
        {
            _mapper = mapper;

            var protocols = Enum.GetNames(typeof(EProtocolType));
            var req = new HttpRequestMessage();

            var httpMethods = Enum.GetNames(typeof(EHttpMethod));
            Protocols = new(protocols);
            Protocol = Protocols.First();
            HttpMethods = new(httpMethods);
            AttackModel = new();
            AttackModel.PropertyChanged += AttackModel_PropertyChanged;

            HttpMethod = HttpMethods.First();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey(NavigationParam) && parameters[NavigationParam] is AttackInfo attackModel)
            {
                //var it = parameters[NavigationParam];
                //int iii = 0;
                AttackModel = attackModel;
                IsUpdate = true;
                ((AsyncCommand)AddNewDDOSCommand)?.RaiseCanExecuteChanged();

            }
        }

        private void AttackModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ((AsyncCommand)AddNewDDOSCommand)?.RaiseCanExecuteChanged();
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (AttackModel is not null)
            {

                if (args.PropertyName == nameof(Protocol) && !string.IsNullOrWhiteSpace(Protocol))
                {
                    var enumValue = (EProtocolType)Enum.Parse(typeof(EProtocolType), Protocol);
                    AttackModel.Protocol = (int)enumValue;
                }

                if (args.PropertyName == nameof(HttpMethod) && !string.IsNullOrWhiteSpace(HttpMethod))
                {
                    var enumValue = (EHttpMethod)Enum.Parse(typeof(EHttpMethod), HttpMethod);
                    AttackModel.HttpMethod = (int)enumValue;
                }
            }
        }

        private Task ClosePage()
        {
            return NavigationService.GoBackAsync(null, true, true);
        }

        private async Task AddNewDDOS()
        {
            var realm = Realm.GetInstance();

            await realm.WriteAsync(t =>
            {
                var realmObjc = _mapper.Map<AttackModel>(AttackModel);
                t.Add(realmObjc, IsUpdate);
            });

            await NavigationService.GoBackAsync(null, true, true);
        }

        private bool CanAddNew(object? arg)
        {
            return !string.IsNullOrWhiteSpace(AttackModel.URL)
                   && !string.IsNullOrWhiteSpace(AttackModel.Message)
                   && !string.IsNullOrWhiteSpace(AttackModel.Title);
        }
    }
}
