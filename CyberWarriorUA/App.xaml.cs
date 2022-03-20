using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CyberWarriorUA.Services;
using CyberWarriorUA.Views;
using Prism.DryIoc;
using Prism.Ioc;
using CyberWarriorUA.Pages.MainTabbedPage;
using CyberWarriorUA.Pages.StartPage;
using CyberWarriorUA.Pages.AddNewDDOSPage;
using Xamarin.CommunityToolkit.Helpers;

namespace CyberWarriorUA
{
    public partial class App : PrismApplication
    {

        public App()
        {

            DependencyService.Register<MockDataStore>();
           
            /// MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainTabbedPage, MainTabbedPageViewModel>();
            containerRegistry.RegisterForNavigation<StartPage, StartPageViewModel>();
            containerRegistry.RegisterForNavigation<AddNewDDOSPage, AddNewDDOSViewModel>();
        }

        protected override void OnInitialized()
        {
            InitializeComponent();

            LocalizationResourceManager.Current.PropertyChanged += (_, _) => AppResources.Culture = LocalizationResourceManager.Current.CurrentCulture;
            LocalizationResourceManager.Current.Init(AppResources.ResourceManager);
           // LocalizationResourceManager.Current.CurrentCulture = new CultureInfo("uk");
            NavigationService.NavigateAsync(nameof(MainTabbedPage));
        }
    }
}
