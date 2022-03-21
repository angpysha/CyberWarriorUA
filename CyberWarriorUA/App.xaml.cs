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
using CyberWarriorUA.Services.ConsoleLogService;
using CyberWarriorUA.Pages.ConsoleLogPage;
using Prism;
using AutoMapper;

namespace CyberWarriorUA
{
    public partial class App : PrismApplication
    {
        internal const string ConfigUrl = @"https://pastebin.com/raw/95D1jjzy";
        public App()
        {

            DependencyService.Register<MockDataStore>();
           
            /// MainPage = new AppShell();
        }

        public App(IPlatformInitializer platformInitializer) : base(platformInitializer)
        {

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
            containerRegistry.RegisterSingleton<IConsoleLogService, ConsoleLogService>();

            var mapperConfig = AutomapperConfig.CreateMapperConfig();

            containerRegistry.RegisterSingleton<IMapper>(() => mapperConfig.CreateMapper());
            
            containerRegistry.RegisterForNavigation<MainTabbedPage, MainTabbedPageViewModel>();
            containerRegistry.RegisterForNavigation<StartPage, StartPageViewModel>();
            containerRegistry.RegisterForNavigation<AddNewDDOSPage, AddNewDDOSViewModel>();
            containerRegistry.RegisterForNavigation<ConsoleLogPage, ConsoleLogViewModel>();
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
