using System;
using System.Runtime.InteropServices;
using BackgroundTasks;
using Foundation;

namespace CyberWarriorUA.iOS.Helpers
{
    public static class BackgroundTasksHelpers
    {
        [DllImport("/usr/lib/libobjc.dylib", EntryPoint = "objc_msgSend")]
        public static extern void Intptr_objc_msgSend(IntPtr receiver, IntPtr selector, IntPtr str);

        public static void ForceStartBackgroundtask(this BGTaskScheduler bGTaskScheduler, string taskId)
        {
            var selectorStr = "_simulateLaunchForTaskWithIdentifier:";

            CallObjC(bGTaskScheduler, taskId, selectorStr);

        }

        public static void ForceFinishTask(this BGTaskScheduler bGTaskScheduler, string taskId)
        {
            var selectorStr = "_simulateExpirationForTaskWithIdentifier:";

            CallObjC(bGTaskScheduler, taskId, selectorStr);
        }

        private static void CallObjC(BGTaskScheduler bGTaskScheduler, string taskId, string selectorStr)
        {
            var selector = new ObjCRuntime.Selector(selectorStr);

            var nsId = new NSString(taskId);

            Intptr_objc_msgSend(BGTaskScheduler.Shared.Handle, selector.Handle, nsId.Handle);
        }
    }
}
