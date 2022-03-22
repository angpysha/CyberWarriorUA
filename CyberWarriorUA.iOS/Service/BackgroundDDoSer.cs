using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BackgroundTasks;
using CyberWarriorUA.iOS.Helpers;
using CyberWarriorUA.Models;
using CyberWarriorUA.Services;
using CyberWarriorUA.Services.ConsoleLogService;
using Foundation;
using Prism.Ioc;
using Prism.Mvvm;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms.Platform.iOS;

namespace CyberWarriorUA.iOS.Service
{
    public class BackgroundDDoSer : BindableBase, IBackgroundDDoSer
    {
        private readonly IMapper _mapper;

        private string _id = "net.petrovskyi.CyberWarriorUA.ddospressor";

        private WeakEventManager<bool> _weakManager = new();

        public event EventHandler<bool> OnRunningChanged
        {
            add => _weakManager.AddEventHandler(value);
            remove => _weakManager.RemoveEventHandler(value);
        }

        private DateTimeOffset _startedDate;
        public bool IsRunning { get; private set; }

        private bool _submitted;

        public BackgroundDDoSer(IMapper mapper)
        {
            _mapper = mapper;
        }

        public void StartDDoSer()
        {
            IsRunning = !IsRunning;
            _startedDate = DateTimeOffset.Now;
            if (_submitted == false)
            {
                var appRefresh = new BGProcessingTaskRequest(_id);
                appRefresh.EarliestBeginDate = DateTime.Now.ToNSDate();
                BGTaskScheduler.Shared.Submit(appRefresh, out NSError error);
                _submitted = true;
            }
            BGTaskScheduler.Shared.ForceStartBackgroundtask(_id);
            _weakManager.RaiseEvent(this, IsRunning, nameof(OnRunningChanged));
        }

        public void StopDDoSer()
        {
            IsRunning = !IsRunning;
            BGTaskScheduler.Shared.ForceFinishTask(_id);
            _weakManager.RaiseEvent(this, IsRunning, nameof(OnRunningChanged));

        }

        public async void RunBackgroundTask(object bgTask)
        {
            var realm = Realms.Realm.GetInstance();
            //Unfotunatuly, relam does not support where for IQuearyable
            var itemsLocal = realm.All<AttackModel>().ToList().Where(x => x.IsActive).ToList();

            var items = _mapper.Map<List<AttackInfo>>(itemsLocal);

            while (true)
            {
                var container = ContainerLocator.Container;
                var consoleLogger = container.Resolve<IConsoleLogService>();
                Thread.Sleep(400);
                consoleLogger.AddLine("Наступна ітерація. Продожую атаку");
                var tasks = new List<Task<DDoSInfo>>();

                foreach (var item in items)
                {
                    var now = DateTimeOffset.Now;
                    var diff = now.Subtract(_startedDate);

                    if (diff.Seconds > item.Timeout)
                        continue;

                    int numOfThreads = item.NumThreads;

                    for (int i = 0; i < numOfThreads; i++)
                    {
                        var ddoser = item.CreateDDoSer();
                        var task = ddoser.Attack();
                        tasks.Add(task);
                    }
                }
                Task.WaitAll(tasks.ToArray());
                long sz = 0;

                foreach (var taskItem in tasks)
                {
                    var res = await taskItem;
                    if (res.Received.HasValue)
                    {
                        sz += res.Received.Value;
                    }
                }

                await consoleLogger.AddLine($"Атака завершена: отримано {sz}");
                tasks.ForEach(x => x.Dispose());
                tasks = null;
                GC.Collect();
            }
        }
    }
}
