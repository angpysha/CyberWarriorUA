using System;
using Prism.Mvvm;
using Prism.Navigation;

namespace CyberWarriorUA.MVVM
{
    public class BaseViewModel : BindableBase, INavigationAware, IInitialize
    {
        protected INavigationService NavigationService { get; }

        public BaseViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public virtual void Initialize(INavigationParameters parameters)
        {
            
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
            
        }
    }
}
