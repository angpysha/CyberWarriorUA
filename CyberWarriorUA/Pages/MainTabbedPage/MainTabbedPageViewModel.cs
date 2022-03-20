using System;
using CyberWarriorUA.MVVM;
using Prism.Navigation;

namespace CyberWarriorUA.Pages.MainTabbedPage
{
    public class MainTabbedPageViewModel : BaseViewModel
    {
        public MainTabbedPageViewModel(INavigationService navigationService) : base(navigationService)
        {
        }
    }
}
