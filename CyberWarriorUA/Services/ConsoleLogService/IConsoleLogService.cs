using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;

namespace CyberWarriorUA.Services.ConsoleLogService
{
    public interface IConsoleLogService
    {
        // ObservableRangeCollection<string> Collection { get; set; }
        IDisposable Connect(out ReadOnlyObservableCollection<string> readOnly);
        Task AddLine(string text);
    }
}
