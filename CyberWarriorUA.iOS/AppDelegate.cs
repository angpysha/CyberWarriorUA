using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using BackgroundTasks;
using CyberWarriorUA.Models;
using CyberWarriorUA.Services;
using Foundation;
using Prism.Ioc;
using UIKit;
using Xamarin.Forms.Platform.iOS;

namespace CyberWarriorUA.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        [DllImport("/usr/lib/libobjc.dylib", EntryPoint = "objc_msgSend")]
        public static extern void Intptr_objc_msgSend(IntPtr receiver, IntPtr selector, IntPtr str);
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());
            RegisterBackgroundTasks();
            var id = "net.petrovskyi.CyberWarriorUA.ddospressor";
            var nsId = new NSString(id);
            var appRefresh = new BGProcessingTaskRequest("net.petrovskyi.CyberWarriorUA.ddospressor");
            appRefresh.EarliestBeginDate = DateTime.Now.ToNSDate();
            BGTaskScheduler.Shared.Submit(appRefresh, out NSError error);
            var selectorStr = "_simulateLaunchForTaskWithIdentifier:";
            var selectorFinish = "_simulateExpirationForTaskWithIdentifier:";
            var selector = new ObjCRuntime.Selector(selectorStr);
            Intptr_objc_msgSend(BGTaskScheduler.Shared.Handle, selector.Handle, nsId.Handle);

            return base.FinishedLaunching(app, options);
        }

        private void RegisterBackgroundTasks()
        {
            var id = 
            BGTaskScheduler.Shared.Register(@"net.petrovskyi.CyberWarriorUA.ddospressor", null, t =>
            {
                var realm = Realms.Realm.GetInstance();
                //Unfotunatuly, relam does not support where for IQuearyable
                var items = realm.All<AttackModel>().ToList().Where(x => x.IsActive).ToList();

                while (true)
                {
                    var container = ContainerLocator.Container;
                    
                    foreach (var item in items) {
                        int numOfThreads = item.NumThreads;
                        WaitHandle[] waitHandles = new WaitHandle[numOfThreads];

                        for (int i = 0; i < numOfThreads; i++)
                        {
                            var j = i;
                            // Or you can use AutoResetEvent/ManualResetEvent
                            var handle = new EventWaitHandle(false, EventResetMode.ManualReset);
                            var thread = new Thread(async () =>
                            {
                                var attacker = item.CreateDDoSer();
                                await attacker.Attack();
                            });
                            waitHandles[j] = handle;
                            thread.Start();
                        }
                        WaitHandle.WaitAll(waitHandles);
                    }
                }
            });
        }

        // override ha
    }
}
