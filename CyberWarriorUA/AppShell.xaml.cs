using System;
using System.Collections.Generic;
using CyberWarriorUA.ViewModels;
using CyberWarriorUA.Views;
using Xamarin.Forms;

namespace CyberWarriorUA
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}
