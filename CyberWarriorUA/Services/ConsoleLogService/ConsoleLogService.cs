using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using DynamicData;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;

namespace CyberWarriorUA.Services.ConsoleLogService
{
    public class ConsoleLogService : IConsoleLogService
    {

        // public ObservableRangeCollection<string> Collection { get; set; }
        private SourceList<string> _collection = new SourceList<string>();
        
        public ConsoleLogService()
        {
          //  Collection = new();
        }

        private SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        public async Task AddLine(string text)
        {
            await semaphoreSlim.WaitAsync();

            try
            {
                var textToAdd = $"[{DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss}")}]:{text}";
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    _collection.Add(textToAdd);
                });
               // Collection.Add(text);
            } catch
            {

            } finally
            {
                semaphoreSlim.Release();
            }
        }

        public IDisposable Connect(out ReadOnlyObservableCollection<string> readOnly)
        {
            return _collection.Connect()
                               .RefCount()
                               .Bind(out readOnly)
                               .DisposeMany()
                               .Subscribe();
        }
    }
}
