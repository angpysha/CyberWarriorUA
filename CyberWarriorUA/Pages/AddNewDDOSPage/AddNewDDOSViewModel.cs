using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using CyberWarriorUA.Models;
using CyberWarriorUA.MVVM;
using Prism.Navigation;
using Realms;
using Xamarin.CommunityToolkit.ObjectModel;

namespace CyberWarriorUA.Pages.AddNewDDOSPage
{
    public class AddNewDDOSViewModel : BaseViewModel
    {
        private ICommand? _closePageCommand;
        public ICommand ClosePageCommand => _closePageCommand
            ??= new AsyncCommand(ClosePage);

        private ICommand _addNewDDOSCommand;
        public ICommand AddNewDDOSCommand => _addNewDDOSCommand
            ??= new AsyncCommand(AddNewDDOS, CanAddNew);

        public ObservableCollection<string> Protocols { get; set; }

        public AttackModel AttackModel { get; set; }

        public string Protocol { get; set; }

        public ObservableCollection<string> HttpMethods { get; set; }
        public string HttpMethod { get; set; }

        public AddNewDDOSViewModel(INavigationService navigationService) : base(navigationService)
        {
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

        private void AttackModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ((AsyncCommand)AddNewDDOSCommand)?.RaiseCanExecuteChanged();
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (AttackModel is not null) {

                if (args.PropertyName == nameof(Protocol) && !string.IsNullOrWhiteSpace(Protocol))
                {
                    var enumValue = (EProtocolType)Enum.Parse(typeof(EProtocolType), Protocol);
                    AttackModel.Protocol = (int) enumValue;
                }

                if (args.PropertyName == nameof(HttpMethod) && !string.IsNullOrWhiteSpace(HttpMethod))
                {
                    var enumValue = (EHttpMethod)Enum.Parse(typeof(EHttpMethod), HttpMethod);
                    AttackModel.HttpMethod = (int) enumValue;
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
                t.Add(AttackModel);
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
