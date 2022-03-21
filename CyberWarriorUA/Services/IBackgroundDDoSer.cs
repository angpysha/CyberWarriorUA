using System;
namespace CyberWarriorUA.Services
{
    public interface IBackgroundDDoSer
    {
        bool IsRunning { get; }
        void StartDDoSer();
        void StopDDoSer();
        void RunBackgroundTask(object task);

        event EventHandler<bool> OnRunningChanged;
    }
}
