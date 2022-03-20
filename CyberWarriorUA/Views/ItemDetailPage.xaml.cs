using System.ComponentModel;
using Xamarin.Forms;
using CyberWarriorUA.ViewModels;

namespace CyberWarriorUA.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}
