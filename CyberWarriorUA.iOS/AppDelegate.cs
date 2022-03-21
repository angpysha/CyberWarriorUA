using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using BackgroundTasks;
using CyberWarriorUA.iOS.Service;
using CyberWarriorUA.Models;
using CyberWarriorUA.Services;
using CyberWarriorUA.Services.ConsoleLogService;
using Foundation;
using Prism;
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
            LoadApplication(new App(new iOSInitializer()));
            RegisterBackgroundTasks();
            return base.FinishedLaunching(app, options);
        }

        private void RegisterBackgroundTasks()
        {
            var id = 
            BGTaskScheduler.Shared.Register(@"net.petrovskyi.CyberWarriorUA.ddospressor", null, t =>
            {
                var container = ContainerLocator.Container;
                var ddoser = ContainerLocator.Container.Resolve<IBackgroundDDoSer>();
                ddoser.RunBackgroundTask((object)t);
            });
        }

        // override ha
    }

    public class iOSInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IBackgroundDDoSer, BackgroundDDoSer>();
        }
    }
}
