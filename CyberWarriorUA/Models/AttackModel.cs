using System;
using Prism.Mvvm;
using Realms;

namespace CyberWarriorUA.Models
{
    public class AttackModel : RealmObject
    {
        public string? Title { get; set; }
        public string? URL { get; set; }
        public int Protocol { get; set; } = (int) EProtocolType.HTTP;
        public int Port { get; set; } = 80;
        public string? Message { get; set; } = "Путін хуйло!!!! Рускій ваєний корабль, іді нахуй!!!!";
        public int NumThreads { get; set; } = 100;
        public int Timeout { get; set; } = 3600;
        public int HttpMethod { get; set; } = (int)EHttpMethod.Get;
        public bool IsActive { get; set; }
        public bool IsHttps { get; set; }
    }
}
